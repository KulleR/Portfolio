using Surveys.Reporting.Indexes;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Practices.Unity;
using Surveys.Core;
using Surveys.Core.Helpers;
using Surveys.Core.Helpers.Filtering;
using Surveys.Core.Messaging;
using Surveys.Core.Repositories;
using Surveys.Data.Content;
using Surveys.Data.Content.Swift;
using Surveys.Data.NHibernate;
using Surveys.Data.NHibernate.Repositories;
using Surveys.Data.NHibernate.UnitOfWorkAware;
using Surveys.Service.Core.AutoMapper;
using Surveys.Service.Core.Handlers;
using Surveys.Service.Core.Services;
using Surveys.Service.Core.Signalr;
using Surveys.Service.Core.Sync.Components;
using Surveys.Service.Core.Sync.Components.Config;
using Surveys.Service.Core.Sync.Components.Predicates;
using Surveys.Service.Core.Sync.Components.SignalR;
using Surveys.Service.Core.Sync.Components.Synchronization;
using Surveys.Service.Core.Sync.Components.SynchronizationInterval;
using Surveys.Service.Core.Sync.Components.Timer;

namespace Surveys.Service.Host
{
    public class UnityConfig
    {
        private static IUnityContainer unityContainer;

        public static IUnityContainer BuildContainer()
        {
            if (unityContainer != null)
            {
                return unityContainer;
            }

            unityContainer = new UnityContainer();
            unityContainer.RegisterType<IUnitOfWorkFactory, NHibernateUnitOfWorkFactory>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<UnitLockManager, UnitLockManager>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IUserContext, DummyContext>();
            unityContainer.RegisterType<DbSessionWrapper, DbSessionWrapper>();
            unityContainer.RegisterType<ICommonMapper, CommonMapper>();
            unityContainer.RegisterType<IAdministratorNotificationService, AdministratorNotificationService>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<IUserRepository, UserRepository>();
            unityContainer.RegisterType<IRoleRepository, RoleRepository>();
            unityContainer.RegisterType<ICompanyRepository, CompanyRepository>();
            unityContainer.RegisterType<IGroupRepository, GroupRepository>();
            unityContainer.RegisterType<IDeviceRepository, DeviceRepository>();
            unityContainer.RegisterType<ISurveyRepository, SurveyRepository>();
            unityContainer.RegisterType<IScreenplayRepository, ScreenplayRepository>();
            unityContainer.RegisterType<IContentRepository, ContentRepository>();
            unityContainer.RegisterType<IQuestionRepository, QuestionRepository>();
            unityContainer.RegisterType<IStyleInfoRepository, StyleInfoRepository>();
            unityContainer.RegisterType<ISessionRepository, SessionRepository>();
            unityContainer.RegisterType<ISessionDataRepository, SessionDataRepository>();
            unityContainer.RegisterType<IResponseRepository, ResponseRepository>();
            unityContainer.RegisterType<INewsRepository, NewsRepository>();
            unityContainer.RegisterType<ICategoryRepository, CategoryRepository>();
            unityContainer.RegisterType<IDeviceEventRepository, DeviceEventRepository>();
            unityContainer.RegisterType<IOnlineActivityRepository, OnlineActivityRepository>();
            unityContainer.RegisterType<IEmployeeRepository, EmployeeRepository>();
            unityContainer.RegisterType<IContentFileRepository, ContentFileRepository>();
            unityContainer.RegisterType<ICameraImageRepository, CameraImageRepository>();
            unityContainer.RegisterType<ITicketRepository, TicketRepository>();
            unityContainer.RegisterType<IVideoRecordRepository, VideoRecordRepository>();
            unityContainer.RegisterType<IAudioRecordRepository, AudioRecordRepository>();
            unityContainer.RegisterType<ISurveyStateRepository, SurveyStateRepository>();
            unityContainer.RegisterType<IProductRepository, ProductRepository>();
            unityContainer.RegisterType<IProductSaleRepository, ProductSaleRepository>();
            unityContainer.RegisterType<IUnitRepository, UnitRepository>();
            unityContainer.RegisterType<IConversationRepository, ConversationRepository>();
            unityContainer.RegisterType<ITestPlanRepository, TestPlanRepository>();
            unityContainer.RegisterType<ITestSessionRepository, TestSessionRepository>();
            unityContainer.RegisterType<IQueueTicketRepository, QueueTicketRepository>();
            unityContainer.RegisterType<IQueueServiceRepository, QueueServiceRepository>();
            unityContainer.RegisterType<INotificationMessageRepository, NotificationMessageRepository>();
            unityContainer.RegisterType<ICompanyActivityRepository, CompanyActivityRepository>();
            unityContainer.RegisterType<IScreenplayPartRepository, ScreenplayPartRepository>();
            unityContainer.RegisterType<IIntegrationActionRepository, IntegrationActionRepository>();
            unityContainer.RegisterType<IIntegrationArgumentRepository, IntegrationArgumentRepository>();
            unityContainer.RegisterType<IIntegrationSchemeRepository, IntegrationSchemeRepository>();
            unityContainer.RegisterType<IIntegrationArgumentValueRepository, IntegrationArgumentValueRepository>();
            unityContainer.RegisterType<IQueueSettingRepository, QueueSettingRepository>();
            unityContainer.RegisterType<IEmployeeAuthActivityRepository, EmployeeAuthActivityRepository>();
            unityContainer.RegisterType<IDeviceStateActivityRepository, DeviceStateActivityRepository>();
            unityContainer.RegisterType<IEQueueTicketStateActivityRepository, EQueueTicketStateActivityRepository>();
            unityContainer.RegisterType<ITriggerRepository, TriggerRepository>();
            unityContainer.RegisterType<IQueueServiceGroupRepository, QueueServiceGroupRepository>();
            unityContainer.RegisterType<IQueueServiceRuleRepository, QueueServiceRuleRepository>();
            unityContainer.RegisterType<IUnitEventRepository, UnitEventRepository>();
            unityContainer.RegisterType<ISoftwarePackageRepository, SoftwarePackageRepository>();
            unityContainer.RegisterType<ITagRepository, TagRepository>();
            unityContainer.RegisterType<IReportInfoRepository, ReportInfoRepository>();

            // Controllers
            unityContainer.RegisterType<DeviceEventHandler>();
            unityContainer.RegisterType<ImageHandler>();
            unityContainer.RegisterType<EQueueHandler>();

            unityContainer.RegisterType<IMessageSender, DefaultSmsProvider>("SmsSender", new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IMessageSender, EmailSender>("EmailSender", new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<MessageSender>(new ContainerControlledLifetimeManager());

            // SignalR
            unityContainer.RegisterType<UnityHubActivator, UnityHubActivator>();
            unityContainer.RegisterType<SignalRDependencyResolver, SignalRDependencyResolver>();
            unityContainer.RegisterType<IHubActivator, UnityHubActivator>();

            unityContainer.RegisterType<EchoHub>();
            unityContainer.RegisterType<ClientHub>();
            unityContainer.RegisterType<DashboardHub>();

            unityContainer.RegisterType<ConnectionManager, ConnectionManager>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<DashboardSessionManager, DashboardSessionManager>(new ContainerControlledLifetimeManager());

            unityContainer.RegisterType<ClientBroadcastService, ClientBroadcastService>();
            unityContainer.RegisterType<DashboardBroadcastService, DashboardBroadcastService>();

            unityContainer.RegisterType<LoggingPipelineModule, LoggingPipelineModule>();
            unityContainer.RegisterType<NHibernateSessionPipelineModule, NHibernateSessionPipelineModule>();
            unityContainer.RegisterType<ISignalRHost, SignalRHost>();
            unityContainer.RegisterType<ISignalROptionsProvider, SignalROptionsProvider>();


            // Content storage
            unityContainer.RegisterType<IContentStorage, ContentStorageFacade>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<LocalContentStorage, LocalContentStorage>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<SwiftObjectStorage, SwiftObjectStorage>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<ContentExpirationManager, ContentExpirationManager>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<IContentObjectCache, DummyObjectCache>(new ContainerControlledLifetimeManager());
            unityContainer.RegisterType<MainService, MainService>(new ContainerControlledLifetimeManager());

            // Indexes
            unityContainer.RegisterType<EqueueServiceTimeAvgIndex>();
            unityContainer.RegisterType<EqueueWaitingTimeAvgIndex>();
            unityContainer.RegisterType<EqueueCompleteTicketCountIndex>();
            unityContainer.RegisterType<EqueueServiceTicketCountIndex>();
            unityContainer.RegisterType<PeriodFilterProvider>();
            unityContainer.RegisterType<EmployeeFilterProvider>();

            // Components
            unityContainer.RegisterType<ISynchronizationComponent, FromHostSynchronizationComponent>("FromHost");
            unityContainer.RegisterType<ISynchronizationComponent, ToHostSynchronizationComponent>("ToHost");
            unityContainer.RegisterType<ISynchronizationComponent, ToHostDeviceSynchronizationComponent>("ToHostDevice");
            unityContainer.RegisterType<ISynchronizationIntervalProvider, CommonSynchronizationIntervalProvider>("Common");
            unityContainer.RegisterType<ISynchronizationIntervalProvider, DeviceSynchronizationIntervalProvider>("Device");
            unityContainer.RegisterType<IConfigSettingsValidator, SynchronizationConfigSettingsValidator>();
            unityContainer.RegisterType<ISynchronizationTimer, SynchronizationTimer>();
            unityContainer.RegisterType<IPredicate, IsDatabaseEmptyPredicate>("IsDatabaseEmpty");


            return unityContainer;
        }
    }
}
