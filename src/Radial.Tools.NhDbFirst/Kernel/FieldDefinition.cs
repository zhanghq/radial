using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Radial.Tools.NhDbFirst.Data;
using Radial.Tools.NhDbFirst.Properties;

namespace Radial.Tools.NhDbFirst.Kernel
{
    //参考：http://msdn.microsoft.com/query/dev10.query?appId=Dev10IDEF1&l=ZH-CN&k=k(COLUMNPROPERTY_TSQL);k(SQL11.SWB.TSQLRESULTS.F1);k(SQL11.SWB.TSQLQUERY.F1);k(MISCELLANEOUSFILESPROJECT);k(DevLang-TSQL)&rd=true

    /// <summary>
    /// 字段定义
    /// </summary>
    public class FieldDefinition
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
        /// 获取数据类型的长度
        /// </summary>
        public int Length { get; private set; }
        /// <summary>
        /// 获取数据类型的小数位数
        /// </summary>
        public int? Scale { get; private set; }
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
        /// 获取默认值
        /// </summary>
        public string Default { get; private set; }


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
                case DataSource.MySql: return GenerateMySql(profile.ConnectionString, tableDef);
                default: return new List<FieldDefinition>();
            }
        }

        /// <summary>
        /// Generates my SQL.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="tableDef">The table def.</param>
        /// <returns></returns>
        private static IList<FieldDefinition> GenerateMySql(string connectionString, TableDefinition tableDef)
        {
            IList<FieldDefinition> list = new List<FieldDefinition>();

            using (DbSession session = DbSession.NewMySqlSession(connectionString))
            {
                IList<RowDataCollection> rows = session.ExecuteRows(string.Format(Resources.MySqlFieldsQuery, tableDef.Name));

                foreach (RowDataCollection row in rows)
                {
                    var f = new FieldDefinition
                    {
                        Name = row["Field"].ToString(),
                        SqlType = row["Type"].ToString(),
                        IsNullable = row["Null"].Value.ToString() == "NO" ? false : true,
                        Default = row["Default"].ToString()
                    };

                    if (row["Key"].ToString() == "PRI")
                    {
                        f.IsPrimaryKey = true;
                        if (row["Extra"].ToString() == "auto_increment")
                            f.IsIdentity = true;
                    }

                    list.Add(f);
                }

            }

            return list;
        }

        /// <summary>
        /// Generates the SQL server.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="tableDef">The table def.</param>
        /// <returns></returns>
        private static IList<FieldDefinition> GenerateSqlServer(string connectionString, TableDefinition tableDef)
        {
            IList<FieldDefinition> list = new List<FieldDefinition>();

            using (DbSession session = DbSession.NewSqlServerSession(connectionString))
            {
                IList<RowDataCollection> rows = session.ExecuteRows(Resources.SqlServerFieldsQuery, tableDef.Schema, tableDef.Name);

                foreach (RowDataCollection row in rows)
                {
                    var f = new FieldDefinition
                    {
                        Name = row["Name"].ToString(),
                        SqlType = row["Type"].ToString(),
                        IsPrimaryKey = int.Parse(row["IsPrimaryKey"].Value.ToString()) == 1,
                        IsIdentity = int.Parse(row["IsIdentity"].Value.ToString()) == 1,
                        IsRowGuid = int.Parse(row["IsRowGuid"].Value.ToString()) == 1,
                        IsNullable = int.Parse(row["IsNullable"].Value.ToString()) == 1,
                        Length = int.Parse(row["Length"].Value.ToString())
                    };

                    if (row["Scale"].Value != DBNull.Value)
                        f.Scale = int.Parse(row["Scale"].Value.ToString());

                    if (!string.IsNullOrWhiteSpace(row["Default"].ToString()))
                    {
                        if (row["Default"].ToString().StartsWith("(") && row["Default"].ToString().EndsWith(")"))
                        {
                            string temp=row["Default"].ToString().Remove(0, 1);
                            f.Default=temp.Remove(temp.Length-1,1);
                        }
                        else
                            f.Default = row["Default"].ToString();
                    }

                    list.Add(f);
                }

            }

            return list;
        }
    }
}
