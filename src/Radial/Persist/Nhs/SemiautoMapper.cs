using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// Semiautomatic mapper.
    /// </summary>
    public class SemiautoMapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SemiautoMapper" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public SemiautoMapper(Configuration configuration) : this(configuration, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SemiautoMapper" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="basicDirectory">The basic mapping file directory.</param>
        public SemiautoMapper(Configuration configuration, string basicDirectory)
        {
            Checker.Parameter(configuration != null, "NHibernate.Cfg.Configuration instance can not be null");

            Configuration = configuration;

            if (string.IsNullOrWhiteSpace(basicDirectory))
                BasicDirectory = GlobalVariables.GetConfigPath("Mapping");

        }

        /// <summary>
        /// Gets or sets NHibernate.Cfg.Configuration
        /// </summary>
        private Configuration Configuration
        {
            get; set;
        }

        /// <summary>
        /// Gets the basic mapping file directory.
        /// </summary>
        public string BasicDirectory
        {
            get;
            private set;
        }

        /// <summary>
        /// Builds the class mapping file path.
        /// </summary>
        /// <param name="classType">Type of the class.</param>
        /// <returns></returns>
        private string BuildClassMappingFilePath(Type classType)
        {
            if (!Directory.Exists(BasicDirectory))
                Directory.CreateDirectory(BasicDirectory);

            string assemblyDirectory = Path.Combine(BasicDirectory, classType.Assembly.GetName().Name);

            if (!Directory.Exists(assemblyDirectory))
                Directory.CreateDirectory(assemblyDirectory);

            var namespaceDirectory = Path.Combine(assemblyDirectory, classType.Namespace);

            if (!Directory.Exists(namespaceDirectory))
                Directory.CreateDirectory(namespaceDirectory);

            return Path.Combine(namespaceDirectory, string.Format("{0}.hbm.config", classType.Name));
        }

        /// <summary>
        /// Creates the class HBM string.
        /// </summary>
        /// <param name="classType">Type of the class.</param>
        /// <returns></returns>
        private string CreateClassHbmString(Type classType)
        {
            //create hbm
            StringBuilder hbmBuilder = new StringBuilder();
            hbmBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            hbmBuilder.AppendLine(string.Format("<hibernate-mapping xmlns=\"urn:nhibernate-mapping-2.2\" assembly=\"{0}\" namespace=\"{1}\">",
                classType.Assembly.GetName().Name, classType.Namespace));
            hbmBuilder.AppendLine(string.Format("  <class name=\"{0}\">", classType.Name));


            var properties = classType.GetProperties();


            var spp = properties.SingleOrDefault(p => string.Compare(p.Name, "Id", false) == 0);

            if (spp != null)
            {
                hbmBuilder.AppendLine(string.Format("    <id name=\"{0}\">", spp.Name));
                hbmBuilder.AppendLine("      <generator class=\"assigned\"/>");
                hbmBuilder.AppendLine("    </id>");
            }
            spp = properties.SingleOrDefault(p => string.Compare(p.Name, "Version", false) == 0);
            if (spp != null)
                hbmBuilder.AppendLine(string.Format("    <version name =\"{0}\" type =\"Int32\" unsaved-value=\"0\" />", spp.Name));

            List<string> ps = new List<string>();

            foreach (var p in properties)
            {
                if (string.Compare(p.Name, "Id", false) == 0)
                    continue;
                if (string.Compare(p.Name, "Version", false) == 0)
                    continue;

                ps.Add(string.Format("    <property name=\"{0}\"/>", p.Name));
            }

            ps.Sort();

            ps.ForEach(o => hbmBuilder.AppendLine(o));

            hbmBuilder.AppendLine("  </class>");
            hbmBuilder.AppendLine("</hibernate-mapping>");
            return hbmBuilder.ToString();
        }

        /// <summary>
        /// Adds the specified class type to mapper.
        /// </summary>
        /// <param name="classType">Type of the class.</param>
        public void Add(Type classType)
        {
            Checker.Parameter(classType != null, "class type can not be null");

            //check file 
            var cfp = BuildClassMappingFilePath(classType);

            //file not exist
            if (!File.Exists(cfp))
                File.WriteAllText(cfp, CreateClassHbmString(classType));

            //add to configuration
            Configuration.AddXmlFile(cfp);
        }

        /// <summary>
        /// Adds the specified class type to mapper.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        public void Add<TClass>() where TClass : class
        {
            Add(typeof(TClass));
        }

        /// <summary>
        /// Adds the specified class types to mapper.
        /// </summary>
        /// <param name="classTypes">The class types.</param>
        public void Add(Type[] classTypes)
        {
            if (classTypes == null)
                return;

            foreach (var c in classTypes)
                Add(c);

        }
        /// <summary>
        /// Adds class types to mapper in the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="filter">The class type filter.</param>
        public void Add(System.Reflection.Assembly assembly, Func<Type, bool> filter)
        {
            Add(new System.Reflection.Assembly[] { assembly }, filter);
        }

        /// <summary>
        /// Adds class types to mapper in the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="filter">The class type filter.</param>
        public void Add(System.Reflection.Assembly[] assemblies, Func<Type, bool> filter)
        {
            if (assemblies == null || filter == null)
                return;

            foreach (var a in assemblies)
            {
                foreach (var c in a.ExportedTypes.Where(filter))
                    Add(c);
            }
        }

    }
}
