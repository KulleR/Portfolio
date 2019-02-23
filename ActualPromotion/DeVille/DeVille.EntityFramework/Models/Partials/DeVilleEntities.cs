using Deville.EntityDataModel.DataContext;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading.Tasks;

namespace Deville.EntityDataModel
{
    public partial class DeVilleEntities : IDataContext
    {
        DbEntityEntry<TEntity> IDataContext.Entry<TEntity>(TEntity entity)
        {
            return this.Entry(entity);
        }

        DbSet<TEntity> IDataContext.Set<TEntity>()
        {
            return base.Set<TEntity>();
        }

        int SaveChanges()
        {
            return this.SaveChanges();
        }

        void IDataContext.Dispose()
        {
            this.Dispose();
        }

        public Task<int> SaveChangesAsync()
        {
            return this.SaveChangesAsync();
        }
    }
}
