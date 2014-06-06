using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Modules.UserUnit
{
    /// <summary>
    /// Preparation.
    /// </summary>
    public static class Preparation
    {
        /// <summary>
        /// Adds assembly to NHibernate.Cfg.Configuration.
        /// </summary>
        /// <param name="cfg">The CFG.</param>
        public static void AddAssembly(Configuration cfg)
        {
            Checker.Parameter(cfg != null, "NHibernate.Cfg.Configuration instance can not be null");

            cfg.AddAssembly(typeof(Preparation).Assembly);
        }
    }
}
