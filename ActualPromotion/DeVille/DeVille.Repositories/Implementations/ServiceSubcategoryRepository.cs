using Deville.EntityDataModel;
using System;
using System.Linq;

namespace Deville.Repositories
{
    public class ServiceSubcategoryRepository : IServiceSubcategoryRepository
    {
        private IRepository<ServiceSubcategory> serviceSubcategoryRepository;

        public ServiceSubcategoryRepository(IRepository<ServiceSubcategory> _serviceSubcategoryRepository)
        {
            this.serviceSubcategoryRepository = _serviceSubcategoryRepository;
        }

        public IQueryable<ServiceSubcategory> GetAll()
        {
            return serviceSubcategoryRepository.GetAll();
        }

        public ServiceSubcategory GetById(int id)
        {
            if (id == 0)
                return null;
            return serviceSubcategoryRepository.GetById(id);
        }

        public int Insert(ServiceSubcategory model)
        {
            if (model == null)
                throw new ArgumentNullException("serviceSubcategory");
            return serviceSubcategoryRepository.Insert(model);
        }

        public void Update(ServiceSubcategory model)
        {
            if (model == null)
                throw new ArgumentNullException("serviceSubcategory");
            serviceSubcategoryRepository.Update(model);

        }

        public void Delete(ServiceSubcategory model)
        {
            if (model == null)
                throw new ArgumentNullException("serviceSubcategory");
            serviceSubcategoryRepository.Delete(model);
        }

        public void SaveChanges()
        {
            serviceSubcategoryRepository.SaveChanges();
        }
    }
}
