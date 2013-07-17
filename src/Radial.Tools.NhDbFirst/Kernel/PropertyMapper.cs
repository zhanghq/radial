using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Radial.Tools.NhDbFirst.Kernel
{
    /// <summary>
    /// 表示模型类的属性映射
    /// </summary>
    public class PropertyMapper : IOutput
    {
        public ClassMapper ClassMapper { get; set; }
        public virtual string TypeString { get; set; }
        public string PropertyName { get; set; }
        public FieldDefinition FieldDefinition { get; set; }

        public DataSource DataSource
        {
            get;
            set;
        }


        public void WriteCode(System.IO.TextWriter writer)
        {
            writer.WriteLine("        /// <summary>");
            writer.WriteLine(string.Format("        /// Gets or sets {0}.", PropertyName));
            writer.WriteLine("        /// </summary>");

            writer.WriteLine(string.Format("        public{0}{1} {2} ", ClassMapper.LazyModel ? " virtual " : " ", TypeString, PropertyName) + "{ get; set; }");

            writer.Write(writer.NewLine);
        }

        public void WriteXml(TextWriter writer)
        {
            switch (DataSource)
            {
                case DataSource.SqlServer: WriteSqlServerXml(writer); break;
                default: throw new NotSupportedException("不支持的数据源类型：" + DataSource.ToString());
            }
        }

        private string BuildSqlServerSqlType(FieldDefinition field)
        {
            string type = FieldDefinition.SqlType;

            type = type.ToLower().Trim();

            if (type == "decimal" || type == "numeric")
                return string.Format("{0}({1},{2})", type, field.Length, field.Scale ?? 0);

            if (type == "datetime2" || type == "datetimeoffset" || type == "time")
                return string.Format("{0}({1})", type, field.Length);

            if (type.Contains("char") || type.Contains("binary"))
            {
                if (field.Length == -1)
                    return type + "(MAX)";

                return string.Format("{0}({1})", type, field.Length);
            }

            return type;
        }

        private void WriteSqlServerXml(System.IO.TextWriter writer)
        {
            if (FieldDefinition.IsPrimaryKey)
            {
                writer.WriteLine(string.Format("    <id name=\"{0}\"{1}>", PropertyName, TypeString == "short" || TypeString == "int" || TypeString == "long" || TypeString == "float" || TypeString == "double" || TypeString == "decimal" ? " unsaved-value=\"0\"" : string.Empty));
                writer.WriteLine(string.Format("      <column name=\"{0}\" sql-type=\"{1}\"/>", FieldDefinition.Name, BuildSqlServerSqlType(FieldDefinition)));
                if (FieldDefinition.IsIdentity)
                    writer.WriteLine("      <generator class=\"native\"/>");
                else if (FieldDefinition.IsRowGuid)
                    writer.WriteLine("      <generator class=\"guid.comb\"/>");
                else
                    writer.WriteLine("      <generator class=\"assigned\"/>");

                writer.WriteLine("    </id>");
            }
            else
            {
                writer.WriteLine(string.Format("    <property name=\"{0}\">", PropertyName));
                writer.WriteLine(string.Format("      <column name=\"{0}\" sql-type=\"{1}\"{2}/>", FieldDefinition.Name, BuildSqlServerSqlType(FieldDefinition), FieldDefinition.IsNullable ? string.Empty : " not-null=\"true\""));
                writer.WriteLine("    </property>");
            }
        }
    }
}
