using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public interface IVacancyRepository
    {
        IQueryable<Vacancy> GetAll();
        Vacancy GetById(int id);
        int Insert(Vacancy model);
        void Update(Vacancy model);
        void Delete(Vacancy model);
        void SaveChanges();
    }
}
