using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Core
{
    public static class AppSettings
    {
        public static string photoUploadDirectory
        {
            get { return ConfigurationManager.AppSettings["photoUploadDirectory"]; }
        }

        public static string adminEmail
        {
            get { return ConfigurationManager.AppSettings["AdminEmail"]; }
        }

        public static string mailFrom
        {
            get { return ConfigurationManager.AppSettings["SendMailFrom"]; }
        }
    }
}
