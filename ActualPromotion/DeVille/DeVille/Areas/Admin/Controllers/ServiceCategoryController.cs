using Deville.Core;
using Deville.Core.Mapper;
using Deville.EntityDataModel;
using Deville.Models;
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
    public class ServiceCategoryController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к отображателю объекта на оъект
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о категориях услуг
        /// </summary>
        private readonly IServiceCategoryRepository serviceCategoryRepository;

        public ServiceCategoryController(IServiceCategoryRepository _serviceCategoryRepository, IMapper _mapper)
        {
            this.serviceCategoryRepository = _serviceCategoryRepository;
            this.mapper = _mapper;
        }

        public ActionResult Index()
        {
            return View(serviceCategoryRepository.GetAll());
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddServiceCategory model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid && (ImgCover != null && ImgCover.ContentLength > 0))
            {
                ServiceCategory serviceCategory = mapper.Map(model, typeof(AddServiceCategory), typeof(ServiceCategory)) as ServiceCategory;

                var fileName = Path.GetFileName(ImgCover.FileName);
                var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                ImgCover.SaveAs(savingPath);
                serviceCategory.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));

                serviceCategoryRepository.Insert(serviceCategory);
                serviceCategoryRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ServiceCategory serviceCategory = serviceCategoryRepository.GetById(id);
            return View(mapper.Map(serviceCategory, typeof(ServiceCategory), typeof(EditServiceCategory)) as EditServiceCategory);
        }

        [HttpPost]
        public ActionResult Edit(EditServiceCategory model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid)
            {
                ServiceCategory serviceCategory = serviceCategoryRepository.GetById(model.Id);
                serviceCategory.Name = model.Name;
                serviceCategory.Description = model.Description;

                if (ImgCover != null && ImgCover.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImgCover.FileName);
                    var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                    ImgCover.SaveAs(savingPath);
                    serviceCategory.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));
                }

                serviceCategoryRepository.Update(serviceCategory);
                serviceCategoryRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            ServiceCategory serviceCategory = serviceCategoryRepository.GetById(id);
            serviceCategoryRepository.Delete(serviceCategory);
            serviceCategoryRepository.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
