using Deville.Core;
using Deville.EntityDataModel;
using Deville.Models.Showcases;
using Deville.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Deville.Controllers
{
    public class ContactController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу обращений посетителей
        /// </summary>
        private readonly IOnlineAppointmentRepository onlineAppointmentRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу категорий услуг
        /// </summary>
        private readonly IServiceCategoryRepository serviceCategoryRepository;

        public ContactController(IOnlineAppointmentRepository _onlineAppointmentRepository, IServiceCategoryRepository _serviceCategoryRepository)
        {
            this.onlineAppointmentRepository = _onlineAppointmentRepository;
            this.serviceCategoryRepository = _serviceCategoryRepository;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SendOnlineAppointment(SendOnlineAppointment model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    onlineAppointmentRepository.Insert(new OnlineAppointment
                    {
                        AuthorName = model.AuthorName,
                        CreateDate = DateTime.Now,
                        Email = model.Email,
                        Phone = model.Phone,
                        ServiceId = model.ServiceId
                    });
                    onlineAppointmentRepository.SaveChanges();

                    new EmailController().SendEmail(new Email
                    {
                        To = model.Email,
                        Body = "Уважаемый, " + model.AuthorName + "! Наши специалисты уже занимаются Вашим вопросом и в скором времени свяжутся с Вами!",
                        From = String.Format("DeVille <{0}>", AppSettings.mailFrom),
                        Subject = "Ваш заявка успешно принята"
                    }).Deliver();

                    StringBuilder body = new StringBuilder();
                    body.AppendFormat("Имя: {0}; Телефон: {1}; Почта: {2}; Категория: {3};", model.AuthorName, model.Phone, model.Email, serviceCategoryRepository.GetById(model.ServiceId).Name);
                    new EmailController().SendEmail(new Email
                    {
                        To = AppSettings.adminEmail,
                        Body = body.ToString(),
                        From = String.Format("DeVille <{0}>", AppSettings.mailFrom),
                        Subject = "Новая online заявка c сайта"
                    }).Deliver();
                }
                catch (Exception e)
                {
                    return Json(new
                    {
                        success = false,
                        message = "Ошибка:" + e.Message
                    });
                }

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
