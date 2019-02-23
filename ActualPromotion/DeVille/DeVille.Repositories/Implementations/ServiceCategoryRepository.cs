using Deville.EntityDataModel;
using System;
using System.Linq;

namespace Deville.Repositories
{
    public class ServiceCategoryRepository : IServiceCategoryRepository
    {
        private IRepository<ServiceCategory> serviceCategoryRepository;

        public ServiceCategoryRepository(IRepository<ServiceCategory> _serviceCategoryRepository)
        {
            this.serviceCategoryRepository = _serviceCategoryRepository;
        }

        public IQueryable<ServiceCategory> GetAll()
        {
            return serviceCategoryRepository.GetAll();
        }

        public ServiceCategory GetById(int id)
        {
            if (id == 0)
                return null;
            return serviceCategoryRepository.GetById(id);
        }

        public int Insert(ServiceCategory model)
        {
            if (model == null)
                throw new ArgumentNullException("serviceCategory");
            return serviceCategoryRepository.Insert(model);
        }

        public void Update(ServiceCategory model)
        {
            if (model == null)
                throw new ArgumentNullException("serviceCategory");
            serviceCategoryRepository.Update(model);

        }

        public void Delete(ServiceCategory model)
        {
            if (model == null)
                throw new ArgumentNullException("serviceCategory");
            serviceCategoryRepository.Delete(model);
        }

        public void SaveChanges()
        {
            serviceCategoryRepository.SaveChanges();
        }
    }
}
