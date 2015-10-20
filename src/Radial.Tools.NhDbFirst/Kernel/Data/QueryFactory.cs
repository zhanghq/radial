﻿using System;
using Radial.Tools.NhDbFirst.Data.Query;
using Radial.Tools.NhDbFirst.Kernel;

namespace Radial.Tools.NhDbFirst.Data
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
                case DataSource.MySql: return new MySqlQuery();
                default: throw new NotSupportedException("不支持" + ds.ToString() + "数据源类型");
            }
        }
    }
}
