using Brio;
using Brio.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brio
{
    public interface IArticleAsideRepository
    {
        IQueryable<ArticleAside> GetAll();
        ArticleAside GetById(int id);
        ArticleAside GetArticleAside(int articleId);
        int Insert(ArticleAside model);
        void Update(ArticleAside model);
        void Delete(ArticleAside model);
        void SaveChanges();
    }
}
