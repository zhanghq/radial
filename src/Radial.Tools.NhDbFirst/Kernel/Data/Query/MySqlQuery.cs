using System;
using System.Collections.Generic;
using System.Text;
using MySql.Data.MySqlClient;

namespace Radial.Tools.NhDbFirst.Data.Query
{
    /// <summary>
    /// MySql查询
    /// </summary>
    class MySqlQuery : SqlQuery
    {
        /// <summary>
        /// 创建参数名称
        /// </summary>
        /// <param name="parameterIndex">参数索引</param>
        /// <returns>参数名称</returns>
        public override string CreateParameterName(int parameterIndex)
        {
            if (parameterIndex < 0)
                throw new ArgumentException("参数索引不能小于0", "parameterIndex");
            return "@P" + parameterIndex.ToString();
        }

        /// <summary>
        /// 获取提供程序
        /// </summary>
        /// <value></value>
        public override System.Data.Common.DbProviderFactory DbProvider
        {
            get { return MySqlClientFactory.Instance; }
        }
    }
}
