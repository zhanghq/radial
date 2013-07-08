using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Radial.Tools.NhDbFirst.Kernel
{
    /// <summary>
    /// 表示模型类映射
    /// </summary>
    public class ClassMapper : IOutput
    {
        public ClassMapper()
        {
            Properties = new List<PropertyMapper>();
        }

        /// <summary>
        /// 获取或设置模型类的程序集名称
        /// </summary>
        public string AssemblyName { get; set; }

        /// <summary>
        /// 获取或设置模型类的命名空间
        /// </summary>
        public string NamespaceName { get; set; }

        /// <summary>
        /// 获取或设置模型类名称
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 获取或设置表定义对象
        /// </summary>
        public TableDefinition TableDefinition { get; set; }

        /// <summary>
        /// 获取或设置模型类是否延迟加载(&lt;class lazy=&quot;true|false&quot;&gt;)
        /// </summary>
        public bool LazyModel { get; set; }

        public IList<PropertyMapper> Properties { get; set; }


        public DataSource DataSource
        {
            get;
            set;
        }

        public void WriteCode(TextWriter writer)
        {
            writer.WriteLine("using System;");
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using System.IO;");
            writer.WriteLine("using System.Linq;");
            writer.WriteLine("using System.Text;");

            writer.Write(writer.NewLine);

            writer.WriteLine(string.Format("namespace {0}", NamespaceName));
            writer.WriteLine("{");

            writer.WriteLine("    /// <summary>");
            writer.WriteLine(string.Format("    /// {0} class.", ClassName));
            writer.WriteLine("    /// <summary>");

            writer.WriteLine(string.Format("    public class {0}", ClassName));
            writer.WriteLine("    {");

            foreach (PropertyMapper p in Properties)
                p.WriteCode(writer);

            writer.WriteLine("    }");

            writer.WriteLine("}");
        }

        public void WriteXml(TextWriter writer)
        {
            switch (DataSource)
            {
                case DataSource.SqlServer: WriteSqlServerXml(writer); break;
                default: throw new NotSupportedException("不支持的数据源类型：" + DataSource.ToString());
            }
        }

        private void WriteSqlServerXml(TextWriter writer)
        {
            writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            writer.WriteLine(string.Format("<hibernate-mapping assembly=\"{0}\" namespace=\"{1}\" xmlns=\"urn:nhibernate-mapping-2.2\">", AssemblyName, NamespaceName));

            writer.WriteLine(string.Format("  <class name=\"{0}\" table=\"{1}\" schema=\"{2}\"{3}>", ClassName, TableDefinition.Name, TableDefinition.Schema, LazyModel ? string.Empty : " lazy=\"false\""));

            foreach (PropertyMapper p in Properties)
                p.WriteXml(writer);

            writer.WriteLine("  </class>");

            writer.WriteLine("</hibernate-mapping>");

        }


    }
}
