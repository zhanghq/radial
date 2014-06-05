using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial.Modules.UserUnit.Models;

//For Demonstrate Only
namespace Radial.Modules.UserUnit.Application
{
    /// <summary>
    /// IUserService
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Creates the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        User Create(string name, string email);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IList<User> GetAll();
    }
}
