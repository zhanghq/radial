using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial.Persist;
using Radial.Modules.UserUnit.Models;

//For Demonstrate Only
namespace Radial.Modules.UserUnit.Domain
{
    /// <summary>
    /// UserManager
    /// </summary>
    class UserManager : ManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserManager"/> class.
        /// </summary>
        /// <param name="uow">The uow.</param>
        public UserManager(IUnitOfWorkEssential uow) : base(uow) { }

        /// <summary>
        /// Creates the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        public User Create(string name, string email)
        {
            return new User { Name = name, Email = email };
        }
    }
}
