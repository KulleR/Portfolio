using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace Deville.Core.Context
{
    public interface IDevilleContext 
    {
        IAuthentication Auth { get; set; }
        User CurrentUser { get; }
    }
}
