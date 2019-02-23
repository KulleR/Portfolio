using System;
using System.Linq;
using Common.Logging;
using Microsoft.Practices.Unity;
using Surveys.Core.Repositories;

namespace Surveys.Service.Core.Sync.Components.SynchronizationInterval
{
    public class CommonSynchronizationIntervalProvider : ISynchronizationIntervalProvider
    {
        private static readonly ILog logger = LogManager.GetLogger<CommonSynchronizationIntervalProvider>();

        [Dependency]
        public IUnitRepository UnitRepository { get; set; }

        [Dependency]
        public IQueueSettingRepository QueueSettingRepository { get; set; }

        public int GetInterval()
        {
            int intervalSec = 900;

            try
            {
                UnitRepository.Wrap(() =>
                {
                    var unit = UnitRepository.Query().FirstOrDefault();
                    var setting = unit != null ? QueueSettingRepository.GetByUnit(unit) : null;

                    if (setting == null)
                    {
                        logger.Warn("Not determine the setting: SyncIntervalMin. Because queue setting not found.");
                    }
                    else
                    {
                        if (setting.IntervalSyncMin > 0)
                        {
                            intervalSec = (int)TimeSpan.FromMinutes(setting.IntervalSyncMin).TotalSeconds;
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                logger.Error($"Failed. Default interval is {intervalSec} sec", ex);
            }

            return intervalSec;
        }
    }
}
