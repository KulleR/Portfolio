using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public interface IServiceSubcategoryRepository
    {
        IQueryable<ServiceSubcategory> GetAll();
        ServiceSubcategory GetById(int id);
        int Insert(ServiceSubcategory model);
        void Update(ServiceSubcategory model);
        void Delete(ServiceSubcategory model);
        void SaveChanges();
    }
}
