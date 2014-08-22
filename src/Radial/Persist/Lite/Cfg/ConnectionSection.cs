using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Radial.Persist.Lite.Cfg
{
    /// <summary>
    /// 数据库设置组结点
    /// </summary>
    public sealed class ConnectionSection : ConfigurationSection
    {
        /// <summary>
        /// 获取数据库设置集合
        /// </summary>
        [ConfigurationProperty("conns", Options = ConfigurationPropertyOptions.IsDefaultCollection | ConfigurationPropertyOptions.IsRequired)]
        public ConnectionElementCollection Connections
        {
            get
            {
                return (ConnectionElementCollection)this["conns"];
            }
        }
    }
}
