using HalalEcodes.Data.Models;
using System.Linq;

namespace HalalEcodes.Data.Repositories.Interfaces
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Получить сущность по идентификатору. В ряде случаев использование GetOrThrow более предпочтительно.
        /// </summary>
        /// <param name="id">Идентификатор сущности</param>
        /// <returns>Сущность с указанным Id, если существует. Иначе - null.</returns>
        TEntity Get(long id);

        /// <summary>
        /// Загрузка всех объектов данной сущности
        /// </summary>
        /// <returns>Неупорядоченный список всех объектов</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Сохранить объект сущность
        /// </summary>
        /// <param name="entity">Сохраняемый объект</param>
        long Save(TEntity entity);

        /// <summary>
        /// Удалить объект сущности
        /// </summary>
        /// <param name="entity">Удаляемая сущность</param>
        void Delete(TEntity entity);
    }
}
