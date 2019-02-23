using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> GetAll();
        IQueryable<Product> GetProductsByCategoryId(int id);
        Product GetById(int id);
        int Insert(Product model);
        void Update(Product model);
        void Delete(Product model);
        void SaveChanges();
    }
}
