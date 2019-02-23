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
    public class PhotoAlbumController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о категориях услуг
        /// </summary>
        private readonly IPhotoAlbumRepository photoAlbumRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о категориях услуг
        /// </summary>
        private readonly IPhotoGalleryRepository photoGalleryRepository;

        /// <summary>
        /// Предоставляет доступ к отображателю объекта на оъект
        /// </summary>
        private readonly IMapper mapper;

        public PhotoAlbumController(IPhotoAlbumRepository _photoAlbumRepository, IPhotoGalleryRepository _photoGalleryRepository, IMapper _mapper)
        {
            this.photoAlbumRepository = _photoAlbumRepository;
            this.photoGalleryRepository = _photoGalleryRepository;
            this.mapper = _mapper;
        }

        public ActionResult Index(int? galleryId)
        {
            if (galleryId.HasValue && galleryId > 0)
            {
                PhotoGallery photoGallery = photoGalleryRepository.GetById(galleryId.Value);
                if (photoGallery != null)
                {
                    ViewBag.Gallery = photoGallery;
                    return View(photoAlbumRepository.GetGalleryAlbums(galleryId.Value));
                }
            }

            return RedirectToAction("Index", "PhotoAlbum");
        }

        [HttpGet]
        public ActionResult Add(int? galleryId)
        {
            if (galleryId.HasValue && galleryId > 0)
            {
                PhotoGallery photoGallery = photoGalleryRepository.GetById(galleryId.Value);
                if (photoGallery != null)
                {
                    ViewBag.GalleryId = galleryId;
                    return View();
                }
            }

            return RedirectToAction("Index", "PhotoAlbum");
        }

        [HttpPost]
        public ActionResult Add(AddPhotoAlbum model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid && (ImgCover != null && ImgCover.ContentLength > 0))
            {
                PhotoAlbum photoAlbum = mapper.Map(model, typeof(AddPhotoAlbum), typeof(PhotoAlbum)) as PhotoAlbum;

                var fileName = Path.GetFileName(ImgCover.FileName);
                var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                ImgCover.SaveAs(savingPath);
                photoAlbum.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));

                photoAlbumRepository.Insert(photoAlbum);
                photoAlbumRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            PhotoAlbum photoAlbum = photoAlbumRepository.GetById(id);
            return View(mapper.Map(photoAlbum, typeof(PhotoAlbum), typeof(EditPhotoAlbum)) as EditPhotoAlbum);
        }

        [HttpPost]
        public ActionResult Edit(EditPhotoAlbum model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid)
            {
                PhotoAlbum photoAlbum = photoAlbumRepository.GetById(model.Id);
                photoAlbum.Name = model.Name;

                if (ImgCover != null && ImgCover.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImgCover.FileName);
                    var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                    ImgCover.SaveAs(savingPath);
                    photoAlbum.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));
                }

                photoAlbumRepository.Update(photoAlbum);
                photoAlbumRepository.SaveChanges();

                return RedirectToAction("Index", new { galleryId = photoAlbum.GalleryId });
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            PhotoAlbum photoAlbum = photoAlbumRepository.GetById(id);
            photoAlbumRepository.Delete(photoAlbum);
            photoAlbumRepository.SaveChanges();

            return RedirectToAction("Index", new { galleryId = photoAlbum.GalleryId });
        }
    }
}
