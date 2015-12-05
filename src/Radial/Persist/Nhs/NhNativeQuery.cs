using NHibernate;
using System;
using System.Data;
using System.Data.Common;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// Native query using NHibernate.
    /// </summary>
    public class NhNativeQuery: Persist.NativeQuery
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NhNativeQuery"/> class.
        /// </summary>
        /// <param name="uow">The uow.</param>
        public NhNativeQuery(IUnitOfWorkEssential uow) : base(uow) { }


        /// <summary>
        /// Gets the NHibernate session object.
        /// </summary>
        private ISession Session
        {
            get
            {
                return (ISession)UnitOfWork.UnderlyingContext;
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
        public override  IDbCommand CreateCommand(string query, params DbParameter[] parameters)
        {
            //flush previous query to database before create new command.
            Session.Flush();

            var cmd = Session.Connection.CreateCommand();
            cmd.CommandText = query;

            if (Session.Transaction.IsActive)
                Session.Transaction.Enlist(cmd);

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
            //flush previous query to database before create new command.
            Session.Flush();

            var cmd = Session.Connection.CreateCommand();
            cmd.CommandText = spName;
            cmd.CommandType = CommandType.StoredProcedure;

            if (Session.Transaction.IsActive)
                Session.Transaction.Enlist(cmd);

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
