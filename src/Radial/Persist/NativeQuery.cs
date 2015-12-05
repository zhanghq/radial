using System.Data;
using System.Data.Common;

namespace Radial.Persist
{
    /// <summary>
    /// INativeQuery
    /// </summary>
    public interface INativeQuery
    {
        #region Standard Query
        /// <summary>
        /// Create System.Data.IDbCommand instance.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// System.Data.IDbCommand instance.
        /// </returns>
        IDbCommand CreateCommand(string query, params DbParameter[] parameters);

        /// <summary>
        /// ExecuteNonQuery.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of rows affected.</returns>
        int ExecuteNonQuery(string query, params DbParameter[] parameters);

        /// <summary>
        /// ExecuteScalar.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The first column of the first result row.</returns>
        object ExecuteScalar(string query, params DbParameter[] parameters);

        /// <summary>
        /// Executes the scalar.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The first column of the first result row.</returns>
        TObject ExecuteScalar<TObject>(string query, params DbParameter[] parameters);

        /// <summary>
        /// ExecuteReader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Data.IDataReader instance.</returns>
        IDataReader ExecuteReader(string query, params DbParameter[] parameters);

        /// <summary>
        /// ExecuteDataTable.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Data.DataTable instance.</returns>
        DataTable ExecuteDataTable(string query, params DbParameter[] parameters);

        #endregion

        #region Stored Procedure
        /// <summary>
        /// Create System.Data.IDbCommand instance for stored procedure.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>
        /// System.Data.IDbCommand instance.
        /// </returns>
        IDbCommand SpCreateCommand(string spName, params DbParameter[] parameters);

        /// <summary>
        /// Stored procedure ExecuteNonQuery.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>The number of rows affected.</returns>
        int SpExecuteNonQuery(string spName, params DbParameter[] parameters);

        /// <summary>
        /// Stored procedure ExecuteScalar.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>The first column of the first result row.</returns>
        object SpExecuteScalar(string spName, params DbParameter[] parameters);


        /// <summary>
        /// Stored procedure ExecuteScalar.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>
        /// The first column of the first result row.
        /// </returns>
        TObject SpExecuteScalar<TObject>(string spName, params DbParameter[] parameters);

        /// <summary>
        /// Stored procedure ExecuteReader.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>System.Data.IDataReader instance.</returns>
        IDataReader SpExecuteReader(string spName, params DbParameter[] parameters);

        /// <summary>
        /// Stored procedure ExecuteDataTable.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>System.Data.DataTable instance.</returns>
        DataTable SpExecuteDataTable(string spName, params DbParameter[] parameters);

        #endregion
    }

    /// <summary>
    /// NativeQuery
    /// </summary>
    public abstract class NativeQuery : INativeQuery
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NativeQuery"/> class.
        /// </summary>
        /// <param name="uow">The uow.</param>
        public NativeQuery(IUnitOfWorkEssential uow)
        {
            Checker.Parameter(uow != null, "the IUnitOfWorkEssential instance can not be null");
            UnitOfWork = uow;
        }

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        protected IUnitOfWorkEssential UnitOfWork
        {
            get;
        }


        /// <summary>
        /// Create System.Data.IDbCommand instance.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>
        /// System.Data.IDbCommand instance.
        /// </returns>
        public abstract IDbCommand CreateCommand(string query, params DbParameter[] parameters);

        /// <summary>
        /// Create System.Data.IDbCommand instance for stored procedure.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>
        /// System.Data.IDbCommand instance.
        /// </returns>
        public abstract IDbCommand SpCreateCommand(string spName, params DbParameter[] parameters);

        #region Standard Query

        /// <summary>
        /// ExecuteNonQuery.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The number of rows affected.</returns>
        public virtual int ExecuteNonQuery(string query, params DbParameter[] parameters)
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
        public virtual object ExecuteScalar(string query, params DbParameter[] parameters)
        {
            var cmd = CreateCommand(query, parameters);

            return cmd.ExecuteScalar();
        }

        /// <summary>
        /// ExecuteScalar.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The first column of the first result row.</returns>
        public virtual TObject ExecuteScalar<TObject>(string query, params DbParameter[] parameters)
        {
            var o = ExecuteScalar(query, parameters);

            if (o == null)
                return default(TObject);

            return (TObject)o;
        }

        /// <summary>
        /// ExecuteReader.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>System.Data.IDataReader instance.</returns>
        public virtual IDataReader ExecuteReader(string query, params DbParameter[] parameters)
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
        public virtual DataTable ExecuteDataTable(string query, params DbParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (var reader = ExecuteReader(query, parameters))
            {
                dt.Load(reader);
            }

            return dt;
        }

        #endregion

        #region Stored Procedure

        /// <summary>
        /// Stored procedure ExecuteNonQuery.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>The number of rows affected.</returns>
        public virtual int SpExecuteNonQuery(string spName, params DbParameter[] parameters)
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
        public virtual object SpExecuteScalar(string spName, params DbParameter[] parameters)
        {
            var cmd = SpCreateCommand(spName, parameters);

            return cmd.ExecuteScalar();
        }


        /// <summary>
        /// Stored procedure ExecuteScalar.
        /// </summary>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>
        /// The first column of the first result row.
        /// </returns>
        public virtual TObject SpExecuteScalar<TObject>(string spName, params DbParameter[] parameters)
        {
            var o = SpExecuteScalar(spName, parameters);

            if (o == null)
                return default(TObject);

            return (TObject)o;
        }

        /// <summary>
        /// Stored procedure ExecuteReader.
        /// </summary>
        /// <param name="spName">The stored procedure name.</param>
        /// <param name="parameters">The stored procedure parameters.</param>
        /// <returns>System.Data.IDataReader instance.</returns>
        public virtual IDataReader SpExecuteReader(string spName, params DbParameter[] parameters)
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
        public virtual DataTable SpExecuteDataTable(string spName, params DbParameter[] parameters)
        {
            DataTable dt = new DataTable();

            using (var reader = SpExecuteReader(spName, parameters))
            {
                dt.Load(reader);
            }

            return dt;
        }

        #endregion
    }
}
