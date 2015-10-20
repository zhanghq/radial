using System;
using Radial.Persist.Lite.Query;

namespace Radial.Persist.Lite
{
    /// <summary>
    /// 查询工厂类
    /// </summary>
    class QueryFactory
    {
        /// <summary>
        /// 创建Sql语句查询类实例
        /// </summary>
        /// <param name="ds">数据源类型</param>
        /// <returns>Sql语句查询类实例</returns>
        public static SqlQuery CreateSqlQueryInstance(DataSource ds)
        {
            switch (ds)
            {
                case DataSource.SqlServer: return new SqlServerQuery();
                case DataSource.MsAccess: return new MsAccessQuery();
                case DataSource.MySql: return new MySqlQuery();
                case DataSource.Sqlite: return new SqliteQuery();
                default: throw new NotSupportedException("暂不支持" + ds.ToString() + "数据源类型");
            }
        }
    }
}
