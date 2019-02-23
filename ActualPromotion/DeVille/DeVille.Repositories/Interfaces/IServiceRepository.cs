using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public interface IServiceRepository
    {
        IQueryable<Service> GetAll();
        Service GetById(int id);
        int Insert(Service model);
        void Update(Service model);
        void Delete(Service model);
        void SaveChanges();
    }
}
