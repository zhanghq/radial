using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookNine.Domain.Model;
using Radial.Persist;

namespace BookNine.Domain.Repository
{
    public interface IUserRepository : IRepository<User, int>
    {
    }
}
