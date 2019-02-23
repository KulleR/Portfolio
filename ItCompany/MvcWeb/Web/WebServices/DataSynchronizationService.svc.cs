using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using Microsoft.Practices.Unity;
using Surveys.Core;
using Surveys.Core.Helpers;
using Surveys.Core.Model;
using Surveys.Core.Model.Activities;
using Surveys.Core.Model.DTO.Sync;
using Surveys.Core.Model.Events;
using Surveys.Core.Model.Queue;
using Surveys.Core.Repositories;
using Surveys.Web.Host.AutoMapper;
using Surveys.Web.Host.Unity;

namespace Surveys.Web.Host.WebServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "DataSynchronizationService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select DataSynchronizationService.svc or DataRecieverService1.svc.cs at the Solution Explorer and start debugging.
    public class DataSynchronizationService : UnitySupportingService<DataSynchronizationService>, IDataSynchronizationService
    {
        private static readonly ILog _logger = LogManager.GetLogger<DataSynchronizationService>();

        [Dependency]
        public IDeviceStateActivityRepository DeviceStateActivityRepository { get; set; }

        [Dependency]
        public IQueueTicketRepository QueueTicketRepository { get; set; }

        [Dependency]
        public IDeviceRepository DeviceRepository { get; set; }

        [Dependency]
        public IEmployeeRepository EmployeeRepository { get; set; }

        [Dependency]
        public IConversationRepository ConversationRepository { get; set; }

        [Dependency]
        public ISessionRepository SessionRepository { get; set; }

        [Dependency]
        public IUnitEventRepository UnitEventRepository { get; set; }

        [Dependency]
        public IContentStorage LocalContentStorage { get; set; }

        [Dependency]
        public ICommonMapper Mapper { get; set; }

        public void DeviceSynchronize(List<DeviceSyncDto> devicesDto)
        {
            var firstDevice = devicesDto.FirstOrDefault();
            _logger.DebugFormat("GET [{0}] DEVICES FROM {1}", devicesDto.Count, firstDevice != null ? DeviceRepository.Get(firstDevice.Id).Unit?.ToString() : "???");


            foreach (var d in devicesDto)
            {
                var device = DeviceRepository.Get(d.Id);
                if (device != null)
                {
                    device.CurrentEmployee = EmployeeRepository.Get(d.CurrentEmployeeId);
                    device.State = d.State;
                    device.LastActivityDate = d.LastActivityDate;
                    device.IsActivated = d.IsActivated;
                    device.SecurityKey = d.SecurityKey;
                }
            }
        }

        public void DeviceActivitiesSynchronize(List<DeviceStateActivitySyncDto> deviceActivitiesDto)
        {
            try
            {
                var deviceActivities = deviceActivitiesDto.Select(c => (DeviceStateActivity)Mapper.Map(c, typeof(DeviceStateActivitySyncDto), typeof(DeviceStateActivity))).ToList();
                _logger.DebugFormat("GET NEW [{0}] DEVICE ACTIVITIES FROM {1}", deviceActivities.Count, deviceActivities.FirstOrDefault()?.Device?.Unit);
                foreach (var a in deviceActivities)
                {
                    DeviceStateActivityRepository.Save(a);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }

        public void QueueTicketSynchronize(List<QueueTicketSyncDto> queueTicketsDto)
        {
            try
            {
                var queueTickets = queueTicketsDto.Select(c => (QueueTicket)Mapper.Map(c, typeof(QueueTicketSyncDto), typeof(QueueTicket))).ToList();
                _logger.DebugFormat("GET NEW [{0}] QUEUE TICKETS FROM {1}", queueTickets.Count, queueTickets.FirstOrDefault()?.Device?.Unit);
                foreach (var q in queueTickets)
                {
                    // Пропускаем дублированные талоны
                    if (QueueTicketRepository.Query().Any(queueTicket => queueTicket.RegisterDate == q.RegisterDate && queueTicket.Unit.Id == q.Unit.Id))
                    {
                        _logger.DebugFormat("{0} is duplicate. Not saved.", q);
                        continue;
                    }

                    foreach (var c in q.Conversations)
                    {
                        c.QueueTicket = q;
                        var company = c.Device.Company;
                        for (var i = 0; i < c.CameraImages.Count; i++)
                        {
                            c.CameraImages[i].Conversation = c;
                            c.CameraImages[i].StorageType = company.ContentStorageType;
                            LocalContentStorage.Put(c.CameraImages[i], c.CameraImages[i].ContentBinary);
                        }

                        for (var i = 0; i < c.AudioRecords.Count; i++)
                        {
                            c.AudioRecords[i].Conversation = c;
                            c.AudioRecords[i].StorageType = company.ContentStorageType;
                            LocalContentStorage.Put(c.AudioRecords[i], c.AudioRecords[i].ContentBinary);
                        }

                        for (var i = 0; i < c.VideoRecords.Count; i++)
                        {
                            c.VideoRecords[i].Conversation = c;
                            c.VideoRecords[i].StorageType = company.ContentStorageType;
                            LocalContentStorage.Put(c.VideoRecords[i], c.VideoRecords[i].ContentBinary);
                        }

                        foreach (var s in c.Sessions)
                        {
                            for (var i = 0; i < s.CameraImages.Count; i++)
                            {
                                s.CameraImages[i].Session = s;
                                s.CameraImages[i].StorageType = company.ContentStorageType;
                                LocalContentStorage.Put(s.CameraImages[i], s.CameraImages[i].ContentBinary);
                            }

                            for (var i = 0; i < s.AudioRecords.Count; i++)
                            {
                                s.AudioRecords[i].Session = s;
                                s.AudioRecords[i].StorageType = company.ContentStorageType;
                                LocalContentStorage.Put(s.AudioRecords[i], s.AudioRecords[i].ContentBinary);
                            }

                            for (var i = 0; i < c.VideoRecords.Count; i++)
                            {
                                s.VideoRecords[i].Session = s;
                                s.VideoRecords[i].StorageType = company.ContentStorageType;
                                LocalContentStorage.Put(s.VideoRecords[i], s.VideoRecords[i].ContentBinary);
                            }
                        }
                    }

                    foreach (var activity in q.Activities)
                    {
                        activity.QueueTicket = q;
                    }

                    QueueTicketRepository.Save(q);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }

        public void ConversationSynchronize(List<ConversationSyncDto> conversationsDto)
        {
            try
            {
                var conversations = conversationsDto.Select(c => (Conversation)Mapper.Map(c, typeof(ConversationSyncDto), typeof(Conversation))).ToList();
                _logger.DebugFormat("GET NEW [{0}] CONVERSATIONS FROM {1}", conversations.Count, conversations.FirstOrDefault()?.Device?.Unit);
                foreach (var c in conversations)
                {
                    // Пропускаем обслуживания созданные в СУО
                    if (c.Device.LocateSettings().ElectronicQueueEnabled)
                    {
                        _logger.DebugFormat("{0} created in Electronic Queue System. Not saved.", c);
                        continue;
                    }

                    // Пропускаем дублированные обслуживания
                    if (ConversationRepository.Query().Any(conversation => conversation.Guid == c.Guid))
                    {
                        _logger.DebugFormat("{0} is duplicate. Not saved.", c);
                        continue;
                    }

                    var company = c.Device.Company;
                    for (var i = 0; i < c.CameraImages.Count; i++)
                    {
                        c.CameraImages[i].Conversation = c;
                        c.CameraImages[i].StorageType = company.ContentStorageType;
                        LocalContentStorage.Put(c.CameraImages[i], c.CameraImages[i].ContentBinary);
                    }

                    for (var i = 0; i < c.AudioRecords.Count; i++)
                    {
                        c.AudioRecords[i].Conversation = c;
                        c.AudioRecords[i].StorageType = company.ContentStorageType;
                        LocalContentStorage.Put(c.AudioRecords[i], c.AudioRecords[i].ContentBinary);
                    }

                    for (var i = 0; i < c.VideoRecords.Count; i++)
                    {
                        c.VideoRecords[i].Conversation = c;
                        c.VideoRecords[i].StorageType = company.ContentStorageType;
                        LocalContentStorage.Put(c.VideoRecords[i], c.VideoRecords[i].ContentBinary);
                    }
                    ConversationRepository.Save(c);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }

        public void SessionSynchronize(List<SessionSyncDto> sessionsDto)
        {
            try
            {
                var sessions = sessionsDto.Select(c => (Session)Mapper.Map(c, typeof(SessionSyncDto), typeof(Session))).ToList();
                _logger.DebugFormat("GET NEW [{0}] SESSIONS FROM {1}", sessions.Count, sessions.FirstOrDefault()?.Device?.Unit);
                foreach (var s in sessions)
                {
                    // Пропускаем дублированные анкеты
                    if (SessionRepository.Query().Any(session => session.Guid == s.Guid))
                    {
                        _logger.DebugFormat("{0} is duplicate. Not saved.", s);
                        continue;
                    }

                    var company = s.Device.Company;
                    for (var i = 0; i < s.CameraImages.Count; i++)
                    {
                        s.CameraImages[i].Session = s;
                        s.CameraImages[i].StorageType = company.ContentStorageType;
                        LocalContentStorage.Put(s.CameraImages[i], s.CameraImages[i].ContentBinary);

                    }

                    for (var i = 0; i < s.AudioRecords.Count; i++)
                    {
                        s.AudioRecords[i].Session = s;
                        s.AudioRecords[i].StorageType = company.ContentStorageType;
                        LocalContentStorage.Put(s.AudioRecords[i], s.AudioRecords[i].ContentBinary);
                    }

                    for (var i = 0; i < s.VideoRecords.Count; i++)
                    {
                        s.VideoRecords[i].Session = s;
                        s.VideoRecords[i].StorageType = company.ContentStorageType;
                        LocalContentStorage.Put(s.VideoRecords[i], s.VideoRecords[i].ContentBinary);
                    }

                    //Для SQL Server
                    if (s.Survey != null && s.Survey.ActivationDate == DateTime.MinValue)
                    {
                        _logger.DebugFormat("ActivationDate of {0} is DateTime.MinValue. Assigned null", s.Survey);
                        s.Survey.ActivationDate = null;
                    }

                    if (s.ReceiveDate == DateTime.MinValue)
                    {
                        _logger.DebugFormat("ReceiveDate of {0} is DateTime.MinValue. Assigned null", s);
                        s.ReceiveDate = DateTime.Now;
                    }

                    SessionRepository.Save(s);
                }
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }

        public void TestSessionSynchronize(List<TestSessionSyncDto> testSessionsDto)
        {
            throw new System.NotImplementedException();
        }

        public void UnitEventsSynchronize(List<UnitEventDto> unitEventsDto)
        {
            try
            {
                //var unitEvents = unitEventsDto.Select(c => (UnitEvent)Mapper.Map(c, typeof(UnitEventDto), typeof(UnitEvent))).ToList();
                //_logger.DebugFormat("GET NEW [{0}] UNIT LU EVENTS", unitEvents.Count);

                //foreach (var e in unitEvents)
                //{
                //    UnitEventRepository.Save(e);
                //}
            }
            catch (Exception e)
            {
                _logger.Error(e);
            }
        }
    }
}
