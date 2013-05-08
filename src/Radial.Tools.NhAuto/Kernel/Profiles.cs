using System;
using System.Collections.Generic;
using System.Text;

namespace Radial.Tools.NhAuto.Kernel
{
    /// <summary>
    /// 配置文件
    /// </summary>
    public class Profiles
    {
        public Profiles()
        {
            ModelLazy = true;
        }

        /// <summary>
        /// 获取或设置数据源类型
        /// </summary>
        public DataSource DataSource { get; set; }

        /// <summary>
        /// 获取或设置连接字符串
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 获取或设置模型类的程序集名称
        /// </summary>
        public string ModelAssembly { get; set; }

        /// <summary>
        /// 获取或设置模型类的命名空间
        /// </summary>
        public string ModelNamespace { get; set; }

        /// <summary>
        /// 获取或设置模型类是否延迟加载(&lt;class lazy=&quot;true|false&quot;&gt;)
        /// </summary>
        public bool ModelLazy { get; set; }

        /// <summary>
        /// 获取或设置输出目录
        /// </summary>
        public string OutputDirectory { get; set; }

    }
}
