using Brio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using ActionMailer.Net;
using ActionMailer.Net.Mvc;

namespace BrioStroy
{
    public class EmailController : MailerBase
    {
        public EmailResult SendEmail(EmailModel model)
        {
            To.Add(model.To);

            From = model.From;
            
            Subject = model.Subject;

            return Email("SendEmail", model);

        }
    }
}
