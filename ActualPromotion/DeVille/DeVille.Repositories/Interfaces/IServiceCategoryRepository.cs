using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public interface IServiceCategoryRepository
    {
        IQueryable<ServiceCategory> GetAll();
        ServiceCategory GetById(int id);
        int Insert(ServiceCategory model);
        void Update(ServiceCategory model);
        void Delete(ServiceCategory model);
        void SaveChanges();
    }
}
