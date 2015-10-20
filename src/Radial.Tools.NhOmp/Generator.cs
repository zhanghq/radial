using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Radial.Tools.NhOmp
{
    class Generator
    {
        public Generator(Assembly classAssembly, Type classType)
        {
            ClassAssembly = classAssembly;
            ClassType = classType;
        }

        Assembly ClassAssembly { get; set; }
        Type ClassType { get; set; }

        public string BuildHbm()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sb.AppendLine(string.Format("<hibernate-mapping xmlns=\"urn:nhibernate-mapping-2.2\" assembly=\"{0}\" namespace=\"{1}\">",
                ClassAssembly.GetName().Name, ClassType.Namespace));
            sb.AppendLine(string.Format("  <class name=\"{0}\">", ClassType.Name));

            List<string> ps = new List<string>();

            foreach (var p in ClassType.GetProperties())
            {
                if (string.Compare(p.Name, "Id", false) == 0)
                {
                    ps.Add(string.Format("    <id name=\"{0}\"/>", p.Name));
                    continue;
                }
                ps.Add(string.Format("    <property name=\"{0}\"/>", p.Name));
            }

            ps.Sort();

            ps.ForEach(o => sb.AppendLine(o));

            sb.AppendLine("  </class>");

            sb.AppendLine("</hibernate-mapping>");

            return sb.ToString();
        }
    }
}
