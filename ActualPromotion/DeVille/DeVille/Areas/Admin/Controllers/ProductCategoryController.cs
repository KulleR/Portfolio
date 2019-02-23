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
    public class ProductCategoryController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к отображателю объекта на оъект
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о категориях услуг
        /// </summary>
        private readonly IProductCategoryRepository productCategoryRepository;

        public ProductCategoryController(IProductCategoryRepository _productCategoryRepository, IMapper _mapper)
        {
            this.productCategoryRepository = _productCategoryRepository;
            this.mapper = _mapper;
        }

        public ActionResult Index()
        {
            return View(productCategoryRepository.GetAll());
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddProductCategory model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid && (ImgCover != null && ImgCover.ContentLength > 0))
            {
                ProductCategory productCategory = mapper.Map(model, typeof(AddProductCategory), typeof(ProductCategory)) as ProductCategory;

                var fileName = Path.GetFileName(ImgCover.FileName);
                var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                ImgCover.SaveAs(savingPath);
                productCategory.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));

                productCategoryRepository.Insert(productCategory);
                productCategoryRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            ProductCategory serviceCategory = productCategoryRepository.GetById(id);
            return View(mapper.Map(serviceCategory, typeof(ProductCategory), typeof(EditProductCategory)) as EditProductCategory);
        }

        [HttpPost]
        public ActionResult Edit(EditProductCategory model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid)
            {
                ProductCategory productCategory = productCategoryRepository.GetById(model.Id);
                productCategory.Title = model.Title;
                productCategory.Description = model.Description;

                if (ImgCover != null && ImgCover.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImgCover.FileName);
                    var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                    ImgCover.SaveAs(savingPath);
                    productCategory.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));
                }

                productCategoryRepository.Update(productCategory);
                productCategoryRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            ProductCategory productCategory = productCategoryRepository.GetById(id);
            productCategoryRepository.Delete(productCategory);
            productCategoryRepository.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}
