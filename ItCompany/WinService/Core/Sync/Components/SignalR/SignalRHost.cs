using System;
using Common.Logging;
using Microsoft.Owin.Hosting;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;

namespace Surveys.Service.Core.Sync.Components.SignalR
{
    public class SignalRHost : ISignalRHost
    {
        private static readonly ILog Logger = LogManager.GetLogger<SignalRHost>();

        private IDisposable _signalRHost;

        [Dependency]
        public ISignalROptionsProvider SignalROptionProvider { get; set; }

        public bool Start()
        {
            try
            {
                StartOptions options = SignalROptionProvider.GetOptions();
                using (_signalRHost = WebApp.Start(options))
                {
                    Logger.InfoFormat("SignalR Host started on {0}", options.Urls.JoinStrings("; "));
                }

                return _signalRHost != null;
            }
            catch (Exception ex)
            {
                Logger.Error("Error during start Service Host: " + ex.Message, ex);
            }

            return false;
        }

        public void Stop()
        {
            if (_signalRHost != null)
            {
                Logger.DebugFormat("Disposing SignalR application");
                _signalRHost.Dispose();
                _signalRHost = null;
            }
        }
    }
}
