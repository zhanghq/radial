using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Radial.DataLite
{
    /// <summary>
    /// 数据库设置
    /// </summary>
    public sealed class ConnectionSettings : ConfigurationElement
    {
        /// <summary>
        /// 获取或设置设置名称
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
                this["name"] = value.Trim().ToLower();
            }
        }


        /// <summary>
        /// 获取或设置连接字符串
        /// </summary>
        [ConfigurationProperty("connectionString", Options = ConfigurationPropertyOptions.IsRequired)]
        public string ConnectionString
        {
            get
            { return (string)this["connectionString"]; }
            set
            { this["connectionString"] = value.Trim(); }
        }

        /// <summary>
        /// 获取或设置数据源类型
        /// </summary>
        [ConfigurationProperty("type", DefaultValue = "SqlServer")]
        public DataSourceType DataSourceType
        {
            get
            { return (DataSourceType)this["type"]; }
            set
            { this["type"] = value; }
        }
    }
}
