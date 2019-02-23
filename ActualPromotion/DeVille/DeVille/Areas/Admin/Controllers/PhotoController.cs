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
    public class PhotoController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о фотографиях
        /// </summary>
        private readonly IPhotoRepository photoRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о фотоальбомах
        /// </summary>
        private readonly IPhotoAlbumRepository photoAlbumRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о фотогаллереях
        /// </summary>
        private readonly IPhotoGalleryRepository photoGalleryRepository;

        /// <summary>
        /// Предоставляет доступ к отображателю объекта на оъект
        /// </summary>
        private readonly IMapper mapper;

        public PhotoController(IPhotoAlbumRepository _photoAlbumRepository, IPhotoGalleryRepository _photoGalleryRepository, IPhotoRepository _photoRepository, IMapper _mapper)
        {
            this.photoAlbumRepository = _photoAlbumRepository;
            this.photoGalleryRepository = _photoGalleryRepository;
            this.photoRepository = _photoRepository;
            this.mapper = _mapper;
        }

        public ActionResult Index(int? albumId)
        {
            if (albumId.HasValue && albumId > 0)
            {
                PhotoAlbum album = photoAlbumRepository.GetById(albumId.Value);
                if (album != null)
                {
                    PhotoGallery gallery = photoGalleryRepository.GetById(album.GalleryId);
                    if (gallery != null)
                    {
                        ViewBag.Album = album;
                        ViewBag.Gallery = gallery;

                        return View(photoRepository.GetAlbumPhotos(albumId.Value));
                    }
                }

            }

            return RedirectToAction("Index", "PhotoGallery");
        }

        [HttpGet]
        public ActionResult Add(int? albumId)
        {
            if (albumId.HasValue && albumId > 0)
            {
                PhotoAlbum album = photoAlbumRepository.GetById(albumId.Value);
                if (album != null)
                {
                    ViewBag.AlbumId = albumId;
                    return View();
                }
            }

            return RedirectToAction("Index", "PhotoGallery");
        }

        [HttpPost]
        public ActionResult Add(AddPhoto model, HttpPostedFileBase ImgUrl)
        {
            if (ModelState.IsValid && (ImgUrl != null && ImgUrl.ContentLength > 0))
            {
                Photo photo = mapper.Map(model, typeof(AddPhoto), typeof(Photo)) as Photo;

                var fileName = Path.GetFileName(ImgUrl.FileName);
                var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                ImgUrl.SaveAs(savingPath);
                photo.ImgUrl = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));

                photoRepository.Insert(photo);
                photoRepository.SaveChanges();

                return RedirectToAction("Index", new { albumId = photo.AlbumId });
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            Photo photo = photoRepository.GetById(id);
            photoRepository.Delete(photo);
            photoRepository.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
