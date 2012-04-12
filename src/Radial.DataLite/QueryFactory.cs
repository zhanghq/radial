using System;
using System.Collections.Generic;
using System.Text;
using Radial.DataLite.Query;

namespace Radial.DataLite
{
    /// <summary>
    /// 查询工厂类
    /// </summary>
    class QueryFactory
    {
        /// <summary>
        /// 创建Sql语句查询类实例
        /// </summary>
        /// <param name="dsType">数据源类型</param>
        /// <returns>Sql语句查询类实例</returns>
        public static SqlQuery CreateSqlQueryInstance(DataSourceType dsType)
        {
            switch (dsType)
            {
                case DataSourceType.SqlServer:
                case DataSourceType.SqlServer9: return new SqlServerQuery();
                case DataSourceType.MsAccess: return new MsAccessQuery();
                default: throw new NotSupportedException("不支持" + dsType.ToString() + "数据源类型");
            }
        }
    }
}
