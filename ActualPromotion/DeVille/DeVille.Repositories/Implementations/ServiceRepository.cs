using Deville.EntityDataModel;
using System;
using System.Linq;

namespace Deville.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private IRepository<Service> serviceRepository;

        public ServiceRepository(IRepository<Service> _serviceRepository)
        {
            this.serviceRepository = _serviceRepository;
        }

        public IQueryable<Service> GetAll()
        {
            return serviceRepository.GetAll();
        }

        public Service GetById(int id)
        {
            if (id == 0)
                return null;
            return serviceRepository.GetById(id);
        }

        public int Insert(Service model)
        {
            if (model == null)
                throw new ArgumentNullException("service");
            return serviceRepository.Insert(model);
        }

        public void Update(Service model)
        {
            if (model == null)
                throw new ArgumentNullException("service");
            serviceRepository.Update(model);

        }

        public void Delete(Service model)
        {
            if (model == null)
                throw new ArgumentNullException("service");
            serviceRepository.Delete(model);
        }

        public void SaveChanges()
        {
            serviceRepository.SaveChanges();
        }
    }
}
