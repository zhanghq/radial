using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.UnitTest.Persist.Nhs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Nhs.Repository
{
    class OrderRepository : BasicRepository<Order, int>
    {
        public OrderRepository(IUnitOfWork uow)
            : base(uow)
        {
            SetExtraCondition(o => !o.IsDelete);
        }
    }
}
