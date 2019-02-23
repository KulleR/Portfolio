using System.Configuration;
using Common.Logging;
using Microsoft.Practices.Unity;
using Surveys.Service.Core.Handlers;
using Topshelf;

namespace Surveys.Service.Host
{
    class Program
    {
        #region Private Fields

        private static readonly ILog Logger = LogManager.GetLogger<Program>();

        #endregion

        #region Public Methods
        static void Main(string[] args)
        {
            Logger.Info("Starting CRMSensor Service");
            Logger.InfoFormat("Version: {0}", typeof(Program).Assembly.GetName().Version);


            var container = UnityConfig.BuildContainer();
            var host = HostFactory.New(
                x =>
                {
                    x.Service<MainService>(
                        s =>
                        {
                            s.BeforeStartingService(() =>
                            {
                                ServiceHandler.BeforeStartingEvent(System.Reflection.Assembly.GetExecutingAssembly().Location);
                            });
                            s.ConstructUsing(builder => container.Resolve<MainService>());
                            s.WhenStarted(service => service.OnStart());
                            s.WhenStopped(service => service.OnStop());

                            
                            //s.WhenShutdown(service => service.OnShutdown());
                        });
                    x.RunAsLocalSystem();
                    x.SetServiceName("CrmSensorService");
                    x.SetDisplayName("CRMSensor Service");
                    x.SetDescription("CRMSensor Service");
                });

            host.Run();
        }

        #endregion
    }
}
