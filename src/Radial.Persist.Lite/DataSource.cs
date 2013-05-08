using System;
using System.Collections.Generic;
using System.Text;

namespace Radial.Persist.Lite
{
    /// <summary>
    /// 数据源类型
    /// </summary>
    public enum DataSource
    {
        /// <summary>
        /// Sql Server Series
        /// </summary>
        SqlServer = 0,
        /// <summary>
        /// Ms Access
        /// </summary>
        MsAccess = 1,
        /// <summary>
        /// Sqlite
        /// </summary>
        Sqlite = 2,
        /// <summary>
        /// MySql
        /// </summary>
        MySql = 3
    }
}
