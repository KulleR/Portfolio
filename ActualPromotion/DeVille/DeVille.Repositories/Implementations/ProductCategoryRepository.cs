using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Deville.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private IRepository<ProductCategory> productCategoryRepository;

        public ProductCategoryRepository(IRepository<ProductCategory> _productCategoryRepository)
        {
            this.productCategoryRepository = _productCategoryRepository;
        }

        public IQueryable<ProductCategory> GetAll()
        {
            return productCategoryRepository.GetAll().Where(model => model.Status == (int)Status.Active);
        }

        public ProductCategory GetById(int id)
        {
            ProductCategory productCategory = productCategoryRepository.GetById(id);
            return productCategory.Status == (int)Status.Active ? productCategory : null;
        }

        public int Insert(ProductCategory model)
        {
            if (model == null)
                throw new ArgumentNullException("productCategory");
            if (model.Status == 0)
                model.Status = (int)Status.Active;
            return productCategoryRepository.Insert(model);
        }

        public void Update(ProductCategory model)
        {
            if (model == null)
                throw new ArgumentNullException("productCategory");
            productCategoryRepository.Update(model);
        }

        public void Delete(ProductCategory model)
        {
            if (model == null)
                throw new ArgumentNullException("productCategory");
            model.Status = (int)Status.Deleted;
        }

        public void SaveChanges()
        {
            productCategoryRepository.SaveChanges();
        }
    }
}