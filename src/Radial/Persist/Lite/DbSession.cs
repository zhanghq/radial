using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data;
using Radial.Persist.Lite.Cfg;
using System.Configuration;

namespace Radial.Persist.Lite
{
    /// <summary>
    /// 数据会话
    /// </summary>
    public class DbSession : IDisposable
    {
        #region Fields

        DataSource ds;
        DbConnection connection;
        DbTransaction transaction;
        SqlQuery sqlQuery;

        int commandTimeout = 30;//default 30 seconds

        #endregion

        #region Constructors

        /// <summary>
        /// 初始化数据会话
        /// </summary>
        public DbSession()
            : this(0)
        {
        }

        /// <summary>
        /// 初始化数据会话
        /// </summary>
        /// <param name="substitution">占位符替换方法</param>
        public DbSession(PlaceholderSubstitution substitution)
            : this(0, substitution)
        {
        }

        /// <summary>
        /// 初始化数据会话
        /// </summary>
        /// <param name="settingsIndex">设置索引(从0开始)</param>
        public DbSession(int settingsIndex)
            : this(settingsIndex, null)
        {
        }

        /// <summary>
        /// 初始化数据会话
        /// </summary>
        /// <param name="settingsIndex">设置索引(从0开始)</param>
        /// <param name="substitution">占位符替换方法</param>
        public DbSession(int settingsIndex, PlaceholderSubstitution substitution)
        {
            ConnectionSection section = ReadCfgSection();
            ConnectionElement settings = section.Connections[settingsIndex];
            if (settings == null)
                throw new ArgumentException("无法找到索引为\"" + settingsIndex + "\"的数据库设置");

            string connectionString = settings.ConnectionString;

            if (substitution != null)
                connectionString = substitution(connectionString);

            Initialize(connectionString, settings.DataSource);
        }

        /// <summary>
        /// 初始化数据会话
        /// </summary>
        /// <param name="settingsName">设置名称(不区分大小写)</param>
        public DbSession(string settingsName)
            : this(settingsName, null)
        {
        }

        /// <summary>
        /// 初始化数据会话
        /// </summary>
        /// <param name="settingsName">设置名称(不区分大小写)</param>
        /// <param name="substitution">占位符替换方法</param>
        public DbSession(string settingsName, PlaceholderSubstitution substitution)
        {
            ConnectionSection section = ReadCfgSection();
            ConnectionElement settings = section.Connections[settingsName];
            if (settings == null)
                throw new ArgumentException("无法找到名称为\"" + settingsName + "\"的数据库设置");

            string connectionString = settings.ConnectionString;

            if (substitution != null)
                connectionString = substitution(connectionString);

            Initialize(connectionString, settings.DataSource);
        }

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

        /// <summary>
        /// 读取配置节点
        /// </summary>
        /// <returns>数据库设置组节点对象</returns>
        private ConnectionSection ReadCfgSection()
        {
            ConnectionSection section = ConfigurationManager.GetSection("connGroup") as ConnectionSection;

            if (section == null)
                throw new ArgumentException("无法获取数据库连接设置节点");

            return section;
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
                return connection.ConnectionString;
            }
        }

        /// <summary>
        /// 获取数据源类型
        /// </summary>
        public DataSource DataSourceType
        {
            get
            {
                return ds;
            }
            private set
            {
                ds = value;
            }
        }

        /// <summary>
        /// 获取或设置数据库连接
        /// </summary>
        private DbConnection Connection
        {
            get
            {
                return connection;
            }
            set
            {
                connection = value;
            }
        }

        /// <summary>
        /// 获取要在数据源执行的DbTransaction事务对象
        /// </summary>
        public DbTransaction Transaction
        {
            get
            {
                return transaction;
            }
            private set
            {
                transaction = value;
            }
        }

        /// <summary>
        /// 获取或设置Sql语句查询类实例
        /// </summary>
        private SqlQuery SqlQuery
        {
            get
            {
                return sqlQuery;
            }
            set
            {
                sqlQuery = value;
            }
        }

        /// <summary>
        /// 获取或设置在终止执行命令的尝试并生成错误之前的等待时间(以秒为单位，默认值为 30 秒)
        /// </summary>
        public int CommandTimeout
        {
            get
            {
                return commandTimeout;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentException("CommandTimeout所分配的属性值不得小于0");

                commandTimeout = value;
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
                if (Transaction != null)
                    Transaction.Dispose();
                Connection.Close();
                Connection.Dispose();
            }
        }

        #endregion

        #region Create DbParameter/DbCommand

        /// <summary>
        /// 返回实现 System.Data.Common.DbCommand 类的提供程序的类的一个新实例。
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>System.Data.Common.DbCommand 的新实例。</returns>
        private DbCommand CreateCommand(string cmdText, params DbParameter[] parameters)
        {
            return CreateCommand(cmdText, CommandType.Text, parameters);
        }

        /// <summary>
        /// 返回实现 System.Data.Common.DbCommand 类的提供程序的类的一个新实例。
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>System.Data.Common.DbCommand 的新实例。</returns>
        private DbCommand CreateCommand(string cmdText, CommandType cmdType, params DbParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(cmdText))
                throw new ArgumentNullException("cmdText", "cmdText不能为空");

            DbCommand cmd = SqlQuery.DbProvider.CreateCommand();
            cmd.CommandText = cmdText;
            cmd.CommandTimeout = CommandTimeout;
            cmd.CommandType = cmdType;

            if (parameters != null)
            {
                foreach (DbParameter p in parameters)
                    cmd.Parameters.Add(p);
            }
            return cmd;
        }


        /// <summary>
        /// 返回 System.Data.SqlClient.SqlParameter 类的新实例。
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数的值</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数最大大小</param>
        /// <param name="direction">参数方向</param>
        /// <returns>System.Data.SqlClient.SqlParameter 的新实例。</returns>
        public System.Data.SqlClient.SqlParameter CreateSqlParameter(string name, object value,
            SqlDbType? dbType = null, int? size = null, ParameterDirection? direction = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name", "name不能为空");

            if (value == null)
                value = DBNull.Value;

            var obj = new System.Data.SqlClient.SqlParameter
            {
                ParameterName = name,
                Value = value
            };

            if (dbType.HasValue)
                obj.SqlDbType = dbType.Value;
            if (size.HasValue)
                obj.Size = size.Value;
            if (direction.HasValue)
                obj.Direction = direction.Value;

            return obj;
        }


        /// <summary>
        /// 返回 System.Data.OleDb.OleDbParameter 类的新实例。
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数的值</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数最大大小</param>
        /// <param name="direction">参数方向</param>
        /// <returns>System.Data.OleDb.OleDbParameter 的新实例。</returns>
        public System.Data.OleDb.OleDbParameter CreateOleDbParameter(string name, object value,
            System.Data.OleDb.OleDbType? dbType = null, int? size = null, ParameterDirection? direction = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name", "name不能为空");

            if (value == null)
                value = DBNull.Value;

            var obj = new System.Data.OleDb.OleDbParameter
            {
                ParameterName = name,
                Value = value
            };

            if (dbType.HasValue)
                obj.OleDbType = dbType.Value;
            if (size.HasValue)
                obj.Size = size.Value;
            if (direction.HasValue)
                obj.Direction = direction.Value;

            return obj;
        }


        /// <summary>
        /// 返回 MySql.Data.MySqlClient.MySqlParameter 类的新实例。
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数的值</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数最大大小</param>
        /// <param name="direction">参数方向</param>
        /// <returns>MySql.Data.MySqlClient.MySqlParameter 的新实例。</returns>
        public MySql.Data.MySqlClient.MySqlParameter CreateMySqlParameter(string name, object value,
            MySql.Data.MySqlClient.MySqlDbType? dbType = null, int? size = null, ParameterDirection? direction = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name", "name不能为空");

            if (value == null)
                value = DBNull.Value;

            var obj = new MySql.Data.MySqlClient.MySqlParameter
            {
                ParameterName = name,
                Value = value
            };

            if (dbType.HasValue)
                obj.MySqlDbType = dbType.Value;
            if (size.HasValue)
                obj.Size = size.Value;
            if (direction.HasValue)
                obj.Direction = direction.Value;

            return obj;
        }

        /// <summary>
        /// 返回 System.Data.Odbc.OdbcParameter  类的新实例。
        /// </summary>
        /// <param name="name">参数名称</param>
        /// <param name="value">参数的值</param>
        /// <param name="dbType">参数类型</param>
        /// <param name="size">参数最大大小</param>
        /// <param name="direction">参数方向</param>
        /// <returns>System.Data.Odbc.OdbcParameter  的新实例。</returns>
        public System.Data.Odbc.OdbcParameter CreateOdbcParameter(string name, object value,
            System.Data.Odbc.OdbcType? dbType = null, int? size = null, ParameterDirection? direction = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("name", "name不能为空");

            if (value == null)
                value = DBNull.Value;

            var obj = new System.Data.Odbc.OdbcParameter
            {
                ParameterName = name,
                Value = value
            };

            if (dbType.HasValue)
                obj.OdbcType = dbType.Value;
            if (size.HasValue)
                obj.Size = size.Value;
            if (direction.HasValue)
                obj.Direction = direction.Value;

            return obj;
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
        private IDataReader ExecuteDataReader(DbCommand command)
        {
            if (command == null || string.IsNullOrEmpty(command.CommandText))
                throw new ArgumentNullException("command", "DbCommand对象及其命令文本不能为空");


            command.Connection = Connection;

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

            if (Transaction != null)
                command.Transaction = Transaction;

            OpenConnection();

            return command.ExecuteScalar();
        }

        #endregion

        #region Public Execute Methods

        /// <summary>
        /// ExecuteDataSet方法
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>DataSet对象</returns>
        public DataSet ExecuteDataSet(string cmdText, params DbParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(cmdText))
                throw new ArgumentNullException("cmdText", "cmdText不能为空");

            return ExecuteDataSet(CreateCommand(cmdText, parameters));
        }

        /// <summary>
        /// ExecuteDataTable方法
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>DataTable对象</returns>
        public DataTable ExecuteDataTable(string cmdText, params DbParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(cmdText))
                throw new ArgumentNullException("cmdText", "cmdText不能为空");

            return ExecuteDataTable(CreateCommand(cmdText, parameters));
        }

        /// <summary>
        /// ExecuteDataReader方法
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>DbDataReader对象</returns>
        public IDataReader ExecuteDataReader(string cmdText, params DbParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(cmdText))
                throw new ArgumentNullException("cmdText", "cmdText不能为空");

            return ExecuteDataReader(CreateCommand(cmdText, parameters));
        }

        /// <summary>
        /// ExecuteNonQuery方法
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>影响的行数</returns>
        public int ExecuteNonQuery(string cmdText, params DbParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(cmdText))
                throw new ArgumentNullException("cmdText", "cmdText不能为空");

            return ExecuteNonQuery(CreateCommand(cmdText, parameters));
        }

        /// <summary>
        /// ExecuteScalar方法
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="parameters">命令参数</param>
        /// <returns>object对象</returns>
        public object ExecuteScalar(string cmdText, params DbParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(cmdText))
                throw new ArgumentNullException("cmdText", "cmdText不能为空");


            return ExecuteScalar(CreateCommand(cmdText, parameters));
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

            return ExecuteDataSet(CreateCommand(spName, CommandType.StoredProcedure, parameters));
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


            return ExecuteDataTable(CreateCommand(spName, CommandType.StoredProcedure, parameters));
        }


        /// <summary>
        /// Stored Procedure ExecuteDataReader
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>DbDataReader对象</returns>
        public IDataReader ExecuteSpDataReader(string spName, params DbParameter[] parameters)
        {
            if (string.IsNullOrEmpty(spName))
                throw new ArgumentNullException("spName", "存储过程名不能为空");

            return ExecuteDataReader(CreateCommand(spName, CommandType.StoredProcedure, parameters));
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

            return ExecuteNonQuery(CreateCommand(spName, CommandType.StoredProcedure, parameters));
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

            return ExecuteScalar(CreateCommand(spName, CommandType.StoredProcedure, parameters));
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
