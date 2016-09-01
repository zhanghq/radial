using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist
{
    /// <summary>
    /// Domain service base abstract class.
    /// </summary>
    public abstract class DomainServiceBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DomainServiceBase"/> class.
        /// </summary>
        /// <param name="uow">The IUnitOfWorkEssential instance.</param>
        public DomainServiceBase(IUnitOfWorkEssential uow)
        {
            Checker.Parameter(uow != null, "the IUnitOfWorkEssential instance can not be null");
            UnitOfWork = uow;
        }

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        protected IUnitOfWorkEssential UnitOfWork { get; private set; }
    }
}
