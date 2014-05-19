using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Radial.Persist;
using QuickStart.Models;

namespace QuickStart.Domain.Repos
{
    /// <summary>
    /// IUserRepository
    /// </summary>
    public interface IUserRepository : IRepository<User, string>
    {
    }
}
