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
    public class ServiceController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к отображателю объекта на оъект
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о категориях услуг
        /// </summary>
        private readonly IServiceCategoryRepository serviceCategoryRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных об услугах
        /// </summary>
        private readonly IServiceRepository serviceRepository;

        public ServiceController(IServiceCategoryRepository _serviceCategoryRepository, IServiceRepository _serviceRepository, IMapper _mapper)
        {
            this.serviceCategoryRepository = _serviceCategoryRepository;
            this.serviceRepository = _serviceRepository;
            this.mapper = _mapper;
        }

        public ActionResult Index()
        {
            return View(serviceRepository.GetAll());
        }

        [HttpGet]
        public ActionResult Add()
        {
            List<ServiceCategory> categories = serviceCategoryRepository.GetAll().ToList();
            ViewBag.ServiceCategories = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddService model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid)
            {
                Service service = mapper.Map(model, typeof(AddService), typeof(Service)) as Service;

                if (ImgCover != null && ImgCover.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImgCover.FileName);
                    var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                    ImgCover.SaveAs(savingPath);
                    service.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));
                }

                serviceRepository.Insert(service);
                serviceRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            List<ServiceCategory> categories = serviceCategoryRepository.GetAll().ToList();
            ViewBag.ServiceCategories = new SelectList(categories, "Id", "Name");

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Service service = serviceRepository.GetById(id);

            List<ServiceCategory> categories = serviceCategoryRepository.GetAll().ToList();
            ViewBag.ServiceCategories = new SelectList(categories, "Id", "Name", service.ServiceCategory);
            return View(mapper.Map(service, typeof(Service), typeof(EditService)) as EditService);
        }

        [HttpPost]
        public ActionResult Edit(EditService model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid)
            {
                Service Service = serviceRepository.GetById(model.Id);
                Service.Name = model.Name;
                Service.Description = model.Description;
                Service.SubcategoryId = model.SubcategoryId;

                if (ImgCover != null && ImgCover.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImgCover.FileName);
                    var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                    ImgCover.SaveAs(savingPath);
                    Service.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));
                }

                serviceRepository.Update(Service);
                serviceRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            List<ServiceCategory> categories = serviceCategoryRepository.GetAll().ToList();
            ViewBag.ServiceCategories = new SelectList(categories, "Id", "Name");
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            Service Service = serviceRepository.GetById(id);
            serviceRepository.Delete(Service);
            serviceCategoryRepository.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
