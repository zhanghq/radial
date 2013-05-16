using System;
using System.Collections.Generic;
using System.Text;
using Radial.Tools.NhAuto.Data;
using Radial.Tools.NhAuto.Properties;

namespace Radial.Tools.NhAuto.Kernel
{
    /// <summary>
    /// 字段定义
    /// </summary>
    class FieldDefinition
    {
        /// <summary>
        /// 获取字段名
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// 获取字段Sql类型
        /// </summary>
        public string SqlType { get; private set; }
        /// <summary>
        /// 获取字段长度
        /// </summary>
        public int Length { get; private set; }
        /// <summary>
        /// 获取是否为主键
        /// </summary>
        public bool IsPrimaryKey { get; private set; }
        /// <summary>
        /// 获取是否为自增标识
        /// </summary>
        public bool IsIdentity { get; private set; }
        /// <summary>
        /// 获取是否为RowGuid标识
        /// </summary>
        public bool IsRowGuid { get; private set; }
        /// <summary>
        /// 获取是否允许为空
        /// </summary>
        public bool IsNullable { get; private set; }


        /// <summary>
        /// Generates the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <param name="tableDef">The table definition.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException">不支持的数据源类型</exception>
        public static IList<FieldDefinition> Generate(Profile profile, TableDefinition tableDef)
        {
            switch (profile.DataSource)
            {
                case DataSource.SqlServer: return GenerateSqlServer(profile.ConnectionString, tableDef);
                default: throw new NotSupportedException("不支持的数据源类型：" + profile.DataSource.ToString());
            }
        }

        private static IList<FieldDefinition> GenerateSqlServer(string connectionString, TableDefinition tableDef)
        {
            IList<FieldDefinition> list = new List<FieldDefinition>();

            using (DbSession session = DbSession.NewSqlServerSession(connectionString))
            {
                IList<RowDataCollection> rows = session.ExecuteRows(Resources.SqlServerFieldDefinitionQuery, tableDef.SchemaName, tableDef.TableName);

                foreach (RowDataCollection row in rows)
                {
                    list.Add(new FieldDefinition
                    {
                        Name = row["Name"].ToString(),
                        SqlType = row["Type"].ToString(),
                        Length = int.Parse(row["Length"].Value.ToString()),
                        IsPrimaryKey = int.Parse(row["IsPrimaryKey"].Value.ToString()) == 1,
                        IsIdentity = int.Parse(row["IsIdentity"].Value.ToString()) == 1,
                        IsRowGuid = int.Parse(row["IsRowGuid"].Value.ToString()) == 1,
                        IsNullable = int.Parse(row["IsNullable"].Value.ToString()) == 1
                    });
                }

            }

            return list;
        }
    }
}
