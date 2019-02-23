using System;
using System.Collections.Generic;
using Microsoft.Owin.Hosting;

namespace Surveys.Service.Core.Sync.Components.SignalR
{
    public interface ISignalROptionsProvider
    {
        string GetSignalrConnectionUrl();

        List<Uri> GetUrlList();

        StartOptions GetOptions();
    }
}
