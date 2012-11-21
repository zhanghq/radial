using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Data.Nhs.Param
{
    /// <summary>
    /// Param Repository
    /// </summary>
    class ParamRepository : BasicRepository<ParamEntity, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParamRepository" /> class.
        /// </summary>
        /// <param name="uow">The IUnitOfWork instance.</param>
        public ParamRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
