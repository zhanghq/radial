using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Radial.Data
{
    /// <summary>
    /// Unit of work interface.
    /// </summary>
    public interface IUnitOfWork : IUnitOfWorkEssential
    {

        /// <summary>
        /// Commit changes to data source.
        /// </summary>
        /// <remarks>use underlying transaction automatically when the ambient transaction is null.</remarks>
        void Commit();

        /// <summary>
        /// Commit changes to data source.
        /// </summary>
        /// <remarks>use underlying transaction automatically.</remarks>
        /// <param name="isolationLevel">Isolation level for the new transaction.</param>
        void Commit(IsolationLevel isolationLevel);

    }
}
