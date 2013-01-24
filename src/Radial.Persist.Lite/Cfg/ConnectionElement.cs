using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Radial.Persist.Lite.Cfg
{
    /// <summary>
    /// 数据库配置元素
    /// </summary>
    public sealed class ConnectionElement : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置名称
        /// </summary>
        [ConfigurationProperty("name", Options = ConfigurationPropertyOptions.IsKey | ConfigurationPropertyOptions.IsRequired)]
        public string Name
        {
            get
            {
                return (string)this["name"];
            }
            set
            {
                this["name"] = value;
            }
        }


        /// <summary>
        /// 获取或设置连接字符串
        /// </summary>
        [ConfigurationProperty("conn", Options = ConfigurationPropertyOptions.IsRequired)]
        public string ConnectionString
        {
            get
            { return (string)this["conn"]; }
            set
            { this["conn"] = value.Trim(); }
        }

        /// <summary>
        /// 获取或设置数据源类型
        /// </summary>
        [ConfigurationProperty("type", DefaultValue = DataSourceType.SqlServer)]
        public DataSourceType DataSourceType
        {
            get
            { return (DataSourceType)this["type"]; }
            set
            { this["type"] = value; }
        }
    }
}
