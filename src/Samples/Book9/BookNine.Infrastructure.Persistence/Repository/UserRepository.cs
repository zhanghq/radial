using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookNine.Domain.Model;
using BookNine.Domain.Repository;
using Radial.Persist;
using Radial.Persist.Nhs;

namespace BookNine.Infrastructure.Persistence.Repository
{
    public class UserRepository : BasicRepository<User, int>, IUserRepository
    {
        public UserRepository(IUnitOfWorkEssential uow)
            : base(uow)
        {
        }
    }
}
