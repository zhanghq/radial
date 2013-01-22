using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Radial.Persist.Lite
{
    /// <summary>
    /// 数据库设置集合
    /// </summary>
    public sealed  class ConnectionSettingsCollection : ConfigurationElementCollection
    {

        /// <summary>
        /// 初始化数据库设置集合
        /// </summary>
        public ConnectionSettingsCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        /// <summary>
        /// 当在派生的类中重写时，创建一个新的 <see cref="T:System.Configuration.ConfigurationElement"/>。
        /// </summary>
        /// <returns>
        /// 新的 <see cref="T:System.Configuration.ConfigurationElement"/>。
        /// </returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConnectionSettings();
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
            return ((ConnectionSettings)element).Name;
        }

        /// <summary>
        /// 向 <see cref="T:System.Configuration.ConfigurationElementCollection"/> 添加配置元素。
        /// </summary>
        /// <param name="element">要添加的 <see cref="T:System.Configuration.ConfigurationElement"/>。</param>
        protected override void BaseAdd(ConfigurationElement element)
        {
            ValidateSettingsName(element);
            base.BaseAdd(element);
        }

        /// <summary>
        /// 向配置元素集合添加配置元素。
        /// </summary>
        /// <param name="index">要添加指定 <see cref="T:System.Configuration.ConfigurationElement"/> 的索引位置。</param>
        /// <param name="element">要添加的 <see cref="T:System.Configuration.ConfigurationElement"/>。</param>
        protected override void BaseAdd(int index, ConfigurationElement element)
        {
            ValidateSettingsName(element);
            base.BaseAdd(index, element);
        }

        /// <summary>
        /// 验证数据库配置名称
        /// </summary>
        /// <param name="element">配置元素</param>
        private void ValidateSettingsName(ConfigurationElement element)
        {
            object[] keys = this.BaseGetAllKeys();
            string elementKey = ((string)GetElementKey(element)).Trim();

            foreach (object key in keys)
            {
                string keyString = ((string)key).Trim();

                if (elementKey == keyString)
                    throw new ConfigurationErrorsException("数据库设置名称" + elementKey + "已存在");
            }
        }

        /// <summary>
        /// 获取索引所对应的连接设置
        /// </summary>
        ///<returns>连接设置</returns>
        public ConnectionSettings this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentException("设置索引不能小于0", "index");
                return BaseGet(index) as ConnectionSettings;
            }
        }

        /// <summary>
        /// 获取设置名称所对应的连接设置
        /// </summary>
        /// <param name="name">设置名称</param>
        /// <returns>连接设置</returns>
        public new ConnectionSettings this[string name]
        {
            get
            {
                if (string.IsNullOrEmpty(name))
                    throw new ArgumentException("设置名称不能为空", "name");

                name = name.Trim().ToLower();
                return BaseGet(name) as ConnectionSettings;
            }
        }
    }
}
