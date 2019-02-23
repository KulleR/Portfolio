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
    public class NewsController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к отображателю объекта на оъект
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных об услугах
        /// </summary>
        private readonly INewsRepository newsRepository;

        public NewsController(INewsRepository _newsRepository, IMapper _mapper)
        {
            this.newsRepository = _newsRepository;
            this.mapper = _mapper;
        }

        public ActionResult Index()
        {
            return View(newsRepository.GetAll());
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddNews model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid && (ImgCover != null && ImgCover.ContentLength > 0))
            {
                News news = mapper.Map(model, typeof(AddNews), typeof(News)) as News;
                news.CreateDate = DateTime.Now;

                var fileName = Path.GetFileName(ImgCover.FileName);
                var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                ImgCover.SaveAs(savingPath);
                news.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));

                newsRepository.Insert(news);
                newsRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            News news = newsRepository.GetById(id);
            return View(mapper.Map(news, typeof(News), typeof(EditNews)) as EditNews);
        }

        [HttpPost]
        public ActionResult Edit(EditNews model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid)
            {
                News news = newsRepository.GetById(model.Id);
                news.Title = model.Title;
                news.Text = model.Text;

                if (ImgCover != null && ImgCover.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImgCover.FileName);
                    var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                    ImgCover.SaveAs(savingPath);
                    news.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));
                }

                newsRepository.Update(news);
                newsRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            News News = newsRepository.GetById(id);
            newsRepository.Delete(News);
            newsRepository.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
