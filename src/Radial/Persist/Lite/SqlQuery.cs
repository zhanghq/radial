using System.Data.Common;

namespace Radial.Persist.Lite
{
    /// <summary>
    /// Sql语句查询基类
    /// </summary>
    abstract class SqlQuery
    {
        /// <summary>
        /// 创建参数名称
        /// </summary>
        /// <param name="parameterIndex">参数索引</param>
        /// <returns>参数名称</returns>
        public abstract string CreateParameterName(int parameterIndex);

        /// <summary>
        /// 获取提供程序
        /// </summary>
        public abstract DbProviderFactory DbProvider { get; }

    }
}
