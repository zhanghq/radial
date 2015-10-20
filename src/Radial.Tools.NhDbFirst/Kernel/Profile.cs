using System;
using System.Text;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace Radial.Tools.NhDbFirst.Kernel
{
    /// <summary>
    /// 配置文件
    /// </summary>
    [TypeConverter(typeof(PropertySorter))]
    [Serializable]
    public class Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Profile"/> class.
        /// </summary>
        public Profile()
        {
            LazyModel = true;
            NameSpliters = new char[] { '-', '_', ',', ' ' };
            OutputDirectory = "Output";
        }

        /// <summary>
        /// 获取或设置数据源类型
        /// </summary>
        [Category("数据库")]
        [Description("数据源类型")]
        [PropertyOrder(0)]
        public DataSource DataSource { get; set; }

        /// <summary>
        /// 获取或设置连接字符串
        /// </summary>
        [Category("数据库")]
        [Description("数据库连接字符串")]
        [PropertyOrder(1)]
        public string ConnectionString { get; set; }

        /// <summary>
        /// 获取或设置需要移除的表或字段名称中的分隔符
        /// </summary>
        [Category("数据库")]
        [Description("需要移除的表或字段名称中的分隔符")]
        [PropertyOrder(2)]
        public char[] NameSpliters { get; set; }

        /// <summary>
        /// 获取或设置模型类的程序集名称
        /// </summary>
        [Category("生成")]
        [Description("生成类的程序集名称")]
        [PropertyOrder(3)]
        public string ClassAssembly { get; set; }

        /// <summary>
        /// 获取或设置模型类的命名空间
        /// </summary>
        [Category("生成")]
        [Description("生成类的命名空间")]
        [PropertyOrder(4)]
        public string ClassNamespace { get; set; }

        /// <summary>
        /// 获取或设置模型类是否延迟加载(&lt;class lazy=&quot;true|false&quot;&gt;)
        /// </summary>
        [Category("生成")]
        [Description("生成类是否启用延迟加载")]
        [PropertyOrder(5)]
        public bool LazyModel { get; set; }

        /// <summary>
        /// 获取或设置输出目录
        /// </summary>
        [Category("文件")]
        [Description("生成文件的输出目录")]
        [PropertyOrder(6)]
        [EditorAttribute(typeof(FolderNameEditor), typeof(UITypeEditor))]
        public string OutputDirectory { get; set; }

        /// <summary>
        /// 获取配置文件路径
        /// </summary>
        [Category("文件")]
        [Description("配置文件保存路径")]
        [PropertyOrder(7)]
        [ReadOnly(true)]
        [XmlIgnore]
        public string FilePath { get; internal set; }

        /// <summary>
        /// Saves the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void Save(string filePath)
        {
            FilePath = filePath;
            File.WriteAllText(filePath, XmlSerializer.Serialize(this), Encoding.UTF8);
        }

        /// <summary>
        /// Loads the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static Profile Load(string filePath)
        {
            Profile obj;

            if (XmlSerializer.TryDeserialize(File.ReadAllText(filePath, Encoding.UTF8), out obj))
                obj.FilePath = filePath;

            return obj;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
                return true;

            if (obj == null || !(obj is Profile))
                return false;

            return this.GetHashCode() == obj.GetHashCode();
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return string.Format("{0}{1}{2}{3}{4}{5}{6}", DataSource, ConnectionString, string.Join<char>(",", NameSpliters), ClassAssembly, ClassNamespace, LazyModel, OutputDirectory).GetHashCode();
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns></returns>
        public Profile Clone()
        {
            using (var ms = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(ms, this);
                ms.Seek(0, 0);
                return bf.Deserialize(ms) as Profile;
            }
        }
    }
}
