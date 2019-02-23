using System;
using System.Linq;
using Common.Logging;
using Microsoft.Practices.Unity;
using Surveys.Core.Repositories;

namespace Surveys.Service.Core.Sync.Components.SynchronizationInterval
{
    public class DeviceSynchronizationIntervalProvider : ISynchronizationIntervalProvider
    {
        private static readonly ILog Logger = LogManager.GetLogger<DeviceSynchronizationIntervalProvider>();

        [Dependency]
        public IDeviceRepository DeviceRepository { get; set; }

        [Dependency("Common")]
        public ISynchronizationIntervalProvider CommonSynchronizationIntervalProvider { get; set; }

        /// <summary>
        /// Интервал синхронизации для устройств в секундах. 
        /// В качестве значения синхронизации выбирается OfflineInterval уменьшенный на 30 сек 
        /// устройства с минимальным интервал подключения.
        /// </summary>
        public int GetInterval()
        {
            int intervalSec = 15;

            try
            {
                DeviceRepository.Wrap(() =>
                {
                    var device = DeviceRepository.Query().ToList().OrderBy(d => d.LocateSettings().ConnectionInterval)
                        .FirstOrDefault();
                    if (device != null)
                    {
                        intervalSec = (int) device.OfflineInterval.TotalSeconds - 30;
                        int commonIntervalSec = CommonSynchronizationIntervalProvider.GetInterval();

                        intervalSec = intervalSec > commonIntervalSec ? commonIntervalSec : intervalSec;
                    }
                    else
                    {
                        Logger.Error("Using default interval is 15 sec as Device Sync Interval");
                    }
                });
            }
            catch (Exception ex)
            {
                Logger.Error("Failed. Default interval is 15 sec", ex);
            }

            return intervalSec;
        }
    }
}
