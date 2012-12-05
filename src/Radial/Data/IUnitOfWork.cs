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
        /// <param name="autoGenerateTransaction">If need to use automatic transaction set to <c>true</c>, default is <c>false</c>.</param>
        void Commit(bool autoGenerateTransaction = false);

        /// <summary>
        /// Commit changes to data source with automatic transaction.
        /// </summary>
        /// <param name="isolationLevel">Isolation level for the new transaction.</param>
        void Commit(IsolationLevel isolationLevel);

    }
}
