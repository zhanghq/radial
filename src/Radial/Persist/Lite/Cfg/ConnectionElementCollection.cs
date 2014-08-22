using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Radial.Persist.Lite.Cfg
{
    /// <summary>
    /// 数据库配置元素集合
    /// </summary>
    public sealed  class ConnectionElementCollection : ConfigurationElementCollection
    {
        /// <summary>
        /// 当在派生的类中重写时，创建一个新的 <see cref="T:System.Configuration.ConfigurationElement"/>。
        /// </summary>
        /// <returns>
        /// 新的 <see cref="T:System.Configuration.ConfigurationElement"/>。
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConnectionElement();
        }

        /// <summary>
        /// 在派生类中重写时获取指定配置元素的元素键。
        /// </summary>
        /// <param name="element">要为其返回键的 <see cref="T:System.Configuration.ConfigurationElement"/>。</param>
        /// <returns>
        /// 一个 <see cref="T:System.Object"/>，用作指定 <see cref="T:System.Configuration.ConfigurationElement"/> 的键。
        /// </returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ConnectionElement)element).Name;
        }

        /// <summary>
        /// 获取索引所对应的连接设置
        /// </summary>
        ///<returns>连接设置</returns>
        public ConnectionElement this[int index]
        {
            get
            {
                return BaseGet(index) as ConnectionElement;
            }
        }

        /// <summary>
        /// 获取设置名称所对应的连接设置
        /// </summary>
        /// <param name="name">设置名称</param>
        /// <returns>连接设置</returns>
        public new ConnectionElement this[string name]
        {
            get
            {
                return BaseGet(name) as ConnectionElement;
            }
        }
    }
}
