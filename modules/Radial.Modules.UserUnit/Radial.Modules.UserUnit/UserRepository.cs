using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial.Persist;
using Radial.Persist.Nhs;

namespace Radial.Modules.UserUnit
{
    /// <summary>
    /// UserRepository
    /// </summary>
    class UserRepository : BasicRepository<User, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserRepository"/> class.
        /// </summary>
        /// <param name="uow">The uow.</param>
        public UserRepository(IUnitOfWorkEssential uow)
            : base(uow)
        {
            SetExtraCondition(o => !o.IsDelete);
        }
    }
}
