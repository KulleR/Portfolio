using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Brio.Models
{
    public partial class Article : IEntity
    {
        private IArticleAsideRepository articleAsideRepository = DependencyResolver.Current.GetService(typeof(IArticleAsideRepository)) as IArticleAsideRepository;

        public int ID
        {
            get { return this.ArticleID; }
        }

        /*public ArticleAside Aside
        {
            get
            {
                if (articleAsideRepository != null)
                {
                    return articleAsideRepository.GetAsideOfArticle(this.ID);
                }
                else
                {
                    return null;
                }
            }

            set
            {
                ArticleAside aside = articleAsideRepository.GetAsideOfArticle(this.ID);
                if (aside != null)
                {
                    aside.Text = (value as ArticleAside).Text;
                    aside.Title = (value as ArticleAside).Title;
                    articleAsideRepository.Update(aside);
                }
                else
                {
                    articleAsideRepository.Insert(value as ArticleAside);
                }

                articleAsideRepository.SaveChanges();
            }
        }*/
    }
}