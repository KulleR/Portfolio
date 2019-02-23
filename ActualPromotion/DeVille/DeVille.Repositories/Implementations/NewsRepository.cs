using Deville.EntityDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    public class NewsRepository : INewsRepository
    {
        private IRepository<News> newsRepository;

        public NewsRepository(IRepository<News> _newsRepository)
        {
            this.newsRepository = _newsRepository;
        }

        public IQueryable<News> GetAll()
        {
            return newsRepository.GetAll();
        }

        public News GetById(int id)
        {
            return newsRepository.GetById(id);
        }

        public int Insert(News model)
        {
            if (model == null)
                throw new ArgumentNullException("News");
            return newsRepository.Insert(model);
        }

        public void Update(News model)
        {
            if (model == null)
                throw new ArgumentNullException("News");
            newsRepository.Update(model);
        }

        public void Delete(News model)
        {
            if (model == null)
                throw new ArgumentNullException("News");
            newsRepository.Delete(model);
        }

        public void SaveChanges()
        {
            newsRepository.SaveChanges();
        }
    }
}
