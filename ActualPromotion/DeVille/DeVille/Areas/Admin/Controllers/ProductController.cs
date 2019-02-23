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
    public class ProductController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о продуктах
        /// </summary>
        private readonly IProductRepository productRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о категориях продуктов
        /// </summary>
        private readonly IProductCategoryRepository productCategoryRepository;

        /// <summary>
        /// Предоставляет доступ к отображателю объекта на оъект
        /// </summary>
        private readonly IMapper mapper;

        public ProductController(IProductRepository _productRepository, IProductCategoryRepository _productCategoryRepository, IMapper _mapper)
        {
            this.productRepository = _productRepository;
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
            List<ProductCategory> categories = productCategoryRepository.GetAll().ToList();
            ViewBag.ProductCategory = new SelectList(categories, "Id", "Title");
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddProduct model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid && (ImgCover != null && ImgCover.ContentLength > 0))
            {
                Product product = mapper.Map(model, typeof(AddProduct), typeof(Product)) as Product;

                var fileName = Path.GetFileName(ImgCover.FileName);
                var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                ImgCover.SaveAs(savingPath);
                product.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));

                productRepository.Insert(product);
                productRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            List<ProductCategory> categories = productCategoryRepository.GetAll().ToList();
            ViewBag.ProductCategory = new SelectList(categories, "Id", "Title");

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            List<ProductCategory> categories = productCategoryRepository.GetAll().ToList();
            ViewBag.ProductCategory = new SelectList(categories, "Id", "Title");

            Product product = productRepository.GetById(id);
            return View(mapper.Map(product, typeof(Product), typeof(EditProduct)) as EditProduct);
        }

        [HttpPost]
        public ActionResult Edit(EditProduct model, HttpPostedFileBase ImgCover)
        {
            if (ModelState.IsValid)
            {
                Product product = productRepository.GetById(model.Id);
                product.Title = model.Title;
                product.Description = model.Description;
                product.Price = model.Price;
                product.Article = model.Article;
                product.IsNovelty = model.IsNovelty;


                if (ImgCover != null && ImgCover.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(ImgCover.FileName);
                    var savingPath = Path.Combine(HttpContext.Server.MapPath(AppSettings.photoUploadDirectory), fileName);
                    ImgCover.SaveAs(savingPath);
                    product.ImgCover = VirtualPathUtility.ToAbsolute(Path.Combine(AppSettings.photoUploadDirectory, fileName));
                }

                productRepository.Update(product);
                productRepository.SaveChanges();

                return RedirectToAction("Index");
            }

            List<ProductCategory> categories = productCategoryRepository.GetAll().ToList();
            ViewBag.ProductCategory = new SelectList(categories, "Id", "Title");
            return View(model);
        }

        public ActionResult Delete(int id)
        {
            Product product = productRepository.GetById(id);
            productRepository.Delete(product);
            productRepository.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
