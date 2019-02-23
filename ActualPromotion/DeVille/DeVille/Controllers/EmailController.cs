using ActionMailer.Net.Mvc;
using Deville.Models.Showcases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Deville.Controllers
{
    public class EmailController : MailerBase
    {
        public EmailResult SendEmail(Email model)
        {
            To.Add(model.To);
            From = model.From;
            Subject = model.Subject;
            return Email("SendEmail", model);
        }
    }
}
