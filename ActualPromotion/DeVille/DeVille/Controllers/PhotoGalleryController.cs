using Deville.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Deville.Controllers
{
    public class PhotoGalleryController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о фотогаллереях
        /// </summary>
        private readonly IPhotoGalleryRepository photoGalleryRepository;

        public PhotoGalleryController(IPhotoGalleryRepository _photoGalleryRepository)
        {
            this.photoGalleryRepository = _photoGalleryRepository;
        }

        public ActionResult Index()
        {
            return View(photoGalleryRepository.GetAll());
        }

    }
}
