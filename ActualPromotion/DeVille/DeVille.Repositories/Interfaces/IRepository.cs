using Deville.EntityDataModel;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Deville.Repositories
{
    /// <summary>
    /// Определяет основные методы доступа к хранилищам
    /// </summary>
    /// <typeparam name="TEntity">Тип репозитория</typeparam>
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        IQueryable<TEntity> GetAll();
        TEntity GetById(object id);
        int Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void SaveChanges();

        Task<TEntity> GetByIdAsync(int id);

        IQueryable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);

        Task EditAsync(TEntity entity);

        Task InsertAsync(TEntity entity);

        Task DeleteAsync(TEntity entity);
    }
}
