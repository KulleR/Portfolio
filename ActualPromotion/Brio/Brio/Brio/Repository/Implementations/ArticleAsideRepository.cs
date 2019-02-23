using Brio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brio
{
    public class ArticleAsideRepository : IArticleAsideRepository
    {
        private IRepository<ArticleAside> articleAsideRepository;

        public ArticleAsideRepository(IRepository<ArticleAside> _articleAsideRepository)
        {
            this.articleAsideRepository = _articleAsideRepository;
        }

        public IQueryable<ArticleAside> GetAll()
        {
            return articleAsideRepository.GetAll();
        }

        public ArticleAside GetById(int id)
        {
            return articleAsideRepository.GetById(id);
        }

        public ArticleAside GetArticleAside(int articleId)
        {
            return articleAsideRepository.GetAll().Where(a => a.Articles.FirstOrDefault().ArticleID == articleId).FirstOrDefault();
        }

        public int Insert(ArticleAside model)
        {
            if (model == null)
                throw new ArgumentNullException("CompanyWork");
            return articleAsideRepository.Insert(model);
        }

        public void Update(ArticleAside model)
        {
            if (model == null)
                throw new ArgumentNullException("CompanyWork");
            articleAsideRepository.Update(model);
        }

        public void Delete(ArticleAside model)
        {
            if (model == null)
                throw new ArgumentNullException("CompanyWork");
            articleAsideRepository.Delete(model);
        }

        public void SaveChanges()
        {
            articleAsideRepository.SaveChanges();
        }
    }
}
