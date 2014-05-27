using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;$endif$
using System.Text;
using System.Threading.Tasks;
using Radial.Persist;
using $safeprojectname$.Models;

//For Demonstrate Only
namespace $safeprojectname$.Domain.Repos
{
    /// <summary>
    /// IUserRepository
    /// </summary>
    public interface IUserRepository : IRepository<User, string>
    {
    }
}
