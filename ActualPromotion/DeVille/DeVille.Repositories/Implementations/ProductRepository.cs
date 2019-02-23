using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Deville.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private IRepository<Product> productRepository;

        public ProductRepository(IRepository<Product> _productRepository)
        {
            this.productRepository = _productRepository;
        }

        public IQueryable<Product> GetAll()
        {
            return productRepository.GetAll();
        }

        public IQueryable<Product> GetProductsByCategoryId(int id)
        {
            return productRepository.GetAll().Where(model => model.CategoryId == id);
        }

        public Product GetById(int id)
        {
            return productRepository.GetById(id);
        }

        public int Insert(Product model)
        {
            if (model == null)
                throw new ArgumentNullException("Product");
            return productRepository.Insert(model);
        }

        public void Update(Product model)
        {
            if (model == null)
                throw new ArgumentNullException("Product");
            productRepository.Update(model);
        }

        public void Delete(Product model)
        {
            if (model == null)
                throw new ArgumentNullException("Product");
            productRepository.Delete(model);
        }

        public void SaveChanges()
        {
            productRepository.SaveChanges();
        }
    }
}