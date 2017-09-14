using System;

namespace Radial.Persist.Lite
{
    /// <summary>
    /// 表示某一字段的数据
    /// </summary>
    public sealed class FieldData
    {
        /// <summary>
        /// 获取或设置字段名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置字段类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 获取或设置字段的值
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 获取或设置字段的值是否不存在(DbNull).
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
