using Deville.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Deville.Controllers
{
    public class NewsController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных об услугах
        /// </summary>
        private readonly INewsRepository newsRepository;

        public NewsController(INewsRepository _newsRepository)
        {
            this.newsRepository = _newsRepository;
        }

        public ActionResult Index()
        {
            return View(newsRepository.GetAll());
        }

        public ActionResult View(int? id)
        {
            if (id.HasValue && id.Value > 0)
            {
                return View(newsRepository.GetById(id.Value));
            }

            return RedirectToAction("Index");
        }

    }
}
