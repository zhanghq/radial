using QuickStart.Domain;
using QuickStart.Domain.Repos;
using QuickStart.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickStart.Application.Impl
{
    /// <summary>
    /// UserService
    /// </summary>
    class UserService : ServiceBase, IUserService
    {
        /// <summary>
        /// Creates the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public User Create(string name, string email)
        {
            using (var uow = ResolveUnitOfWork())
            {
                UserManager um = new UserManager(uow);
                User u = um.Create(name, email);

                uow.RegisterNew<User>(u);

                uow.Commit();

                return u;
            }
        }


        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public IList<User> GetAll()
        {
            using (var uow = ResolveUnitOfWork())
            {
                IUserRepository repo = ResolveRepository<IUserRepository>(uow);

                return repo.FindAll();
            }
        }
    }
}
