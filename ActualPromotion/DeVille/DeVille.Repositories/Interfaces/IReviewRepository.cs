using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public interface IReviewRepository
    {
        IQueryable<Review> GetAll();
        Review GetById(int id);
        int Insert(Review model);
        void Update(Review model);
        void Delete(Review model);
        void SaveChanges();
    }
}
