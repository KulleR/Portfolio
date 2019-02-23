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
    public class ReviewController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных об отзывах
        /// </summary>
        private readonly IReviewRepository reviewRepository;

        /// <summary>
        /// Предоставляет доступ к отображателю объекта на оъект
        /// </summary>
        private readonly IMapper mapper;

        public ReviewController(IReviewRepository _reviewRepository, IMapper _mapper)
        {
            this.reviewRepository = _reviewRepository;
            this.mapper = _mapper;
        }

        public ActionResult Index()
        {
            return View(reviewRepository.GetAll());
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddReview model, HttpPostedFileBase AuthorPhoto)
        {
            if (ModelState.IsValid && (AuthorPhoto != null && AuthorPhoto.ContentLength > 0))
            {
                Review review = mapper.Map(model, typeof(AddReview), typeof(Review)) as Review;
                review.CreateDate = DateTime.Now;

                var fileName = Path.GetFileName(AuthorPhoto.FileName);
                var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                AuthorPhoto.SaveAs(savingPath);
                review.AuthorPhoto = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));

                reviewRepository.Insert(review);
                reviewRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Review Review = reviewRepository.GetById(id);
            return View(mapper.Map(Review, typeof(Review), typeof(EditReview)) as EditReview);
        }

        [HttpPost]
        public ActionResult Edit(EditReview model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid)
            {
                Review review = reviewRepository.GetById(model.Id);
                review.Title = model.Title;
                review.AuthorFullName = model.AuthorFullName;
                review.Message = model.Message;
                review.Title = model.Title;


                if (ImgCover != null && ImgCover.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImgCover.FileName);
                    var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                    ImgCover.SaveAs(savingPath);
                    review.AuthorPhoto = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));
                }

                reviewRepository.Update(review);
                reviewRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            Review review = reviewRepository.GetById(id);
            reviewRepository.Delete(review);
            reviewRepository.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
