using Deville.EntityDataModel;
using System;
using System.Linq;

namespace Deville.Repositories
{
    /// <summary>
    /// Предоставляет методы, которые предоставляют доступ к хранилищу пользователей
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private IRepository<User> userRepository;

        public UserRepository(IRepository<User> userRepository)
        {
            this.userRepository = userRepository;
        }
        public IQueryable<User> GetAll()
        {
            return userRepository.GetAll();
        }

        public User GetById(int id)
        {
            if (id == 0)
                return null;
            return userRepository.GetById(id);
        }

        public int Insert(User model)
        {
            if (model == null)
                throw new ArgumentNullException("user");
            return userRepository.Insert(model);
        }

        public void Update(User model)
        {
            if (model == null)
                throw new ArgumentNullException("user");
            userRepository.Update(model);

        }

        public void Delete(User model)
        {
            if (model == null)
                throw new ArgumentNullException("user");
            userRepository.Delete(model);
        }

        public User Login(string email, string password)
        {
            return userRepository.GetAll().FirstOrDefault(p => string.Equals(p.Email, email) && p.Password == password);
        }

        public void SaveChanges()
        {
            userRepository.SaveChanges();
        }

        public User GetByEmail(string email)
        {
            return userRepository.GetAll().FirstOrDefault(p => string.Equals(p.Email, email));
        }
    }
}

