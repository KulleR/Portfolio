using Deville.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Deville.Controllers
{
    public class VacancyController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о продуктах
        /// </summary>
        private readonly IVacancyRepository vacancyRepository;

        public VacancyController(IVacancyRepository _vacancyRepository)
        {
            this.vacancyRepository = _vacancyRepository;
        }

        public ActionResult Index()
        {
            return View(vacancyRepository.GetAll());
        }

    }
}
