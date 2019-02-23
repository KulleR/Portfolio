using System;
using System.Collections.Generic;
using System.Linq;
using Surveys.Core.Model;
using Surveys.Core.Model.Content;
using Surveys.Core.Model.DTO.Sync;
using Surveys.Core.Repositories;
using Microsoft.Practices.Unity;
using Surveys.Core.Helpers;
using Surveys.Core.Model.Queue;
using Surveys.Core.Model.Activities;
using Surveys.Core.Model.DTO;
using Surveys.Core.Model.DTO.Device;
using Surveys.Core.Model.DTO.Script;
using Surveys.Core.Model.Events;
using Surveys.Core.Services.Providers.UnitSchedule;
using Telegram.Bot.Helpers;
using AutoMapperLib = AutoMapper;

namespace Surveys.Core.AutoMapper
{
    public class CommonMapper : ICommonMapper
    {
        #region Dependency

        [Dependency]
        public IContentStorage LocalContentStorage { get; set; }

        [Dependency]
        public IQueueTicketRepository QueueTicketRepository { get; set; }

        [Dependency]
        public IDeviceRepository DeviceRepository { get; set; }

        [Dependency]
        public IEmployeeRepository EmployeRepository { get; set; }

        [Dependency]
        public ISurveyRepository SurveyRepository { get; set; }

        [Dependency]
        public IQuestionRepository QuestionRepository { get; set; }

        [Dependency]
        public ICompanyRepository CompanyRepository { get; set; }

        [Dependency]
        public IResponseRepository ResponseRepository { get; set; }

        [Dependency]
        public IUnitRepository UnitRepository { get; set; }

        [Dependency]
        public IQueueServiceRepository QueueServiceRepository { get; set; }

        [Dependency]
        public ISessionRepository SessionRepository { get; set; }

        [Dependency]
        public IConversationRepository ConversationRepository { get; set; }

        [Dependency]
        public ITestSessionRepository TestSessionRepository { get; set; }

        [Dependency]
        public ISoftwarePackageRepository SoftwarePackageRepository { get; set; }

        #endregion

        public AutoMapperLib.IMapper Mapper { get; set; }

        public CommonMapper()
        {
            AutoMapperLib.MapperConfiguration config = GetConfiguration();
            Mapper = config.CreateMapper();
        }

        public object Map(object source, Type sourceType, Type destinationType)
        {
            return Mapper.Map(source, sourceType, destinationType);
        }

        public T Map<T>(object source) where T : class
        {
            return (T)Mapper.Map(source, source.GetType(), typeof(T));
        }

        #region Private methods

        private AutoMapperLib.MapperConfiguration GetConfiguration()
        {
            return new AutoMapperLib.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Device, DeviceSyncDto>().ForMember(dto => dto.CurrentEmployeeId,
                    mpe => mpe.MapFrom(src => src.CurrentEmployee.Id));

                cfg.CreateMap<DeviceStateActivity, DeviceStateActivitySyncDto>()
                    .ForMember(dto => dto.DeviceId, mpe => mpe.MapFrom(src => src.Device.Id))
                    .ForMember(dto => dto.EmployeeId, mpe => mpe.MapFrom(src => src.Employee.Id));

                cfg.CreateMap<Conversation, ConversationSyncDto>()
                    .ForMember(dto => dto.DeviceId, mpe => mpe.MapFrom(src => src.Device.Id))
                    .ForMember(dto => dto.EmployeeId, mpe => mpe.MapFrom(src => src.Employee.Id));

                cfg.CreateMap<Session, SessionSyncDto>()
                    .ForMember(dto => dto.SurveyId, mpe => mpe.MapFrom(src => src.Survey.Id))
                    .ForMember(dto => dto.DeviceId, mpe => mpe.MapFrom(src => src.Device.Id))
                    .ForMember(dto => dto.EmployeeId, mpe => mpe.MapFrom(src => src.Employee.Id))
                    .ForMember(dto => dto.TimeoutQuestionId, mpe => mpe.MapFrom(src => src.TimeoutQuestion.Id));

                cfg.CreateMap<TestSession, TestSessionSyncDto>()
                    .ForMember(dto => dto.SurveyId, mpe => mpe.MapFrom(src => src.Survey.Id))
                    .ForMember(dto => dto.DeviceId, mpe => mpe.MapFrom(src => src.Device.Id))
                    .ForMember(dto => dto.EmployeeId, mpe => mpe.MapFrom(src => src.Employee.Id));

                cfg.CreateMap<CameraImage, CameraImageSyncDto>()
                    .ForMember(dto => dto.DeviceId, mpe => mpe.MapFrom(src => src.Device.Id))
                    .ForMember(dto => dto.ContentBinary, mpe => mpe.MapFrom(src => LocalContentStorage.Get(src)))
                    .ForMember(dto => dto.CompanyId, mpe => mpe.MapFrom(src => src.Company.Id));

                cfg.CreateMap<AudioRecord, AudioRecordSyncDto>()
                    .ForMember(dto => dto.ContentBinary, mpe => mpe.MapFrom(src => LocalContentStorage.Get(src)))
                    .ForMember(dto => dto.DeviceId, mpe => mpe.MapFrom(src => src.Device.Id));

                cfg.CreateMap<VideoRecord, VideoRecordSyncDto>()
                    .ForMember(dto => dto.ContentBinary, mpe => mpe.MapFrom(src => LocalContentStorage.Get(src)))
                    .ForMember(dto => dto.DeviceId, mpe => mpe.MapFrom(src => src.Device.Id));

                cfg.CreateMap<MediaRecordNote, MediaRecordNoteSyncData>().ForMember(dto => dto.MediaRecord,
                    mpe =>
                        mpe.MapFrom(src => Map(src.MediaRecord, typeof(ContentFile), typeof(ContentFileSyncDto))));

                cfg.CreateMap<ContentFile, ContentFileSyncDto>()
                    .ForMember(dto => dto.ContentBinary, mpe => mpe.MapFrom(src => LocalContentStorage.Get(src)))
                    .ForMember(dto => dto.CompanyId, mpe => mpe.MapFrom(src => src.Company.Id));

                cfg.CreateMap<SessionData, SessionDataSyncDto>()
                    .ForMember(dto => dto.QuestionId, mpe => mpe.MapFrom(src => src.Question.Id))
                    .ForMember(dto => dto.ResponseId, mpe => mpe.MapFrom(src => src.Response.Id));

                cfg.CreateMap<SurveyState, SurveyStateSyncDto>()
                    .ForMember(dto => dto.CompanyId, mpe => mpe.MapFrom(src => src.Company.Id));

                cfg.CreateMap<QueueTicket, QueueTicketSyncDto>()
                    .ForMember(dto => dto.CompanyId, mpe => mpe.MapFrom(src => src.Company.Id))
                    .ForMember(dto => dto.UnitId, mpe => mpe.MapFrom(src => src.Unit.Id))
                    .ForMember(dto => dto.FirstDeviceId, mpe => mpe.MapFrom(src => src.FirstDevice.Id))
                    .ForMember(dto => dto.DeviceId, mpe => mpe.MapFrom(src => src.Device.Id))
                    .ForMember(dto => dto.CallingDeviceId, mpe => mpe.MapFrom(src => src.CallingDevice.Id))
                    .ForMember(dto => dto.EmployeeId, mpe => mpe.MapFrom(src => src.Employee.Id))
                    .ForMember(dto => dto.ServiceId, mpe => mpe.MapFrom(src => src.Service.Id))
                    .ForMember(dto => dto.RedirectServiceId, mpe => mpe.MapFrom(src => src.RedirectService.Id));

                cfg.CreateMap<EQueueTicketStateActivity, EQueueTicketStateActivitySyncDto>()
                    .ForMember(dto => dto.QueueServiceId, mpe => mpe.MapFrom(src => src.QueueService.Id))
                    .ForMember(dto => dto.EmployeeId, mpe => mpe.MapFrom(src => src.Employee.Id))
                    .ForMember(dto => dto.DeviceId, mpe => mpe.MapFrom(src => src.Device.Id));

                cfg.CreateMap<UnitEvent, UnitEventDto>()
                    .ForMember(dto => dto.SoftwarePackageId, mpe => mpe.MapFrom(src => src.SoftwarePackage.Id))
                    .ForMember(dto => dto.UnitId, mpe => mpe.MapFrom(src => src.Unit.Id));

                cfg.CreateMap<QueueSetting, QueueSettingDto>()
                    .ForMember(dto => dto.UnitId, mpe => mpe.MapFrom(src => src.Unit != null ? src.Unit.Id : 0))
                    .ForMember(dto => dto.UnitSchedule, mpe => mpe.MapFrom(src => src.Unit.Schedule));

                cfg.CreateMap<DeviceStateActivitySyncDto, DeviceStateActivity>()
                    .ForMember(dto => dto.Device, mpe => mpe.MapFrom(src => DeviceRepository.Get(src.DeviceId)))
                    .ForMember(dto => dto.Employee, mpe => mpe.MapFrom(src => EmployeRepository.Get(src.EmployeeId)));

                cfg.CreateMap<ConversationSyncDto, Conversation>()
                    .ForMember(dto => dto.Device, mpe => mpe.MapFrom(src => DeviceRepository.Get(src.DeviceId)))
                    .ForMember(dto => dto.Employee, mpe => mpe.MapFrom(src => EmployeRepository.Get(src.EmployeeId)));

                cfg.CreateMap<SessionSyncDto, Session>()
                    .ForMember(dto => dto.Survey, mpe => mpe.MapFrom(src => SurveyRepository.Get(src.SurveyId)))
                    .ForMember(dto => dto.Device, mpe => mpe.MapFrom(src => DeviceRepository.Get(src.DeviceId)))
                    .ForMember(dto => dto.Employee, mpe => mpe.MapFrom(src => EmployeRepository.Get(src.EmployeeId)))
                    .ForMember(dto => dto.TimeoutQuestion,
                        mpe => mpe.MapFrom(src => QuestionRepository.Get(src.TimeoutQuestionId)));

                cfg.CreateMap<TestSessionSyncDto, TestSession>()
                    .ForMember(dto => dto.Survey, mpe => mpe.MapFrom(src => SurveyRepository.Get(src.SurveyId)))
                    .ForMember(dto => dto.Device, mpe => mpe.MapFrom(src => DeviceRepository.Get(src.DeviceId)))
                    .ForMember(dto => dto.Employee, mpe => mpe.MapFrom(src => EmployeRepository.Get(src.EmployeeId)));

                cfg.CreateMap<CameraImageSyncDto, CameraImage>()
                    .ForMember(dto => dto.Device, mpe => mpe.MapFrom(src => DeviceRepository.Get(src.DeviceId)))
                    .ForMember(dto => dto.Company, mpe => mpe.MapFrom(src => CompanyRepository.Get(src.CompanyId)));

                cfg.CreateMap<AudioRecordSyncDto, AudioRecord>()
                    .ForMember(dto => dto.Device, mpe => mpe.MapFrom(src => DeviceRepository.Get(src.DeviceId)))
                    .ForMember(dto => dto.Company, mpe => mpe.MapFrom(src => CompanyRepository.Get(src.CompanyId)));

                cfg.CreateMap<VideoRecordSyncDto, VideoRecord>()
                    .ForMember(dto => dto.Device, mpe => mpe.MapFrom(src => DeviceRepository.Get(src.DeviceId)))
                    .ForMember(dto => dto.Company, mpe => mpe.MapFrom(src => CompanyRepository.Get(src.CompanyId)));

                cfg.CreateMap<MediaRecordNoteSyncData, MediaRecordNote>().ForMember(dto => dto.MediaRecord,
                    mpe =>
                        mpe.MapFrom(src => Map(src.MediaRecord, typeof(ContentFileSyncDto), typeof(ContentFile))));

                cfg.CreateMap<ContentFileSyncDto, ContentFile>().ForMember(dto => dto.Company,
                    mpe => mpe.MapFrom(src => CompanyRepository.Get(src.CompanyId)));

                cfg.CreateMap<SessionDataSyncDto, SessionData>()
                    .ForMember(dto => dto.Question, mpe => mpe.MapFrom(src => QuestionRepository.Get(src.QuestionId)))
                    .ForMember(dto => dto.Response, mpe => mpe.MapFrom(src => ResponseRepository.Get(src.ResponseId)));

                cfg.CreateMap<SurveyStateSyncDto, SurveyState>().ForMember(dto => dto.Company,
                    mpe => mpe.MapFrom(src => CompanyRepository.Get(src.CompanyId)));

                cfg.CreateMap<EQueueTicketStateActivitySyncDto, EQueueTicketStateActivity>().ForMember(
                        dto => dto.QueueService,
                        mpe => mpe.MapFrom(src => QueueServiceRepository.Get(src.QueueServiceId))).ForMember(
                        dto => dto.Employee,
                        mpe => mpe.MapFrom(src => src.EmployeeId > 0 ? EmployeRepository.Get(src.EmployeeId) : null))
                    .ForMember(dto => dto.Device,
                        mpe => mpe.MapFrom(src => src.DeviceId > 0 ? DeviceRepository.Get(src.DeviceId) : null));

                cfg.CreateMap<QueueTicketSyncDto, QueueTicket>()
                    .ForMember(dto => dto.Company, mpe => mpe.MapFrom(src => CompanyRepository.Get(src.CompanyId)))
                    .ForMember(dto => dto.Unit, mpe => mpe.MapFrom(src => UnitRepository.Get(src.UnitId)))
                    .ForMember(dto => dto.FirstDevice,
                        mpe => mpe.MapFrom(src => DeviceRepository.Get(src.FirstDeviceId)))
                    .ForMember(dto => dto.Device, mpe => mpe.MapFrom(src => DeviceRepository.Get(src.DeviceId)))
                    .ForMember(dto => dto.CallingDevice,
                        mpe => mpe.MapFrom(src => DeviceRepository.Get(src.CallingDeviceId)))
                    .ForMember(dto => dto.Employee, mpe => mpe.MapFrom(src => EmployeRepository.Get(src.EmployeeId)))
                    .ForMember(dto => dto.Service, mpe => mpe.MapFrom(src => QueueServiceRepository.Get(src.ServiceId)))
                    .ForMember(dto => dto.RedirectService,
                        mpe => mpe.MapFrom(src => QueueServiceRepository.Get(src.RedirectServiceId)));

                cfg.CreateMap<QueueTicket, QueueTicketDto>()
                    .ForMember(dto => dto.CallDate,
                        mpe => mpe.MapFrom(src => src.CallDate.HasValue
                            ? src.CallDate.Value.ToUnixTime().ToString()
                            : string.Empty))
                    .ForMember(dto => dto.RegisterDate,
                        mpe => mpe.MapFrom(src => src.RegisterDate.ToUnixTime().ToString()))
                    .ForMember(dto => dto.ServiceName, mpe => mpe.MapFrom(src => src.Service.DisplayName))
                    .ForMember(dto => dto.DeviceDescription, mpe => mpe.MapFrom(src => src.Device.Description));

                cfg.CreateMap<UnitEventDto, UnitEvent>().ForMember(src => src.SoftwarePackage,
                        mpe => mpe.MapFrom(dto => SoftwarePackageRepository.Get(dto.SoftwarePackageId)))
                    .ForMember(src => src.Unit, mpe => mpe.MapFrom(dto => UnitRepository.Get(dto.UnitId)));

                cfg.CreateMap<Device, DeviceForListDto>()
                    .ForMember(mpe => mpe.CurrentEmployeeName, dto => dto.MapFrom(src => src.CurrentEmployee.FullnameWithFatherName))
                    .ForMember(mpe => mpe.SurveyName, dto => dto.MapFrom(src => src.Survey.Name))
                    .ForMember(mpe => mpe.GroupName, dto => dto.MapFrom(src => src.Group.Name))
                    .ForMember(mpe => mpe.SoftwareVersion, dto => dto.MapFrom(src => src.LocateSoftwareVersionName()))
                    .ForMember(mpe => mpe.LastActivity, dto => dto.MapFrom(src => src.LastActivityDate));

                cfg.CreateMap<ScriptBlock, ScriptBlockDto>();

                cfg.CreateMap<Script, ScriptDto>()
                    .ForMember(mpe => mpe.Language, dto => dto.MapFrom(src => src.ScriptLanguageStr));
            });
        }

        #endregion
    }
}