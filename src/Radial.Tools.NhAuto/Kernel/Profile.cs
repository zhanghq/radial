using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Radial.Tools.NhAuto.Kernel
{
    /// <summary>
    /// 配置文件
    /// </summary>
    class Profile
    {
        public Profile()
        {
            LazyModel = true;
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
        public bool LazyModel { get; set; }

        /// <summary>
        /// 获取或设置模型名称生成脚本
        /// </summary>
        public string ModelNameScript { get; set; }

        /// <summary>
        /// 获取或设置属性名称生成脚本
        /// </summary>
        public string PropertyNameScript { get; set; }

        /// <summary>
        /// 获取或设置输出目录
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// 检查是否有效
        /// </summary>
        public void Validate()
        {
            if (ConnectionString == null || ConnectionString.Trim().Length == 0)
                throw new Exception("数据库连接字符串不能为空");
            if (ModelAssembly == null || ModelAssembly.Trim().Length == 0)
                throw new Exception("模型类程序集名称不能为空");
            if (ModelNamespace == null || ModelNamespace.Trim().Length == 0)
                throw new Exception("模型类命名空间不能为空");
            if (OutputDirectory == null || OutputDirectory.Trim().Length == 0)
                throw new Exception("输出目录不能为空");
        }
    }
}
