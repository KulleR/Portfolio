using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BrioStroy
{
    public static class AppSettings
    {
        public static int CurrentCompany = Convert.ToInt32(ConfigurationSettings.AppSettings["CurrentCompany"]);
        public static string MailFrom = ConfigurationSettings.AppSettings["SendMailFrom"];
        public static string AdminEmail = ConfigurationSettings.AppSettings["AdminEmail"];
        public static string DocUploadDirectory = ConfigurationSettings.AppSettings["DocUploadDirectory"];
        public static string PriceUploadDirectory = ConfigurationSettings.AppSettings["PriceUploadDirectory"];
        public static string PhotoUploadDirectory = ConfigurationSettings.AppSettings["PhotoUploadDirectory"];
    }
}