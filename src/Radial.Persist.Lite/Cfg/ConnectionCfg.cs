using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Radial.Persist.Lite.Cfg
{
    /// <summary>
    /// 数据库配置类
    /// </summary>
    public sealed  class ConnectionCfg
    {
        /// <summary>
        /// 读取配置节点
        /// </summary>
        /// <returns>数据库设置组节点对象</returns>
        public static ConnectionSection Read()
        {
            ConnectionSection section = ConfigurationManager.GetSection("connGroup") as ConnectionSection;

            if (section == null)
                throw new ArgumentException("无法获取数据库连接设置节点");

            return section;
        }
    }
}
