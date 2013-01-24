using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Radial.Boot.Cfg
{
    /// <summary>
    /// Boot task configuration.
    /// </summary>
    public static class BootTaskCfg
    {
        /// <summary>
        /// Loads the tasks.
        /// </summary>
        /// <returns></returns>
        public static BootTaskElementCollection LoadTasks()
        {
            BootTaskSection section = ConfigurationManager.GetSection("boot") as BootTaskSection;

            if (section == null)
                return new BootTaskElementCollection();

            return section.Tasks;

        }
    }
}
