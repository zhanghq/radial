using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using System.Text.RegularExpressions;
using System.Configuration;
using Radial.Tools.NhDbFirst.Kernel;

namespace Radial.Tools.NhDbFirst.Data
{
    /// <summary>
    /// 数据会话
    /// </summary>
    public class DbSession : IDisposable
    {
        #region Fields

        DataSource _ds;
        DbConnection _connection;
        DbTransaction _transaction;
        SqlQuery _sqlQuery;

        int _commandTimeout=30;//default 30 seconds

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化数据会话
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="ds">数据源类型</param>
        public DbSession(string connectionString, DataSource ds)
        {
            Initialize(connectionString, ds);
        }

        /// <summary>
        /// 新建SqlServer数据会话
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>数据会话对象</returns>
        public static DbSession NewSqlServerSession(string connectionString)
        {
            return new DbSession(connectionString, DataSource.SqlServer);
        }

        /// <summary>
        /// 新建MySql数据会话
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <returns>数据会话对象</returns>
        public static DbSession NewMySqlSession(string connectionString)
        {
            return new DbSession(connectionString, DataSource.MySql);
        }


        /// <summary>
        /// 初始化数据会话
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="ds">数据源类型</param>
        private void Initialize(string connectionString, DataSource ds)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException("connectionString", "用于打开连接的字符串不能为空");

            SqlQuery = QueryFactory.CreateSqlQueryInstance(ds);
            DataSourceType = ds;
            Connection = SqlQuery.DbProvider.CreateConnection();
            Connection.ConnectionString = connectionString;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取打开连接的字符串
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return _connection.ConnectionString;
            }
        }

        /// <summary>
        /// 获取数据源类型
        /// </summary>
        public DataSource DataSourceType
        {
            get
            {
                return _ds;
            }
            private set
            {
                _ds = value;
            }
        }

        /// <summary>
        /// 获取或设置数据库连接
        /// </summary>
        private DbConnection Connection
        {
            get
            {
                return _connection;
            }
            set
            {
                _connection = value;
            }
        }

        /// <summary>
        /// 获取要在数据源执行的DbTransaction事务对象
        /// </summary>
        public DbTransaction Transaction
        {
            get
            {
                return _transaction;
            }
            private set
            {
                _transaction = value;
            }
        }

        /// <summary>
        /// 获取或设置Sql语句查询类实例
        /// </summary>
        private SqlQuery SqlQuery
        {
            get
            {
                return _sqlQuery;
            }
            set
            {
                _sqlQuery = value;
            }
        }

        /// <summary>
        /// 获取或设置在终止执行命令的尝试并生成错误之前的等待时间(以秒为单位，默认值为 30 秒)
        /// </summary>
        public int CommandTimeout
        {
            get
            {
                return _commandTimeout;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("CommandTimeout所分配的属性值不得小于0");
                _commandTimeout = value;
            }
        }

        #endregion

        #region Transaction

        /// <summary>
        /// 开启DbTransaction事务
        /// </summary>
        public void BeginTransaction()
        {
            OpenConnection();
            Transaction = Connection.BeginTransaction();
        }


        /// <summary>
        /// 开启DbTransaction事务
        /// </summary>
        /// <param name="level">锁定行为</param>
        public void BeginTransaction(IsolationLevel level)
        {
            OpenConnection();
            Transaction = Connection.BeginTransaction(level);
        }

        /// <summary>
        /// 提交DbTransaction事务
        /// </summary>
        public void Commit()
        {
            if (Transaction != null)
            {
                Transaction.Commit();
                Transaction.Dispose();
            }
        }


        /// <summary>
        /// 回滚DbTransaction事务
        /// </summary>
        public void Rollback()
        {
            if (Transaction != null)
            {
                Transaction.Rollback();
                Transaction.Dispose();
            }
        }

        #endregion

        #region Connection

        /// <summary>
        /// 打开数据连接
        /// </summary>
        private void OpenConnection()
        {
            if (Connection != null)
            {
                if (Connection.State == ConnectionState.Closed)
                    Connection.Open();
            }
        }

        /// <summary>
        /// 关闭数据连接
        /// </summary>
        private void CloseConnection()
        {
            if (Connection != null)
            {
                Connection.Close();
                Connection.Dispose();
            }
        }

        #endregion

        #region Create DbParameter

        /// <summary>
        /// 返回实现 System.Data.Common.DbParameter 类的提供程序的类的一个新实例。
        /// </summary>
        /// <returns>System.Data.Common.DbParameter 的新实例。</returns>
        public DbParameter CreateParameter()
        {
            return SqlQuery.DbProvider.CreateParameter();
        }

        #endregion

        #region Private Execute Methods

        /// <summary>
        /// ExecuteDataSet方法
        /// </summary>
        /// <param name="command">DbCommand对象</param>
        /// <returns>DataSet对象</returns>
        private DataSet ExecuteDataSet(DbCommand command)
        {
            if (command == null || string.IsNullOrEmpty(command.CommandText))
                throw new ArgumentNullException("command", "DbCommand对象及其命令文本不能为空");

            DataSet ds = new DataSet();

            command.Connection = Connection;
            command.CommandTimeout = CommandTimeout;

            if (Transaction != null)
                command.Transaction = Transaction;

           
            DbDataAdapter adapter = SqlQuery.DbProvider.CreateDataAdapter();
            adapter.SelectCommand = command;

            OpenConnection();

            adapter.Fill(ds);

            return ds;
        }

        /// <summary>
        /// ExecuteDataTable方法
        /// </summary>
        /// <param name="command">DbCommand对象</param>
        /// <returns>DataTable对象</returns>
        private DataTable ExecuteDataTable(DbCommand command)
        {
            if (command == null || string.IsNullOrEmpty(command.CommandText))
                throw new ArgumentNullException("command", "DbCommand对象及其命令文本不能为空");

            DataTable dt = new DataTable();


            command.Connection = Connection;
            command.CommandTimeout = CommandTimeout;

            if (Transaction != null)
                command.Transaction = Transaction;

            DbDataAdapter adapter = SqlQuery.DbProvider.CreateDataAdapter();
            adapter.SelectCommand = command;

            OpenConnection();

            adapter.Fill(dt);
            return dt;
        }

        /// <summary>
        /// ExecuteDataReader方法
        /// </summary>
        /// <param name="command">DbCommand对象</param>
        /// <returns>DataReader对象</returns>
        private DbDataReader ExecuteDataReader(DbCommand command)
        {
            if (command == null || string.IsNullOrEmpty(command.CommandText))
                throw new ArgumentNullException("command", "DbCommand对象及其命令文本不能为空");


            command.Connection = Connection;
            command.CommandTimeout = CommandTimeout;

            if (Transaction != null)
                command.Transaction = Transaction;

            OpenConnection();

            return command.ExecuteReader();
        }

        /// <summary>
        /// ExecuteNonQuery方法
        /// </summary>
        /// <param name="command">DbCommand对象</param>
        /// <returns>影响的行数</returns>
        private int ExecuteNonQuery(DbCommand command)
        {
            if (command == null || string.IsNullOrEmpty(command.CommandText))
                throw new ArgumentNullException("command", "DbCommand对象及其命令文本不能为空");


            command.Connection = Connection;
            command.CommandTimeout = CommandTimeout;

            if (Transaction != null)
                command.Transaction = Transaction;

            OpenConnection();

            int count = command.ExecuteNonQuery();
            return count;
        }

        /// <summary>
        /// ExecuteScalar方法
        /// </summary>
        /// <param name="command">DbCommand对象</param>
        /// <returns>object对象</returns>
        private object ExecuteScalar(DbCommand command)
        {
            if (command == null || string.IsNullOrEmpty(command.CommandText))
                throw new ArgumentNullException("command", "DbCommand对象及其命令文本不能为空");


            command.Connection = Connection;
            command.CommandTimeout = CommandTimeout;

            if (Transaction != null)
                command.Transaction = Transaction;

            OpenConnection();

            object obj = command.ExecuteScalar();
            return obj;
        }

        #endregion

        #region Public Execute Methods

        /// <summary>
        /// ExecuteDataSet方法
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramValues">参数数组</param>
        /// <returns>DataSet对象</returns>
        public DataSet ExecuteDataSet(string cmdText, params object[] paramValues)
        {
            if (string.IsNullOrEmpty(cmdText))
                throw new ArgumentNullException("cmdText", "cmdText不能为空");

            TextCommandData data = new TextCommandData(cmdText, paramValues);

            return ExecuteDataSet(data);
        }

        /// <summary>
        /// ExecuteDataTable方法
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramValues">参数数组</param>
        /// <returns>DataTable对象</returns>
        public DataTable ExecuteDataTable(string cmdText, params object[] paramValues)
        {
            if (string.IsNullOrEmpty(cmdText))
                throw new ArgumentNullException("cmdText", "cmdText不能为空");

            TextCommandData data = new TextCommandData(cmdText, paramValues);

            return ExecuteDataTable(data);
        }

        /// <summary>
        /// ExecuteDataReader方法
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramValues">参数数组</param>
        /// <returns>DataReader对象</returns>
        public DbDataReader ExecuteDataReader(string cmdText, params object[] paramValues)
        {
            if (string.IsNullOrEmpty(cmdText))
                throw new ArgumentNullException("cmdText", "cmdText不能为空");

            TextCommandData data = new TextCommandData(cmdText, paramValues);

            return ExecuteDataReader(data);
        }

        /// <summary>
        /// ExecuteNonQuery方法
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramValues">参数数组</param>
        /// <returns>影响的行数</returns>
        public int ExecuteNonQuery(string cmdText, params object[] paramValues)
        {
            if (string.IsNullOrEmpty(cmdText))
                throw new ArgumentNullException("cmdText", "cmdText不能为空");

            TextCommandData data = new TextCommandData(cmdText, paramValues);

            return ExecuteNonQuery(data);
        }

        /// <summary>
        /// ExecuteRows方法
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramValues">参数数组</param>
        /// <returns>IList&lt;RowDataCollection&gt;对象</returns>
        public IList<RowDataCollection> ExecuteRows(string cmdText, params object[] paramValues)
        {
            return ExecuteRows(new TextCommandData(cmdText, paramValues));
        }


        /// <summary>
        /// ExecuteFirstRow方法
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramValues">参数数组</param>
        /// <returns>RowDataCollection对象</returns>
        public RowDataCollection ExecuteFirstRow(string cmdText, params object[] paramValues)
        {
            return ExecuteFirstRow(new TextCommandData(cmdText, paramValues));
        }


        /// <summary>
        /// ExecuteScalar方法
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="paramValues">参数数组</param>
        /// <returns>object对象</returns>
        public object ExecuteScalar(string cmdText, params object[] paramValues)
        {
            if (string.IsNullOrEmpty(cmdText))
                throw new ArgumentNullException("cmdText", "cmdText不能为空");

            TextCommandData data = new TextCommandData(cmdText, paramValues);

            return ExecuteScalar(data);
        }

        /// <summary>
        /// ExecuteDataSet方法
        /// </summary>
        /// <param name="cmdData">文本命令对象</param>
        /// <returns>DataSet对象</returns>
        public DataSet ExecuteDataSet(TextCommandData cmdData)
        {
            if (cmdData == null)
                throw new ArgumentNullException("cmdData","cmdData不能为空");

            DbCommand cmd = cmdData.CreateCommand(SqlQuery.DbProvider, SqlQuery.CreateParameterName);

            return ExecuteDataSet(cmd);
        }

        /// <summary>
        /// ExecuteDataTable方法
        /// </summary>
        /// <param name="cmdData">文本命令对象</param>
        /// <returns>DataTable对象</returns>
        public DataTable ExecuteDataTable(TextCommandData cmdData)
        {
            if (cmdData == null)
                throw new ArgumentNullException("cmdData", "cmdData不能为空");

            DbCommand cmd = cmdData.CreateCommand(SqlQuery.DbProvider, SqlQuery.CreateParameterName);

            return ExecuteDataTable(cmd);
        }

        /// <summary>
        /// ExecuteDataReader方法
        /// </summary>
        /// <param name="cmdData">文本命令对象</param>
        /// <returns>DbDataReader对象</returns>
        public DbDataReader ExecuteDataReader(TextCommandData cmdData)
        {
            if (cmdData == null)
                throw new ArgumentNullException("cmdData", "cmdData不能为空");

            DbCommand cmd = cmdData.CreateCommand(SqlQuery.DbProvider, SqlQuery.CreateParameterName);

            return ExecuteDataReader(cmd);
        }

        /// <summary>
        /// ExecuteRows方法
        /// </summary>
        /// <param name="cmdData">文本命令对象</param>
        /// <returns>IList&lt;RowDataCollection&gt;对象</returns>
        public IList<RowDataCollection> ExecuteRows(TextCommandData cmdData)
        {
            IList<RowDataCollection> list = new List<RowDataCollection>();

            using (DbDataReader reader = ExecuteDataReader(cmdData))
            {
                while (reader.Read())
                {
                    RowDataCollection collection = new RowDataCollection();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        collection.Add(new RowData
                        {
                            FieldName = reader.GetName(i),
                            FieldType = reader.GetFieldType(i),
                            Value = reader[i]
                        });
                    }

                    list.Add(collection);
                }
            }

            return list;
        }

        /// <summary>
        /// ExecuteFirstRow方法
        /// </summary>
        /// <param name="cmdData">文本命令对象</param>
        /// <returns>RowDataCollection对象</returns>
        public RowDataCollection ExecuteFirstRow(TextCommandData cmdData)
        {
            RowDataCollection collection = new RowDataCollection();

            using (DbDataReader reader = ExecuteDataReader(cmdData))
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        collection.Add(new RowData
                            {
                                FieldName = reader.GetName(i),
                                FieldType = reader.GetFieldType(i),
                                Value = reader[i]
                            });
                    }
                    break;
                }
            }

            return collection;
        }

        /// <summary>
        /// ExecuteNonQuery方法
        /// </summary>
        /// <param name="cmdData">文本命令对象</param>
        /// <returns>影响的行数</returns>
        public int ExecuteNonQuery(TextCommandData cmdData)
        {
            if (cmdData == null)
                throw new ArgumentNullException("cmdData", "cmdData不能为空");

            DbCommand cmd = cmdData.CreateCommand(SqlQuery.DbProvider, SqlQuery.CreateParameterName);

            return ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// ExecuteScalar方法
        /// </summary>
        /// <param name="cmdData">文本命令对象</param>
        /// <returns>object对象</returns>
        public object ExecuteScalar(TextCommandData cmdData)
        {
            if (cmdData == null)
                throw new ArgumentNullException("cmdData", "cmdData不能为空");

            DbCommand cmd = cmdData.CreateCommand(SqlQuery.DbProvider, SqlQuery.CreateParameterName);

            return ExecuteScalar(cmd);
        }

        #endregion

        #region Public Execute Stored Procedure Methods

        /// <summary>
        /// Stored Procedure ExecuteDataSet
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>DataSet对象</returns>
        public DataSet ExecuteSpDataSet(string spName, params DbParameter[] parameters)
        {
            if (string.IsNullOrEmpty(spName))
                throw new ArgumentNullException("spName", "存储过程名不能为空");

            DbCommand cmd = SqlQuery.DbProvider.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            if (CommandTimeout < 0)
                throw new ArgumentException("CommandTimeout所分配的属性值不得小于0");
            cmd.CommandTimeout = CommandTimeout;

            if (parameters != null)
            {
                foreach (DbParameter p in parameters)
                {
                    //check for derived output value with no value assigned
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(p);
                }
            }
            return ExecuteDataSet(cmd);
        }


        /// <summary>
        /// Stored Procedure ExecuteDataTable
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>DataTable对象</returns>
        public DataTable ExecuteSpDataTable(string spName, params DbParameter[] parameters)
        {
            if (string.IsNullOrEmpty(spName))
                throw new ArgumentNullException("spName", "存储过程名不能为空");

            DbCommand cmd = SqlQuery.DbProvider.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            if (CommandTimeout < 0)
                throw new ArgumentException("CommandTimeout所分配的属性值不得小于0");
            cmd.CommandTimeout = CommandTimeout;

            if (parameters != null)
            {
                foreach (DbParameter p in parameters)
                {
                    //check for derived output value with no value assigned
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(p);
                }
            }

            return ExecuteDataTable(cmd);
        }


        /// <summary>
        /// Stored Procedure ExecuteDataReader
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>DbDataReader对象</returns>
        public DbDataReader ExecuteSpDataReader(string spName, params DbParameter[] parameters)
        {
            if (string.IsNullOrEmpty(spName))
                throw new ArgumentNullException("spName", "存储过程名不能为空");

            DbCommand cmd = SqlQuery.DbProvider.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            if (CommandTimeout < 0)
                throw new ArgumentException("CommandTimeout所分配的属性值不得小于0");
            cmd.CommandTimeout = CommandTimeout;

            if (parameters != null)
            {
                foreach (DbParameter p in parameters)
                {
                    //check for derived output value with no value assigned
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(p);
                }
            }
            return ExecuteDataReader(cmd);
        }

        /// <summary>
        /// Stored Procedure ExecuteRows
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>IList&lt;RowDataCollection&gt;对象</returns>
        public IList<RowDataCollection> ExecuteSpRows(string spName, params DbParameter[] parameters)
        {
            IList<RowDataCollection> list = new List<RowDataCollection>();

            using (DbDataReader reader = ExecuteSpDataReader(spName, parameters))
            {
                while (reader.Read())
                {
                    RowDataCollection collection = new RowDataCollection();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        collection.Add(new RowData
                            {
                                FieldName = reader.GetName(i),
                                FieldType = reader.GetFieldType(i),
                                Value = reader[i]
                            });
                    }

                    list.Add(collection);
                }
            }

            return list;
        }

        /// <summary>
        /// Stored Procedure ExecuteFirstRow
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>RowDataCollection对象</returns>
        public RowDataCollection ExecuteSpFirstRow(string spName, params DbParameter[] parameters)
        {
            RowDataCollection collection = new RowDataCollection();

            using (DbDataReader reader = ExecuteSpDataReader(spName, parameters))
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        collection.Add(new RowData
                        {
                            FieldName = reader.GetName(i),
                            FieldType = reader.GetFieldType(i),
                            Value = reader[i]
                        });
                    }
                    break;
                }
            }

            return collection;
        }

        /// <summary>
        /// Stored Procedure ExecuteNonQuery
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>影响的行数</returns>
        public int ExecuteSpNonQuery(string spName, params DbParameter[] parameters)
        {
            if (string.IsNullOrEmpty(spName))
                throw new ArgumentNullException("spName", "存储过程名不能为空");

            DbCommand cmd = SqlQuery.DbProvider.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            if (CommandTimeout < 0)
                throw new ArgumentException("CommandTimeout所分配的属性值不得小于0");
            cmd.CommandTimeout = CommandTimeout;

            if (parameters != null)
            {
                foreach (DbParameter p in parameters)
                {
                    //check for derived output value with no value assigned
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(p);
                }
            }
            return ExecuteNonQuery(cmd);
        }


        /// <summary>
        /// Stored Procedure ExecuteScalar
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>object对象</returns>
        public object ExecuteSpScalar(string spName, params DbParameter[] parameters)
        {
            if (string.IsNullOrEmpty(spName))
                throw new ArgumentNullException("spName", "存储过程名不能为空");

            DbCommand cmd = SqlQuery.DbProvider.CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = spName;

            if (CommandTimeout < 0)
                throw new ArgumentException("CommandTimeout所分配的属性值不得小于0");
            cmd.CommandTimeout = CommandTimeout;

            if (parameters != null)
            {
                foreach (DbParameter p in parameters)
                {
                    //check for derived output value with no value assigned
                    if ((p.Direction == ParameterDirection.InputOutput) && (p.Value == null))
                    {
                        p.Value = DBNull.Value;
                    }

                    cmd.Parameters.Add(p);
                }
            }

            return ExecuteScalar(cmd);
        }

        #endregion

        #region IDisposable 成员

        /// <summary>
        /// 执行与释放或重置非托管资源相关的应用程序定义的任务。
        /// </summary>
        public void Dispose()
        {
            CloseConnection();
        }

        #endregion
    }
}
