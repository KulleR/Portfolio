using System;
using System.Linq;
using System.Threading;
using Common.Logging;
using Microsoft.Practices.Unity;
using Surveys.Core.Model;
using Surveys.Core.Repositories;
using Surveys.Service.Core.Sync.Components.Predicates;
using Surveys.Service.Core.Sync.Components.SynchronizationInterval;

namespace Surveys.Service.Core.Sync.Components.Timer
{
    public class SynchronizationTimer : ISynchronizationTimer
    {
        private static readonly ILog Logger = LogManager.GetLogger<SynchronizationTimer>();

        [Dependency]
        public IConfigSettingsValidator ConfigSettingsValidator { get; set; }

        [Dependency("Common")]
        public ISynchronizationIntervalProvider CommonSynchronizationIntervalProvider { get; set; }

        [Dependency("Device")]
        public ISynchronizationIntervalProvider DeviceSynchronizationIntervalProvider { get; set; }

        [Dependency("ToHost")]
        public ISynchronizationComponent ToHostSynchronizationComponent { get; set; }

        [Dependency("ToHostDevice")]
        public ISynchronizationComponent ToHostDeviceSynchronizationComponent { get; set; }

        [Dependency("FromHost")]
        public ISynchronizationComponent FromHostSynchronizationComponent { get; set; }

        [Dependency]
        public IEQueueTicketStateActivityRepository EQueueTicketStateActivityRepository { get; set; }

        [Dependency]
        public IQueueTicketRepository QueueTicketRepository { get; set; }

        [Dependency]
        public IDeviceRepository DeviceRepository { get; set; }

        [Dependency]
        public IDeviceStateActivityRepository DeviceStateActivityRepository { get; set; }

        [Dependency("IsDatabaseEmpty")]
        public IPredicate IsDatabaseEmptyPredicate { get; set; }

        public void Start()
        {
            if (ConfigSettingsValidator.Validate())
            {
                if (IsDatabaseEmptyPredicate.Test())
                {
                    FromHostSynchronizationComponent.Synchronize();
                }

                DeviceRepository.Wrap(() =>
                {
                    CloseAllPreviousActivity();
                    ChangeOldStates();

                    StartCommonSynchronizationTimer();
                    StartDeviceSynchronizationTimer();
                });
            }
        }

        private void StartCommonSynchronizationTimer()
        {
            var syncThread = new Thread(() =>
            {
                while (true)
                {
                    ToHostSynchronizationComponent.Synchronize();
                    FromHostSynchronizationComponent.Synchronize();

                    var syncInterval = CommonSynchronizationIntervalProvider.GetInterval();
                    Logger.InfoFormat("Content synchronization interval: {0} sec. Next sync time: {1}", syncInterval, DateTime.Now.AddSeconds(syncInterval));

                    Thread.Sleep(TimeSpan.FromSeconds(syncInterval));
                }
            });

            syncThread.IsBackground = true;
            syncThread.Start();
        }

        private void StartDeviceSynchronizationTimer()
        {
            var deviceSyncThread = new Thread(() =>
            {
                while (true)
                {
                    ToHostDeviceSynchronizationComponent.Synchronize();

                    var syncInterval = DeviceSynchronizationIntervalProvider.GetInterval();
                    Logger.InfoFormat("Device info synchronization interval: {0} sec. Next sync time: {1}", syncInterval, DateTime.Now.AddSeconds(syncInterval));

                    Thread.Sleep(TimeSpan.FromSeconds(syncInterval));
                }
            });

            deviceSyncThread.IsBackground = true;
            deviceSyncThread.Start();
        }

        /// <summary>
        /// Изменение старого статуса на новый в таблицах талонов и активностей
        /// </summary>
        /// <remarks>Удалить в версии 1.5</remarks>
        private void ChangeOldStates()
        {
            Logger.Debug("UpdateTicketState [AutoCalling => Calling]");

            var tickets = QueueTicketRepository.Query().Where(x =>
                x.State == Surveys.Core.Model.Queue.QueueTicketState.AutoCalling);

            if (tickets.Count() != 0)
            {
                foreach (var item in tickets)
                {
                    item.State = Surveys.Core.Model.Queue.QueueTicketState.Calling;
                }
            }

            var activities = EQueueTicketStateActivityRepository.Query().Where(x =>
                x.QueueTicketState == Surveys.Core.Model.Queue.QueueTicketState.AutoCalling);

            if (activities.Count() != 0)
            {
                foreach (var item in activities)
                {
                    item.QueueTicketState = Surveys.Core.Model.Queue.QueueTicketState.Calling;
                }
            }

            if (!tickets.Any() && !activities.Any())
            {
                Logger.Debug("AutoCalling not found");
            }
        }

        private void CloseAllPreviousActivity()
        {
            foreach (Device device in DeviceRepository.GetEQueueDevices())
            {
                if (device.State == DeviceState.Idle || device.State == DeviceState.Opened || 
                    device.State == DeviceState.Pause || device.CurrentEmployee != null)
                {
                    Logger.DebugFormat("Resetting state for {0}", device);
                    device.CurrentEmployee = null;

                    DeviceStateActivityRepository.StartNewActivity(device, DeviceState.Closed);
                    device.State = DeviceState.Closed;

                    DeviceRepository.Save(device);
                }
            }
        }
    }
}
