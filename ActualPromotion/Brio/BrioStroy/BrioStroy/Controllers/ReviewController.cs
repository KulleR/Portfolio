
using Brio;
using Brio.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BrioStroy
{
    public class ReviewController : Controller
    {
        private readonly IReviewRepository reviewRepository;
        private string photoUploadDirecory = "//Files//Documents//";

        public ReviewController(IReviewRepository reviewRepository)
        {
            this.reviewRepository = reviewRepository;
        }

        public ActionResult GetAll()
        {
            return View(reviewRepository.GetCompanyReviews(AppSettings.CurrentCompany));
        }


        public JsonResult GetReviewContent(int reviewId)
        {
            Review review = reviewRepository.GetById(reviewId);
            ReviewContent response = new ReviewContent
            {
                AuthorName = review.Title,
                AuthorPosition = review.AutorPosition,
                Message = review.ReviewMessage,
                AuthorCompany = review.AuthorCompany,
                AuthorAvatar = review.AuthorPhoto
            };
            return Json(response);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            return View();
        }

        [Authorize(Roles="Admin")]
        [HttpPost]
        public ActionResult Add(ReviewContent postReview, HttpPostedFileBase AuthorAvatar)
        {
            if (ModelState.IsValid && (AuthorAvatar != null && AuthorAvatar.ContentLength > 0))
            {
                Review review = new Review();

                review.AuthorCompany = postReview.AuthorCompany;
                review.AutorPosition = postReview.AuthorPosition;
                review.CompanyId = AppSettings.CurrentCompany;
                review.Date = DateTime.Now;
                review.ReviewMessage = postReview.Message;
                review.Title = postReview.AuthorName;

                /*Сохранение фото*/
                var fileName = Path.GetFileName(AuthorAvatar.FileName);
                var savingPath = Path.Combine(HttpContext.Server.MapPath(photoUploadDirecory), fileName);
                AuthorAvatar.SaveAs(savingPath);
                review.AuthorPhoto = VirtualPathUtility.ToAbsolute(Path.Combine(photoUploadDirecory, fileName));

                reviewRepository.Insert(review);
                reviewRepository.SaveChanges();

                return RedirectToAction("GetAll");
                //throw new HttpException(403, "Forbidden");
            }
            else
                return View(postReview);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Review editReview = reviewRepository.GetById(id);
            return View(new EditReviewContent()
                    {
                        ID = editReview.ID,
                        AuthorCompany = editReview.AuthorCompany,
                        AuthorName = editReview.Title,
                        AuthorPosition = editReview.AutorPosition,
                        Message = editReview.ReviewMessage
                    }
                );
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public ActionResult Edit(EditReviewContent editReview, HttpPostedFileBase AuthorAvatar)
        {
            if (ModelState.IsValid)
            {
                Review review = reviewRepository.GetById(editReview.ID);

                review.AuthorCompany = editReview.AuthorCompany;
                review.AutorPosition = editReview.AuthorPosition;
                review.CompanyId = AppSettings.CurrentCompany;
                review.Date = DateTime.Now;
                review.ReviewMessage = editReview.Message;
                review.Title = editReview.AuthorName;

                if (AuthorAvatar != null && AuthorAvatar.ContentLength > 0)
                {
                    /*Сохранение фото*/
                    var fileName = Path.GetFileName(AuthorAvatar.FileName);
                    var savingPath = Path.Combine(HttpContext.Server.MapPath(photoUploadDirecory), fileName);
                    AuthorAvatar.SaveAs(savingPath);
                    review.AuthorPhoto = VirtualPathUtility.ToAbsolute(Path.Combine(photoUploadDirecory, fileName));
                }

                reviewRepository.Update(review);
                reviewRepository.SaveChanges();

                return RedirectToAction("GetAll");
                //throw new HttpException(403, "Forbidden");
            }
            else
                return View(editReview);
        }

        public ActionResult Delete(int id)
        {
            Review review = reviewRepository.GetById(id);
            reviewRepository.Delete(review);
            reviewRepository.SaveChanges();
            return RedirectToAction("GetAll");
        }
    }
}
