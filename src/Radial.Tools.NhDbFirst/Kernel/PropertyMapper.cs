using System;
using System.Collections.Generic;
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


        public void WriteCode(System.IO.TextWriter writer)
        {
            writer.WriteLine("        /// <summary>");
            writer.WriteLine(string.Format("        /// Gets or sets {0}.", PropertyName));
            writer.WriteLine("        /// <summary>");

            writer.WriteLine(string.Format("        public{0}{1} {2} ", ClassMapper.LazyModel ? " virtual " : " ", TypeString, PropertyName) + "{ get; set; }");

            writer.Write(writer.NewLine);
        }

        public void WriteXml(System.IO.TextWriter writer)
        {
            if (FieldDefinition.IsPrimaryKey)
            {
                writer.WriteLine(string.Format("    <id name=\"{0}\"{1}>", PropertyName, TypeString == "short" || TypeString == "int" || TypeString == "long" || TypeString == "float" || TypeString == "double" || TypeString == "decimal" ? " unsaved-value=\"0\"" : string.Empty));
                writer.WriteLine(string.Format("      <column name=\"{0}\" sql-type=\"{1}\"{2}/>", FieldDefinition.Name, FieldDefinition.SqlType, FieldDefinition.Length > 0 ? " length=\"" + FieldDefinition.Length + "\"" : string.Empty));
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
                writer.WriteLine(string.Format("      <column name=\"{0}\" sql-type=\"{1}\"{2}{3}/>", FieldDefinition.Name, FieldDefinition.SqlType, FieldDefinition.Length > 0 ? " length=\"" + FieldDefinition.Length + "\"" : string.Empty, FieldDefinition.IsNullable ? string.Empty : " not-null=\"true\""));
                writer.WriteLine("    </property>");
            }
        }
    }
}
