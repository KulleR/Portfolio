using Deville.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Deville.Controllers
{
    public class ReviewController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных об отзывах
        /// </summary>
        private readonly IReviewRepository reviewRepository;

        public ReviewController(IReviewRepository _reviewRepository)
        {
            this.reviewRepository = _reviewRepository;
        }

        public ActionResult Index()
        {
            return View(reviewRepository.GetAll());
        }

    }
}
