using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Radial.Tools.NhDbFirst.Data;
using Radial.Tools.NhDbFirst.Properties;

namespace Radial.Tools.NhDbFirst.Kernel
{
    /// <summary>
    /// 数据表定义
    /// </summary>
    public class TableDefinition
    {
        /// <summary>
        /// 获取表Schema名称
        /// </summary>
        public string Schema { get; private set; }

        /// <summary>
        /// 获取表名
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            if (Schema != null)
                return (Schema.ToLower() + Name.ToLower()).GetHashCode();
            return Name.ToLower().GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj == null)
                return false;

            return this.GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// ==s the specified a.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static bool operator ==(TableDefinition a, TableDefinition b)
        {
            return a.Equals(b);
        }

        /// <summary>
        /// !=s the specified a.
        /// </summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns></returns>
        public static bool operator !=(TableDefinition a, TableDefinition b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Retrieves the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">不支持的数据源类型</exception>
        public static IList<TableDefinition> Retrieve(Profile profile)
        {
            switch (profile.DataSource)
            {
                case DataSource.SqlServer: return RetrieveSqlServer(profile.ConnectionString);
                case DataSource.MySql: return RetrieveMySql(profile.ConnectionString);
                default: return new List<TableDefinition>();
            }
        }

        /// <summary>
        /// Retrieves my SQL.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private static IList<TableDefinition> RetrieveMySql(string connectionString)
        {
            IList<TableDefinition> list = new List<TableDefinition>();

            using (DbSession session = DbSession.NewMySqlSession(connectionString))
            {
                IList<RowDataCollection> rows = session.ExecuteRows(Resources.MySqlTablesQuery);

                foreach (RowDataCollection row in rows)
                {
                    list.Add(new TableDefinition { Name = row[0].ToString() });
                }
            }

            return list;
        }

        /// <summary>
        /// Retrieves the SQL server.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        private static IList<TableDefinition> RetrieveSqlServer(string connectionString)
        {
            IList<TableDefinition> list = new List<TableDefinition>();

            using (DbSession session = DbSession.NewSqlServerSession(connectionString))
            {
                IList<RowDataCollection> rows = session.ExecuteRows(Resources.SqlServerTablesQuery);

                foreach (RowDataCollection row in rows)
                {
                    list.Add(new TableDefinition { Schema = row["SchemaName"].ToString(), Name = row["TableName"].ToString() });
                }
            }

            return list;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            if (!string.IsNullOrWhiteSpace(Schema))
                return string.Join(".", Schema, Name);
            return Name;
        }
    }
}
