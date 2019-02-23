using Common.Logging;
using Surveys.Core.Configuration;

namespace Surveys.Service.Core.Sync.Components.Config
{
    public class SynchronizationConfigSettingsValidator : IConfigSettingsValidator
    {
        private static readonly ILog Logger = LogManager.GetLogger<SynchronizationConfigSettingsValidator>();

        public bool Validate()
        {
            var userName = LocalConfiguration.GetSetting("SyncUser");

            if (userName == string.Empty)
            {
                Logger.Warn("Setting [SyncUser] in config is empty!");
                return false;
            }

            int companyId;
            var isSuccessParseCompany = int.TryParse(LocalConfiguration.GetSetting("SyncCompany"), out companyId);

            if (!isSuccessParseCompany)
            {
                Logger.WarnFormat("Setting [SyncCompany: {0}] in config is NOT CORRECT!", companyId);
                return false;
            }

            return true;
        }
    }
}
