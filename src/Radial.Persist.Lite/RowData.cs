using System;
using System.Collections.Generic;
using System.Text;

namespace Radial.Persist.Lite
{
    /// <summary>
    /// 表示某一行的数据
    /// </summary>
    public sealed class RowData
    {
        /// <summary>
        /// 获取或设置数据行的字段名
        /// </summary>
        public string FieldName { get; set; }

        /// <summary>
        /// 获取或设置数据行的字段类型
        /// </summary>
        public Type FieldType { get; set; }

        /// <summary>
        /// 获取或设置数据行的值
        /// </summary>
        public object Value { get; set; }
    }
}
