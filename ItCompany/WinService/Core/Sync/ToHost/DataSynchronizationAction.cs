using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using Microsoft.Practices.Unity;
using NHibernate.Linq;
using NHibernate.Util;
using Surveys.Core.Configuration;
using Surveys.Core.Model;
using Surveys.Core.Model.Activities;
using Surveys.Core.Model.DTO.Sync;
using Surveys.Core.Model.Events;
using Surveys.Core.Model.Queue;
using Surveys.Core.Repositories;
using Surveys.Data.NHibernate;
using Surveys.Service.Core.AutoMapper;
using Surveys.Service.Core;
using Surveys.Service.Core.DataSynchronizationService;

namespace Surveys.Service.Core.Sync.ToHost
{
    public class DataSynchronizationAction
    {
        private readonly ILog _logger = LogManager.GetLogger<DataSynchronizationAction>();
        private readonly DataSynchronizationServiceClient _client = new DataSynchronizationServiceClient();
        private readonly int _maxObjectSendCount = LocalConfiguration.GetIntSetting("MaxObjectSendCount", 100);

        #region Dependencies

        [Dependency]
        public IQueueTicketRepository QueueTicketRepository { get; set; }

        [Dependency]
        public IDeviceRepository DeviceRepository { get; set; }

        [Dependency]
        public IConversationRepository ConversationRepository { get; set; }

        [Dependency]
        public ISessionRepository SessionRepository { get; set; }

        [Dependency]
        public IUnitEventRepository UnitEventRepository { get; set; }

        [Dependency]
        public DbSessionWrapper DbSessionWrapper { get; set; }

        [Dependency]
        public ICommonMapper Mapper { get; set; }

        #endregion

        public void DeviceSynchronize(int companyId)
        {
            SafeExecute(delegate
            {
                var devices = DeviceRepository.Query().Where(d => d.Company.Id == companyId).ToList();
                SyncEntities<Device, DeviceSyncDto>(devices, (dtos, iterationDevices) => _client.DeviceSynchronize(dtos));
            });
        }

        public void DeviceActivitiesSynchronize(int companyId, int syncIntervalSec)
        {
            SafeExecute(delegate
            {
                List<Device> devices = DeviceRepository.Query().Where(d => d.Company.Id == companyId).ToList();
                List<DeviceStateActivity> deviceActivities = devices.SelectMany(d => 
                    d.Activities.Where(a => 
                        a.EndDate < DateTime.Now.Subtract(TimeSpan.FromSeconds(syncIntervalSec)) &&
                        !a.IsSynchronized && 
                        a.Closed)).ToList();
                SyncEntities<DeviceStateActivity, DeviceStateActivitySyncDto>(deviceActivities, (dtos, iterationActivities) =>
                {
                    _client.DeviceActivitiesSynchronize(dtos);
                    iterationActivities.ForEach(q => q.IsSynchronized = true);
                });
            });
        }

        public void QueueTicketsSynchronize(int companyId, int syncIntervalSec)
        {
            SafeExecute(delegate
            {
                var devicesIds = DeviceRepository.Query().Where(d => d.Company.Id == companyId).Select(d => d.Id).ToList();

                List<QueueTicket> cancelledQueueTickets = QueueTicketRepository.Query().
                    Where(q => !q.IsSynchronized && q.State == QueueTicketState.Cancelled).
                    Where(q => q.RegisterDate.Date < DateTime.Now.Date).
                    Fetch(q => q.Device).
                    Fetch(q => q.Employee).
                    Fetch(q => q.CameraImage).
                    FetchMany(q => q.Conversations).
                    FetchMany(q => q.Activities).ToList();

                List<QueueTicket> queueTickets = QueueTicketRepository.Query()
                    .Where(q => 
                        q.CompleteDate < DateTime.Now.Subtract(TimeSpan.FromSeconds(syncIntervalSec)) &&
                        devicesIds.Contains(q.Device.Id) &&
                        !q.IsSynchronized &&
                        q.State != QueueTicketState.Service &&
                        q.State != QueueTicketState.Waiting &&
                        q.State != QueueTicketState.Delayed &&
                        q.State != QueueTicketState.Cancelled).
                     Fetch(q => q.Device).
                     Fetch(q => q.Employee).
                     Fetch(q => q.CameraImage).
                     FetchMany(q => q.Conversations).
                     FetchMany(q => q.Activities).ToList();

                queueTickets.AddRange(cancelledQueueTickets);

                SyncEntities<QueueTicket, QueueTicketSyncDto>(queueTickets, (dtos, iterationTickets) =>
                {
                    _client.QueueTicketSynchronize(dtos);
                    iterationTickets.ForEach(q =>
                    {
                        q.IsSynchronized = true;
                        q.Conversations.ForEach(c =>
                        {
                            c.IsSynchronized = true;
                            c.Sessions.ForEach(s => { s.IsSynchronized = true; });
                        });
                    });
                });
            });
        }

        public void ConversationsSynchronize(int companyId, int syncIntervalSec)
        {
            SafeExecute(delegate
            {
                var devicesIds = DeviceRepository.Query().Where(d => d.Company.Id == companyId).Select(d => d.Id).ToList();

                IList<Conversation> conversations = ConversationRepository.Query().
                    Where(c =>
                        c.EndDate.HasValue &&
                        c.EndDate.Value < DateTime.Now.Subtract(TimeSpan.FromSeconds(syncIntervalSec)) &&
                        devicesIds.Contains(c.Device.Id) && 
                        c.QueueTicket == null &&
                        !c.IsSynchronized).
                    Fetch(c => c.Device).
                    Fetch(c => c.Employee).ToList();

                SyncEntities<Conversation, ConversationSyncDto>(conversations, (dtos, iterationConversations) =>
                {
                    _client.ConversationSynchronize(dtos);
                    iterationConversations.ForEach(c =>
                    {
                        c.IsSynchronized = true;
                        c.Sessions.ForEach(s => { s.IsSynchronized = true; });
                    });
                });
            });
        }

        public void SessionsSynchronize(int companyId, int syncIntervalSec)
        {
            SafeExecute(delegate
            {
                var devicesIds = DeviceRepository.Query().Where(d => d.Company.Id == companyId).Select(d => d.Id).ToList();

                IList<Session> sessions = SessionRepository.Query().
                    Where(s =>
                        s.ReceiveDate < DateTime.Now.Subtract(TimeSpan.FromSeconds(syncIntervalSec)) &&
                        devicesIds.Contains(s.Device.Id) &&
                        s.Conversation == null &&
                        !s.IsSynchronized).
                    Fetch(c => c.Device).
                    Fetch(c => c.Employee).
                    FetchMany(s => s.DataCollection).ToList();

                SyncEntities<Session, SessionSyncDto>(sessions, (dtos, iterationSessions) =>
                {
                    _client.SessionSynchronize(dtos);
                    iterationSessions.ForEach(s => s.IsSynchronized = true);
                });
            });
        }

        public void UnitEventsSynchronize(int companyId, int syncIntervalSec)
        {
            SafeExecute(delegate
            {
                IList<UnitEvent> unitEvents = UnitEventRepository.Query().Where(e => e.Unit.Company.Id == companyId && !e.IsSynchronized).ToList();

                SyncEntities<UnitEvent, UnitEventDto>(unitEvents, (dtos, iterationUnitEvents) =>
                {
                    _client.UnitEventsSynchronize(dtos);
                    iterationUnitEvents.ForEach(s => s.IsSynchronized = true);
                });
            });
        }

        #region Help methods

        public void SafeExecute(Action syncEntitiesAction)
        {
            try
            {
                DbSessionWrapper.Wrap(syncEntitiesAction);
            }
            catch (Exception exc)
            {
                _logger.ErrorFormat("Unable to synchronize entities", exc);
            }
        }

        private void SyncEntities<TEntity, TEntityDto>(IList<TEntity> entities, Action<TEntityDto[], IList<TEntity>> sendAction)
        {
            _logger.InfoFormat("Synchronizing <{0}> entities: {1}", typeof(TEntity).Name, entities.Count);

            var loopCount = Math.Ceiling((double)entities.Count / _maxObjectSendCount);
            for (var i = 0; i < loopCount; i++)
            {
                IList<TEntity> iterationActivities;

                var startIndex = i * _maxObjectSendCount;
                var endIndex = startIndex + _maxObjectSendCount;
                if (startIndex + _maxObjectSendCount < entities.Count)
                {
                    iterationActivities = entities.Skip(startIndex).Take(_maxObjectSendCount).ToList();
                }
                else
                {
                    iterationActivities = entities.Skip(startIndex).Take(entities.Count - startIndex).ToList();
                    endIndex = startIndex + (entities.Count - startIndex);
                }

                try
                {
                    _logger.InfoFormat("Sending DTO <{0}> from {1} to {2} ", typeof(TEntity).Name, startIndex, endIndex);
                    var iterationDtoObjects = iterationActivities.Select(t => (TEntityDto)Mapper.Map(t, typeof(TEntity), typeof(TEntityDto))).ToList();
                    sendAction(iterationDtoObjects.ToArray(), iterationActivities);
                }
                catch (Exception exc)
                {
                    _logger.ErrorFormat("Unable to send DTO", exc);
                }

                _logger.InfoFormat("Sended DTO <{0}> from {1} to {2} ", typeof(TEntity).Name, startIndex, endIndex);
            }
        }

        #endregion
    }
}
