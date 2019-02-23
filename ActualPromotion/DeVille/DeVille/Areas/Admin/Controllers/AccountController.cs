using Deville.Core.Context;
using Deville.Models.Showcases;
using Deville.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Deville.Areas.Admin.Controllers
{
    public class AccountController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о пользователях
        /// </summary>
        private readonly IUserRepository userRepository;

        /// <summary>
        /// Экземпляр класса DevilleContext, предоставляет доступ к системным данным приложения.
        /// Может быть использован для доступа к текущему авторизованному пользователю
        /// </summary>
        private readonly IDevilleContext devilleContext;

        public AccountController(IUserRepository _userRepository, IDevilleContext _devilleContext)
        {
            this.userRepository = _userRepository;
            this.devilleContext = _devilleContext;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginUser model, string ReturnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = devilleContext.Auth.Login(model.Email, model.Password, model.RememberMe);

                if (user != null)
                {
                    if (Url.IsLocalUrl(ReturnUrl))
                    {
                        return Redirect(ReturnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("common", "Имя пользователя или пароль является не корректным.");
                }
            }

            return View(model);
        }

        /// <summary>  
        /// Метод отвечающий за бизнес логику на странице выхода из профиля.</summary>
        /// <returns>Экземпляр ViewResult, который выполняет визуализацию представления.</returns>
        public ActionResult Logout()
        {
            devilleContext.Auth.LogOut();
            return RedirectToAction("Index", "Home");
        }
    }
}
