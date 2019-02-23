using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HalalEcodes.Data.Models;
using HalalEcodes.Data.Repositories.Interfaces;

namespace HalalEcodes.Data.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        private EcodeContext DbContext { get; set; }

        public Repository() { }

        public Repository(EcodeContext context)
        {
            DbContext = context;
        }

        public void Delete(TEntity entity)
        {
            DbContext.Remove(entity);
            DbContext.SaveChanges();
        }

        public TEntity Get(long id)
        {
            return DbContext.Find<TEntity>(id);
        }

        public IQueryable<TEntity> GetAll()
        {
            return DbContext.Set<TEntity>();
        }

        public long Save(TEntity entity)
        {
            DbContext.Add(entity);
            DbContext.SaveChanges();
            return entity.Id;
        }
    }
}
