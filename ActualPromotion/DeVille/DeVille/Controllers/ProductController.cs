using Deville.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Deville.Controllers
{
    public class ProductController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о продуктах
        /// </summary>
        private readonly IProductRepository productRepository;

        /// <summary>
        /// Предоставляет доступ к хранилищу данных о категориях продукта
        /// </summary>
        private readonly IProductCategoryRepository productCategoryRepository;

        public ProductController(IProductRepository _productRepository, IProductCategoryRepository _productCategoryRepository)
        {
            this.productRepository = _productRepository;
            this.productCategoryRepository = _productCategoryRepository;
        }

        public ActionResult Index(int? categoryId)
        {
            if (categoryId.HasValue && categoryId > 0)
            {
                ViewBag.Category = productCategoryRepository.GetById(categoryId.Value);
                return View(productRepository.GetProductsByCategoryId(categoryId.Value));
            }

            return RedirectToAction("Index", "ProductCategory");
        }

        public ActionResult GetNovelty()
        {
            return View(productRepository.GetAll().Where(model => model.IsNovelty));
        }
    }
}
