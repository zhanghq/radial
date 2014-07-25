using NHibernate;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// Straight query using NHibernate.
    /// </summary>
    public sealed class StraightQuery
    {
        IUnitOfWorkEssential _uow;

        /// <summary>
        /// Initializes a new instance of the <see cref="StraightQuery"/> class.
        /// </summary>
        /// <param name="uow">The uow.</param>
        public StraightQuery(IUnitOfWorkEssential uow)
        {
            Checker.Parameter(uow != null, "the IUnitOfWorkEssential instance can not be null");
            _uow = uow;
        }

        /// <summary>
        /// Gets the NHibernate session object.
        /// </summary>
        private ISession Session
        {
            get
            {
                return (ISession)_uow.UnderlyingContext;
            }
        }

        /// <summary>
        /// Prepares the transaction, typically this method is invoked implicitly, but it  also can be explicit used to implement custom control.
        /// </summary>
        /// <param name="level">The isolation level.</param>
        public void PrepareTransaction(IsolationLevel? level = null)
        {
            _uow.PrepareTransaction(level);
        }

        #region Standard Query

        /// <summary>
        /// ExecuteNonQuery.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of rows affected.</returns>
        public int ExecuteNonQuery(string query, params DbParameter[] parameters)
        {
            var cmd = CreateCommand(query, parameters);

            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// ExecuteScalar.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The first column of the first result row.</returns>
        public object ExecuteScalar(string query, params DbParameter[] parameters)
        {
            var cmd = CreateCommand(query, parameters);

            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// ExecuteReader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Data.IDataReader instance.</returns>
        public IDataReader ExecuteReader(string query, params DbParameter[] parameters)
        {
            var cmd = CreateCommand(query, parameters);

            return cmd.ExecuteReader();
        }

        /// <summary>
        /// ExecuteDataTable.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Data.DataTable instance.</returns>
        public DataTable ExecuteDataTable(string query, params DbParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (var reader = ExecuteReader(query, parameters))
            {
                dt.Load(reader);
            }

            return dt;
        }

        /// <summary>
        /// Create System.Data.IDbCommand instance.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// System.Data.IDbCommand instance.
        /// </returns>
        private IDbCommand CreateCommand(string query, params DbParameter[] parameters)
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

        #endregion

        #region Stored Procedure

        /// <summary>
        /// Stored procedure ExecuteNonQuery.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>The number of rows affected.</returns>
        public int SpExecuteNonQuery(string spName, params DbParameter[] parameters)
        {
            var cmd = SpCreateCommand(spName, parameters);

            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Stored procedure ExecuteScalar.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>The first column of the first result row.</returns>
        public object SpExecuteScalar(string spName, params DbParameter[] parameters)
        {
            var cmd = SpCreateCommand(spName, parameters);

            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// Stored procedure ExecuteReader.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>System.Data.IDataReader instance.</returns>
        public IDataReader SpExecuteReader(string spName, params DbParameter[] parameters)
        {
            var cmd = SpCreateCommand(spName, parameters);

            return cmd.ExecuteReader();
        }

        /// <summary>
        /// Stored procedure ExecuteDataTable.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>System.Data.DataTable instance.</returns>
        public DataTable SpExecuteDataTable(string spName, params DbParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (var reader = SpExecuteReader(spName, parameters))
            {
                dt.Load(reader);
            }

            return dt;
        }

        /// <summary>
        /// Create System.Data.IDbCommand instance for stored procedure.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>
        /// System.Data.IDbCommand instance.
        /// </returns>
        private IDbCommand SpCreateCommand(string spName, params DbParameter[] parameters)
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

        #endregion
    }
}
