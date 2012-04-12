using System;
using System.Collections.Generic;
using System.Text;

namespace Radial.DataLite
{
    /// <summary>
    /// 数据源类型
    /// </summary>
    public enum DataSourceType
    {
        /// <summary>
        /// Sql Server 2005 or above
        /// </summary>
        SqlServer9 = 0,
        /// <summary>
        /// Sql Server 2000
        /// </summary>
        SqlServer = 1,
        /// <summary>
        /// Ms Access
        /// </summary>
        MsAccess = 2,
        /// <summary>
        /// Sqlite
        /// </summary>
        Sqlite = 3,
        /// <summary>
        /// MySql
        /// </summary>
        MySql = 4
    }
}
