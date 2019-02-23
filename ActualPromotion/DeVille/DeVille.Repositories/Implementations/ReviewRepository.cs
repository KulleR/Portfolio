using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private IRepository<Review> reviewRepository;

        public ReviewRepository(IRepository<Review> _reviewRepository)
        {
            this.reviewRepository = _reviewRepository;
        }

        public IQueryable<Review> GetAll()
        {
            return reviewRepository.GetAll();
        }

        public Review GetById(int id)
        {
            if (id == 0)
                return null;
            return reviewRepository.GetById(id);
        }

        public int Insert(Review model)
        {
            if (model == null)
                throw new ArgumentNullException("review");
            return reviewRepository.Insert(model);
        }

        public void Update(Review model)
        {
            if (model == null)
                throw new ArgumentNullException("review");
            reviewRepository.Update(model);

        }

        public void Delete(Review model)
        {
            if (model == null)
                throw new ArgumentNullException("review");
            reviewRepository.Delete(model);
        }

        public void SaveChanges()
        {
            reviewRepository.SaveChanges();
        }
    }
}
