using $safeprojectname$.Domain.Repos;
using $safeprojectname$.Models;
using Radial.Persist;
using Radial.Persist.Nhs;
using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;$endif$
using System.Text;
using System.Threading.Tasks;

//For Demonstrate Only
namespace $safeprojectname$.Infras.Repos.SqlClient
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
