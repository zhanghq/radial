using System;
using System.Collections.Generic;
using System.Text;

namespace Radial.DataLite
{
    /// <summary>
    /// 占位符替换方法
    /// </summary>
    /// <param name="originalString">包含占位符的原始字符串</param>
    /// <returns>替换占位符后的字符串</returns>
    public delegate string PlaceholderSubstitution(string originalString);

    /// <summary>
    /// 参数名称构建方法
    /// </summary>
    /// <param name="parameterIndex">参数索引</param>
    /// <returns>参数名称</returns>
    public delegate string ParameterNameBuilder(int parameterIndex);
}
