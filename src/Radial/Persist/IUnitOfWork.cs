using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Radial.Persist
{
    /// <summary>
    /// Unit of work interface.
    /// </summary>
    public interface IUnitOfWork : IUnitOfWorkEssential
    {
        /// <summary>
        /// Commit changes to data source.
        /// </summary>
        void Commit();

    }
}
