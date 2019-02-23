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
    public class PhotoGalleryController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о фотогаллереях
        /// </summary>
        private readonly IPhotoGalleryRepository photoGalleryRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о фотоальбомах
        /// </summary>
        private readonly IPhotoAlbumRepository photoAlbumRepository;

        /// <summary>
        /// Предоставляет доступ к отображателю объекта на оъект
        /// </summary>
        private readonly IMapper mapper;

        public PhotoGalleryController(IPhotoGalleryRepository _photoGalleryRepository, IPhotoAlbumRepository _photoAlbumRepository, IMapper _mapper)
        {
            this.photoGalleryRepository = _photoGalleryRepository;
            this.photoAlbumRepository = _photoAlbumRepository;
            this.mapper = _mapper;
        }

        public ActionResult Index()
        {
            return View(photoGalleryRepository.GetAll());
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddPhotoGallery model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid && (ImgCover != null && ImgCover.ContentLength > 0))
            {
                PhotoGallery photoGallery = mapper.Map(model, typeof(AddPhotoGallery), typeof(PhotoGallery)) as PhotoGallery;

                var fileName = Path.GetFileName(ImgCover.FileName);
                var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                ImgCover.SaveAs(savingPath);
                photoGallery.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));

                photoGalleryRepository.Insert(photoGallery);
                photoGalleryRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            PhotoGallery serviceCategory = photoGalleryRepository.GetById(id);
            return View(mapper.Map(serviceCategory, typeof(PhotoGallery), typeof(EditPhotoGallery)) as EditPhotoGallery);
        }

        [HttpPost]
        public ActionResult Edit(EditPhotoGallery model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid)
            {
                PhotoGallery photoGallery = photoGalleryRepository.GetById(model.Id);
                photoGallery.Name = model.Name;

                if (ImgCover != null && ImgCover.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImgCover.FileName);
                    var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                    ImgCover.SaveAs(savingPath);
                    photoGallery.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));
                }

                photoGalleryRepository.Update(photoGallery);
                photoGalleryRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            PhotoGallery photoGallery = photoGalleryRepository.GetById(id);
            photoGalleryRepository.Delete(photoGallery);
            photoGalleryRepository.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
