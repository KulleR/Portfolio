using Deville.Core;
using Deville.Core.Mapper;
using Deville.EntityDataModel;
using Deville.Models.Showcases;
using Deville.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Deville.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ServiceSubcategoryController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к отображателю объекта на оъект
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о подкатегориях услуг
        /// </summary>
        private readonly IServiceSubcategoryRepository serviceSubcategoryRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о категориях услуг
        /// </summary>
        private readonly IServiceCategoryRepository serviceCategoryRepository;

        public ServiceSubcategoryController(IServiceSubcategoryRepository _serviceSubcategoryRepository, IServiceCategoryRepository _serviceCategoryRepository, IMapper _mapper)
        {
            this.serviceSubcategoryRepository = _serviceSubcategoryRepository;
            this.serviceCategoryRepository = _serviceCategoryRepository;
            this.mapper = _mapper;
        }

        public ActionResult Index()
        {
            return View(serviceSubcategoryRepository.GetAll());
        }

        [HttpGet]
        public ActionResult Add()
        {
            List<ServiceCategory> categories = serviceCategoryRepository.GetAll().ToList();
            ViewBag.ServiceCategories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddServiceSubcategory model)
        {
            if (ModelState.IsValid)
            {
                ServiceSubcategory serviceSubcategory = mapper.Map(model, typeof(AddServiceSubcategory), typeof(ServiceSubcategory)) as ServiceSubcategory;

                serviceSubcategoryRepository.Insert(serviceSubcategory);
                serviceSubcategoryRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            List<ServiceCategory> categories = serviceCategoryRepository.GetAll().ToList();
            ViewBag.ServiceCategories = new SelectList(categories, "Id", "Name");
            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ServiceSubcategory serviceCategory = serviceSubcategoryRepository.GetById(id);
            List<ServiceCategory> categories = serviceCategoryRepository.GetAll().ToList();
            ViewBag.ServiceCategories = new SelectList(categories, "Id", "Name", serviceCategory.ServiceCategory);
            return View(mapper.Map(serviceCategory, typeof(ServiceSubcategory), typeof(EditServiceSubcategory)) as EditServiceSubcategory);
        }

        [HttpPost]
        public ActionResult Edit(EditServiceSubcategory model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid)
            {
                ServiceSubcategory serviceCategory = serviceSubcategoryRepository.GetById(model.Id);
                serviceCategory.Title = model.Title;
                serviceCategory.Description = model.Description;

                serviceSubcategoryRepository.Update(serviceCategory);
                serviceSubcategoryRepository.SaveChanges();

                return RedirectToAction("Index");
            }
            List<ServiceCategory> categories = serviceCategoryRepository.GetAll().ToList();
            ViewBag.ServiceCategories = new SelectList(categories, "Id", "Name");
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            ServiceSubcategory serviceCategory = serviceSubcategoryRepository.GetById(id);
            serviceSubcategoryRepository.Delete(serviceCategory);
            serviceSubcategoryRepository.SaveChanges();

            return RedirectToAction("Index");
        }

        public JsonResult GetByCategoryId(int id)
        {
            List<ServiceSubcategory> serviceSubcategories = serviceSubcategoryRepository.GetAll().Where(s => s.CategoryId == id).ToList();
            List<DtoServiceSubcategory> dtoServiceSubcategories = mapper.Map(serviceSubcategories, typeof(List<ServiceSubcategory>), typeof(List<DtoServiceSubcategory>)) as List<DtoServiceSubcategory>;
            return Json(dtoServiceSubcategories);
        }
    }
}

