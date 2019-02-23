using System;
using Common.Logging;
using Microsoft.Practices.Unity;
using Surveys.Core.Configuration;
using Surveys.Service.Core.Sync.Components.SynchronizationInterval;
using Surveys.Service.Core.Sync.ToHost;

namespace Surveys.Service.Core.Sync.Components.Synchronization
{
    public class ToHostSynchronizationComponent : ISynchronizationComponent
    {
        private static readonly ILog Logger = LogManager.GetLogger<ToHostSynchronizationComponent>();

        [Dependency]
        public DataSynchronizationAction ToHostSyncService { get; set; }

        [Dependency("Common")]
        public ISynchronizationIntervalProvider SynchronizationIntervalProvider { get; set; }

        public void Synchronize()
        {
            try
            {
                int companyId;
                int.TryParse(LocalConfiguration.GetSetting("SyncCompany"), out companyId);

                int syncIntervalSec = SynchronizationIntervalProvider.GetInterval();

                ToHostSyncService.DeviceActivitiesSynchronize(companyId, syncIntervalSec);
                ToHostSyncService.QueueTicketsSynchronize(companyId, syncIntervalSec);
                ToHostSyncService.ConversationsSynchronize(companyId, syncIntervalSec);
                ToHostSyncService.SessionsSynchronize(companyId, syncIntervalSec);
                ToHostSyncService.UnitEventsSynchronize(companyId, syncIntervalSec);
            }
            catch (Exception e)
            {
                Logger.Error("ToHostSync Failed", e);
            }
        }
    }
}
