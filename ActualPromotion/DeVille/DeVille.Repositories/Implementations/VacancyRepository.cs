using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public class VacancyRepository : IVacancyRepository
    {
        private IRepository<Vacancy> vacancyRepository;

        public VacancyRepository(IRepository<Vacancy> _vacancyRepository)
        {
            this.vacancyRepository = _vacancyRepository;
        }

        public IQueryable<Vacancy> GetAll()
        {
            return vacancyRepository.GetAll();
        }

        public Vacancy GetById(int id)
        {
            if (id == 0)
                return null;
            return vacancyRepository.GetById(id);
        }

        public int Insert(Vacancy model)
        {
            if (model == null)
                throw new ArgumentNullException("vacancyRepository");
            return vacancyRepository.Insert(model);
        }

        public void Update(Vacancy model)
        {
            if (model == null)
                throw new ArgumentNullException("vacancyRepository");
            vacancyRepository.Update(model);

        }

        public void Delete(Vacancy model)
        {
            if (model == null)
                throw new ArgumentNullException("vacancyRepository");
            vacancyRepository.Delete(model);
        }

        public void SaveChanges()
        {
            vacancyRepository.SaveChanges();
        }
    }
}
