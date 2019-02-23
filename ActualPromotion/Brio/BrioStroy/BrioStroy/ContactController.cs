using Brio;
using Brio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace BrioLab
{
    public class ContactController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о пользователях
        /// </summary>
        private readonly ICompanyRepository companyRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу обращений посетителей
        /// </summary>
        private readonly IFeedbackRepository feedbackRepository;

        public ContactController(ICompanyRepository _contactRepository, IFeedbackRepository _feedbackRepository)
        {
            this.companyRepository = _contactRepository;
            this.feedbackRepository = _feedbackRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.Contacts = companyRepository.GetByCompany(AppSettings.CurrentCompany);
            return View();
        }

        [HttpPost]
        public JsonResult SendFeedback(SendFeedback model)
        {
            if (ModelState.IsValid)
            {
                feedbackRepository.Insert(new Feedback { 
                    Email = model.Email, 
                    Name = model.Name, 
                    Phone = model.Phone, 
                    Message = model.Message 
                });

                new EmailController().SendEmail(new EmailModel
                {
                    To = model.Email,
                    Body = "Уважаемый, " + model.Name + "! Наши специалисты уже занимаются Вашим вопросом и в скором времени свяжутся с Вами!",
                    From = AppSettings.MailFrom,
                    Subject = "Ваш заявка успешно принята"
                }).Deliver();

                StringBuilder body = new StringBuilder();
                body.AppendFormat("Имя: {0}; Телефон: {1}; Почта: {2}; Сообщение: {3};", model.Name, model.Phone, model.Email, model.Message);
                new EmailController().SendEmail(new EmailModel
                {
                    To = AppSettings.AdminEmail,
                    Body = body.ToString(),
                    From = AppSettings.MailFrom,
                    Subject = "Заявка с сайта " + companyRepository.GetById(AppSettings.CurrentCompany).CompanyName
                }).Deliver();

                return Json(new
                {
                    success = true,
                    message = "Отправлено"
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    message = "Ошибка"
                });
            }

           
        }
    }
}
