using QuickStart.Domain.Repos;
using QuickStart.Models;
using Radial.Persist;
using Radial.Persist.Nhs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickStart.Infras.Repos.SqlClient
{
    /// <summary>
    /// UserRepository
    /// </summary>
    class UserRepository : BasicRepository<User, string>, IUserRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="uow">The uow.</param>
        public UserRepository(IUnitOfWorkEssential uow)
            : base(uow)
        { }
    }
}
