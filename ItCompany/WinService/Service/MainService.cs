using System;
using Common.Logging;
using Microsoft.Practices.Unity;
using Surveys.Core.Model;
using Surveys.Core.Repositories;
using Surveys.Data.NHibernate;
using Surveys.Service.Core.Handlers;
using Surveys.Service.Core.Services;
using Surveys.Service.Core.Sync.Components;
using Surveys.Service.Core.Sync.Components.SignalR;
using Surveys.Service.Core.Sync.FromHost;
using Surveys.Service.Core.Sync.ToHost;

namespace Surveys.Service.Host
{
    public class MainService
    {
        #region Private Fields

        private static readonly ILog logger = LogManager.GetLogger<Program>();

        #endregion

        #region Repositories

        [Dependency]
        public IDeviceRepository DeviceRepository { get; set; }

        [Dependency]
        public EQueueHandler EQueueHandler { get; set; }

        [Dependency]
        public ISynchronizationTimer SynchronizationTimer { get; set; }

        [Dependency]
        public ISignalRHost SignalRHost { get; set; }

        #endregion

        #region Services

        [Dependency]
        public IAdministratorNotificationService AdministratorNotificationService { get; set; }

        #endregion

        #region Public methods

        public void OnStart()
        {
            logger.Info("Starting MainService");

            SignalRHost.Start();
            SynchronizationTimer.Start();
            AdministratorNotificationService.Start();
        }

        public void OnStop()
        {
            logger.Info("Stopping MainService");

            AdministratorNotificationService.Stop();
            CloseAllContext();
            SignalRHost.Stop();
        }

        public void OnShutdown()
        {
            logger.Info("Shutdown MainService");
            OnStop();
        }

        #endregion

        #region Help methods

        private void CloseAllContext()
        {
            foreach (Device device in DeviceRepository.GetEQueueDevices())
            {
                EQueueHandler.DashboardSessionManager.ExecuteFor(device, context =>
                {
                    EQueueHandler.CloseSessionForContext(context);
                });
            }
        }

        #endregion
    }
}
