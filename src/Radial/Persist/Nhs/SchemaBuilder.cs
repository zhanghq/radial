using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// Schema builder.
    /// </summary>
    public sealed class SchemaBuilder
    {
        Configuration _cfg = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchemaBuilder"/> class.
        /// </summary>
        /// <param name="storageAlias">The storage alias (case insensitive).</param>
        public SchemaBuilder(string storageAlias = null)
        {
            if (string.IsNullOrWhiteSpace(storageAlias))
            {
                var cwp = SessionFactoryPool.GetConfigurationWrappers().FirstOrDefault();

                Checker.Requires(cwp != null, "can not find the any ConfigurationWrapper instance without storage alias");

                _cfg = cwp.Configuration;
            }
            else
                _cfg = SessionFactoryPool.GetConfigurationWrapper(storageAlias).Configuration;
        }

        /// <summary>
        /// Determines whether to use standard output for scripts.
        /// </summary>
        public bool UseStdOut
        {
            get; set;
        }


        /// <summary>
        /// Executes create schema script against the database.
        /// </summary>
        public void Create()
        {
            SchemaExport ce = new SchemaExport(_cfg);
            ce.Execute(UseStdOut, true, false);
        }

        /// <summary>
        /// Executes drop schema script against the database.
        /// </summary>
        public void Drop()
        {
            SchemaExport ce = new SchemaExport(_cfg);
            ce.Drop(UseStdOut, true);
        }

        /// <summary>
        /// Validates the database schema.
        /// </summary>
        public void Validate()
        {
            SchemaValidator sv = new SchemaValidator(_cfg);
            sv.Validate();
        }

        /// <summary>
        /// Executes update schema script against the database.
        /// </summary>
        public void Update()
        {
            SchemaUpdate su = new SchemaUpdate(_cfg);
            su.Execute(UseStdOut, true);
        }

        /// <summary>
        /// Exports create schema script.
        /// </summary>
        /// <returns></returns>
        public string Export()
        {
            StringBuilder sb = new StringBuilder();
            SchemaExport ce = new SchemaExport(_cfg);
            ce.Execute(o => sb.AppendLine(o), false, false);

            return sb.ToString();
        }
    }
}
