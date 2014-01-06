using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.UnitTest.Persist.Nhs.Domain;
using System.Data;

namespace Radial.UnitTest.Persist.Nhs.Repository
{
    class UserRepository : BasicRepository<User,int>
    {
        public UserRepository(IUnitOfWork uow)
            : base(uow)
        {
            UseQueryCache = true;
        }

        public DataTable FindAllDataTable()
        {
            return SpExecuteDataTable("Sp_User_All");
        }
    }
}
