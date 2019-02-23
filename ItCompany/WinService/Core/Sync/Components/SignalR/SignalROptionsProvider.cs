using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using Common.Logging;
using Microsoft.Owin.Hosting;
using Surveys.Core.Configuration;

namespace Surveys.Service.Core.Sync.Components.SignalR
{
    public class SignalROptionsProvider : ISignalROptionsProvider
    {
        private static readonly ILog Logger = LogManager.GetLogger<SignalROptionsProvider>();

        public List<Uri> GetUrlList()
        {
            var ipAddrListStr = LocalConfiguration.GetSetting("SignalRAddressList").Split(';');
            var ipAddrList = new List<Uri>();

            for (var i = 0; i < ipAddrListStr.Length; i++)
            {
                if (ipAddrListStr[i].StartsWith("http"))
                {
                    ipAddrList.Add(new Uri($"{ipAddrListStr[i]}"));
                }
                else
                {
                    ipAddrList.Add(new Uri($"http://{ipAddrListStr[i]}"));
                }
            }

            return ipAddrList;
        }

        public string GetSignalrConnectionUrl()
        {
            List<Uri> listenUrls = GetUrlList();
            if (listenUrls.Any())
            {
                var serviceUri = listenUrls.FirstOrDefault(u => u.Host == System.Web.HttpContext.Current.Request.Url.Host);
                if (serviceUri != null)
                {
                    //logger.DebugFormat("Accessible signalr connection url: {0}", serviceUri.AbsoluteUri);
                    return Path.Combine(serviceUri.AbsoluteUri, "signalr");
                }

                //logger.DebugFormat("Not found accessible signalr connection url, choose default url: {0}", ListenUrls.First().AbsoluteUri);
                return Path.Combine(listenUrls.First().AbsoluteUri, "signalr");
            }

            Logger.ErrorFormat("Signalr connection address list is empty!");
            return string.Empty;
        }

        public StartOptions GetOptions()
        {
            var options = new StartOptions();

            foreach (var url in GetUrlList())
            {
                options.Urls.Add(url.AbsoluteUri);
            }

            return options;
        }
    }
}
