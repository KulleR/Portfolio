using Deville.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Deville.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о категориях услуг
        /// </summary>
        private readonly IServiceCategoryRepository serviceCategoryRepository;

        public HomeController(IServiceCategoryRepository _serviceCategoryRepository)
        {
            this.serviceCategoryRepository = _serviceCategoryRepository;
        }

        public ActionResult Index()
        {
            return View(serviceCategoryRepository.GetAll());
        }

    }
}
