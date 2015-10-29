using System;

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

        /// <summary>
        /// 获取或设置数据行的值是否不存在(DbNull).
        /// </summary>
        public bool IsDbNull
        {
            get
            {
                return Value == System.DBNull.Value;
            }
        }

        /// <summary>
        /// 返回表示当前对象的 <see cref="System.String" />.
        /// </summary>
        /// <returns>
        ///  一个 <see cref="System.String" /> 表示当前的对象.
        /// </returns>
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
