using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;$endif$
using System.Text;
using System.Threading.Tasks;
using $safeprojectname$.Models;

//For Demonstrate Only
namespace $safeprojectname$.Application
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
