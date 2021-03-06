﻿using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;$endif$
using System.Text;
using System.Threading.Tasks;
using Radial.Persist;
using $safeprojectname$.Models;

//For Demonstrate Only
namespace $safeprojectname$.Domain
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
