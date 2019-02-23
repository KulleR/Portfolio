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

namespace Deville.Controllers
{
    public class ProductCategoryController : Controller
    {
        /// <summary>
        /// Предоставляет доступ к хранилищу данных о продуктах
        /// </summary>
        private readonly IProductCategoryRepository productCategoryRepository;

        public ProductCategoryController(IProductCategoryRepository _productCategoryRepository)
        {
            this.productCategoryRepository = _productCategoryRepository;
        }

        public ActionResult Index()
        {
            return View(productCategoryRepository.GetAll());
        }
    }
}
