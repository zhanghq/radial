﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            sb.AppendLine(string.Format("  <class name=\"{0}\">",ClassType.Name));

            foreach(var p in ClassType.GetProperties())
            {
                if (string.Compare(p.Name, "Id", false) == 0)
                {
                    sb.AppendLine(string.Format("    <id name=\"{0}\"/>", p.Name));
                    continue;
                }
                sb.AppendLine(string.Format("    <property name=\"{0}\"/>", p.Name));
            }

            sb.AppendLine("  </class>");

            sb.AppendLine("</hibernate-mapping>");

            return sb.ToString();
        }
    }
}
