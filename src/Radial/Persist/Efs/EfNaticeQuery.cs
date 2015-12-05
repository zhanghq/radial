using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Efs
{
    /// <summary>
    /// Native query using Entity Framework.
    /// </summary>
    public class EfNaticeQuery : NativeQuery
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfNaticeQuery"/> class.
        /// </summary>
        /// <param name="uow">The uow.</param>
        public EfNaticeQuery(IUnitOfWorkEssential uow) : base(uow) { }

        /// <summary>
        /// Gets the database context.
        /// </summary>
        /// <value>
        /// The database context.
        /// </value>
        private DbContext DbContext
        {
            get
            {
                return (DbContext)UnitOfWork.UnderlyingContext;
            }
        }

        /// <summary>
        /// Create System.Data.IDbCommand instance.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// System.Data.IDbCommand instance.
        /// </returns>
        public override IDbCommand CreateCommand(string query, params DbParameter[] parameters)
        {
            if (DbContext.Database.Connection.State == ConnectionState.Closed)
                DbContext.Database.Connection.Open();

            var cmd = DbContext.Database.Connection.CreateCommand();
            cmd.CommandText = query;

            if (DbContext.Database.CurrentTransaction != null)
                cmd.Transaction = DbContext.Database.CurrentTransaction.UnderlyingTransaction;

            if (parameters != null)
            {
                foreach (var p in parameters)
                    cmd.Parameters.Add(p);
            }

            return cmd;
        }

        /// <summary>
        /// Create System.Data.IDbCommand instance for stored procedure.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>
        /// System.Data.IDbCommand instance.
        /// </returns>
        public override IDbCommand SpCreateCommand(string spName, params DbParameter[] parameters)
        {
            if (DbContext.Database.Connection.State == ConnectionState.Closed)
                DbContext.Database.Connection.Open();

            var cmd = DbContext.Database.Connection.CreateCommand();
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;

            if (DbContext.Database.CurrentTransaction != null)
                cmd.Transaction = DbContext.Database.CurrentTransaction.UnderlyingTransaction;

            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    if (p.Direction == ParameterDirection.InputOutput && p.Value == null)
                        p.Value = DBNull.Value;

                    cmd.Parameters.Add(p);
                }
            }

            return cmd;
        }
    }
}
