using System;
using Common.Logging;
using Microsoft.Practices.Unity;
using Surveys.Core.Configuration;
using Surveys.Service.Core.Sync.ToHost;

namespace Surveys.Service.Core.Sync.Components.Synchronization
{
    public class ToHostDeviceSynchronizationComponent : ISynchronizationComponent
    {
        private static readonly ILog logger = LogManager.GetLogger<ToHostDeviceSynchronizationComponent>();

        [Dependency]
        public DataSynchronizationAction ToHostSyncService { get; set; }


        /// <summary>
        /// Синхронизируем устройства отдельно от остальных данных
        /// по интервалу равному интервалу подключения устройства.
        /// Делается для того, чтобы на ЦА были актуальные данные об устройствах.
        /// </summary>
        public void Synchronize()
        {
            int companyId;
            int.TryParse(LocalConfiguration.GetSetting("SyncCompany"), out companyId);

            try
            {
                ToHostSyncService.DeviceSynchronize(companyId);
            }
            catch (Exception e)
            {
                logger.Error("ToHostSync Failed", e);
            }
        }
    }
}
