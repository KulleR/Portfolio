using Deville.EntityDataModel;
using System.Linq;

namespace Deville.Repositories
{
    /// <summary>
    /// Определяют методы, которые предоставляют доступ к хранилищу пользователей
    /// </summary>
    public interface IUserRepository
    {
        IQueryable<User> GetAll();
        User GetById(int id);
        int Insert(User model);
        void Update(User model);
        void Delete(User model);
        User Login(string email, string password);
        User GetByEmail(string email);
        void SaveChanges();
    }
}
