using Deville.Core.Mapper;
using Deville.EntityDataModel;
using Deville.Models.Showcases;
using Deville.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Deville.Controllers
{
    public class ServiceCategoryController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о категориях услуг
        /// </summary>
        private readonly IServiceCategoryRepository serviceCategoryRepository;

        /// <summary>
        /// Предоставляет доступ к отображателю объекта на оъект
        /// </summary>
        private readonly IMapper mapper;

        public ServiceCategoryController(IServiceCategoryRepository _serviceCategoryRepository, IMapper _mapper)
        {
            this.serviceCategoryRepository = _serviceCategoryRepository;
            this.mapper = _mapper;
        }

        public ActionResult Index()
        {
            return View(serviceCategoryRepository.GetAll());
        }

        public ActionResult GetServices(int? categoryId)
        {
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                return View(serviceCategoryRepository.GetById(categoryId.Value));
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult PriceLists()
        {
            return View(serviceCategoryRepository.GetAll());
        }

        [HttpPost]
        public JsonResult GetAll()
        {
            List<ServiceCategory> categories = serviceCategoryRepository.GetAll().ToList();
            List<DtoServiceCategory> response = mapper.Map(categories, typeof(List<ServiceCategory>), typeof(List<DtoServiceCategory>)) as List<DtoServiceCategory>;
            return Json(response);
        }
    }
}
