using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Deville.Helpers.Extensions
{
    public static class HtmlExtensions
    {
        public static string IsSelected(this HtmlHelper html/*, string action = null*/, string[] controller = null)
        {
            string cssClass = "active";
            string currentAction = (string)html.ViewContext.RouteData.Values["action"];
            string currentController = (string)html.ViewContext.RouteData.Values["controller"];

            if (controller.Length > 0)
            {
                foreach (string c in controller)
                {
                    if (c == currentController)
                    {
                        return cssClass;
                    }
                }
            }

            return String.Empty;
        }
    }
}