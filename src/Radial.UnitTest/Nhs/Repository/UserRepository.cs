using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial.UnitTest.Nhs.Domain;
using Radial.Data.Nhs;
using NHibernate;
using Radial.Data;

namespace Radial.UnitTest.Nhs.Repository
{
    class UserRepository : BasicRepository<User,int>
    {
        public UserRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
