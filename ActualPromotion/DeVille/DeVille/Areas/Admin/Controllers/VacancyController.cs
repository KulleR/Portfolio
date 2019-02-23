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
    public class VacancyController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о продуктах
        /// </summary>
        private readonly IVacancyRepository vacancyRepository;

        /// <summary>
        /// Предоставляет доступ к отображателю объекта на оъект
        /// </summary>
        private readonly IMapper mapper;

        public VacancyController(IVacancyRepository _vacancyRepository, IMapper _mapper)
        {
            this.vacancyRepository = _vacancyRepository;
            this.mapper = _mapper;
        }

        public ActionResult Index()
        {
            return View(vacancyRepository.GetAll());
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddVacancy model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid && (ImgCover != null && ImgCover.ContentLength > 0))
            {
                Vacancy vacancy = mapper.Map(model, typeof(AddVacancy), typeof(Vacancy)) as Vacancy;

                var fileName = Path.GetFileName(ImgCover.FileName);
                var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                ImgCover.SaveAs(savingPath);
                vacancy.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));

                vacancyRepository.Insert(vacancy);
                vacancyRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Vacancy product = vacancyRepository.GetById(id);
            return View(mapper.Map(product, typeof(Vacancy), typeof(EditVacancy)) as EditVacancy);
        }

        [HttpPost]
        public ActionResult Edit(EditVacancy model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid)
            {
                Vacancy vacancy = vacancyRepository.GetById(model.Id);
                vacancy.Title = model.Title;
                vacancy.Description = model.Description;
                vacancy.Demands = model.Demands;
                vacancy.Duties = model.Duties;
                vacancy.Сondition = model.Сondition;

                if (ImgCover != null && ImgCover.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImgCover.FileName);
                    var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                    ImgCover.SaveAs(savingPath);
                    vacancy.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));
                }

                vacancyRepository.Update(vacancy);
                vacancyRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            Vacancy vacancy = vacancyRepository.GetById(id);
            vacancyRepository.Delete(vacancy);
            vacancyRepository.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
