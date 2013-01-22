using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Radial.Persist.Lite
{
    /// <summary>
    /// 数据库设置组结点
    /// </summary>
    public sealed class ConnectionGroupSection : ConfigurationSection
    {
        /// <summary>
        /// 获取数据库设置集合
        /// </summary>
        [ConfigurationProperty("connections", Options = ConfigurationPropertyOptions.IsDefaultCollection | ConfigurationPropertyOptions.IsRequired)]
        public ConnectionSettingsCollection Connections
        {
            get
            {
                return (ConnectionSettingsCollection)this["connections"];
            }
        }

        /// <summary>
        /// 读取配置节点
        /// </summary>
        /// <returns>数据库设置组节点对象</returns>
        public static ConnectionGroupSection Read()
        {
            ConnectionGroupSection section = ConfigurationManager.GetSection("connectionGroup") as ConnectionGroupSection;

            if (section == null)
                throw new ArgumentException("无法获取数据库连接设置节点");

            return section;
        }
    }
}
