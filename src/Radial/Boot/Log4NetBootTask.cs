using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Radial.Logging;

namespace Radial.Boot
{
    /// <summary>
    /// Log4NetBootTask
    /// </summary>
    /// <seealso cref="Radial.Boot.IBootTask" />
    public class Log4NetBootTask : IBootTask
    {
        /// <summary>
        /// System initialize process.
        /// </summary>
        public void Initialize(NameValueCollection args)
        {
            //file relative path.
            string configPath = args != null ? args["configPath"] : null;

            if (!string.IsNullOrWhiteSpace(configPath))
                configPath = GlobalVariables.GetPath(configPath);

            if (!Dependency.Container.IsRegistered<LogWriter>())
            {
                Log4NetWriter.Prepare(configPath);

                Dependency.Container.RegisterType<LogWriter, Log4NetWriter>();
            }
        }

        /// <summary>
        /// Start system.
        /// </summary>
        public void Start()
        {
        }
        /// <summary>
        /// Stop system.
        /// </summary>
        public void Stop()
        {
        }
    }
}
