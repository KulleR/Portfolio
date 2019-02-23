using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public interface IProductCategoryRepository
    {
        IQueryable<ProductCategory> GetAll();
        ProductCategory GetById(int id);
        int Insert(ProductCategory model);
        void Update(ProductCategory model);
        void Delete(ProductCategory model);
        void SaveChanges();
    }
}
