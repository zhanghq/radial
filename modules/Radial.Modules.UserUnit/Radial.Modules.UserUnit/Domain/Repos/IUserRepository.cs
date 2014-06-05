using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial.Persist;
using Radial.Modules.UserUnit.Models;

//For Demonstrate Only
namespace Radial.Modules.UserUnit.Domain.Repos
{
    /// <summary>
    /// IUserRepository
    /// </summary>
    public interface IUserRepository : IRepository<User, string>
    {
    }
}
