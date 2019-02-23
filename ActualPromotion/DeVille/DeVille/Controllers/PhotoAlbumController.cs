using Deville.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Deville.Controllers
{
    public class PhotoAlbumController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о фотоальбомах
        /// </summary>
        private readonly IPhotoAlbumRepository photoAlbumRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о фотогалереях
        /// </summary>
        private readonly IPhotoGalleryRepository photoGalleryRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о фотографиях
        /// </summary>
        private readonly IPhotoRepository photoRepository;

        public PhotoAlbumController(IPhotoAlbumRepository _photoAlbumRepository, IPhotoRepository _photoRepository, IPhotoGalleryRepository _photoGalleryRepository)
        {
            this.photoAlbumRepository = _photoAlbumRepository;
            this.photoRepository = _photoRepository;
            this.photoGalleryRepository = _photoGalleryRepository;
        }

        public ActionResult Index(int? galleryId)
        {
            if (galleryId.HasValue && galleryId > 0)
            {
                ViewBag.Gallery = photoGalleryRepository.GetById(galleryId.Value);
                return View(photoAlbumRepository.GetGalleryAlbums(galleryId.Value));
            }

            return RedirectToAction("Index", "PhotoGallery");
        }

        public ActionResult GetPhotos(int? albumId)
        {
            if (albumId.HasValue && albumId > 0)
            {
                ViewBag.Album = photoAlbumRepository.GetById(albumId.Value);
                return View(photoRepository.GetAlbumPhotos(albumId.Value));
            }

            return RedirectToAction("Index", "PhotoGallery");
        }

    }
}
