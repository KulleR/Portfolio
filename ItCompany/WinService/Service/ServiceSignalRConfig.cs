using Common.Logging;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Diagnostics;
using Microsoft.Practices.Unity;
using Owin;
using Surveys.Service.Core.Signalr;
using Surveys.Service.Host;

[assembly: OwinStartup(typeof(ServiceSignalRConfig))]

namespace Surveys.Service.Host
{
    class ServiceSignalRConfig
    {
        private static readonly ILog logger = LogManager.GetLogger<ServiceSignalRConfig>();

        public void Configuration(IAppBuilder app)
        {
            var container = UnityConfig.BuildContainer();

            GlobalHost.DependencyResolver = container.Resolve<SignalRDependencyResolver>();
            GlobalHost.HubPipeline.AddModule(container.Resolve<LoggingPipelineModule>());
            GlobalHost.HubPipeline.AddModule(container.Resolve<NHibernateSessionPipelineModule>());

            logger.DebugFormat("Configuring SignalR");
            // Передавать информацию об ошибках сервера на клиент
            app.UseErrorPage(new ErrorPageOptions { ShowExceptionDetails = true, ShowQuery = true, ShowEnvironment = true });
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);
                var hubConfiguration = new HubConfiguration
                {
                    // Используем JSONP, поскольку среди клиентов есть IE 8, CORS не используем
                    EnableJSONP = true,
                    EnableDetailedErrors = true,
                    
                };
                // Run the SignalR pipeline. We're not using MapSignalR
                // since this branch already runs under the "/signalr"
                // path.
                map.RunSignalR(hubConfiguration);
            });

        }
    }
}
