using System;
using Common.Logging;
using Microsoft.Practices.Unity;
using Surveys.Data.NHibernate;
using Surveys.Service.Core.Sync.FromHost;

namespace Surveys.Service.Core.Sync.Components.Synchronization
{
    public class FromHostSynchronizationComponent : ISynchronizationComponent
    {
        private static readonly ILog Logger = LogManager.GetLogger<FromHostSynchronizationComponent>();

        [Dependency]
        public WebApiSyncService FromHostSyncService { get; set; }

        [Dependency]
        public DbSessionWrapper DbSessionWrapper { get; set; }

        public void Synchronize()
        {
            try
            {
                DbSessionWrapper.Wrap(() => FromHostSyncService.SyncDataFromHost());
            }
            catch (Exception e)
            {
                Logger.Error("FromHostSync Failed", e);
            }
        }
    }
}
