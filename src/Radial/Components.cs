using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace Radial
{
    /// <summary>
    /// Components
    /// </summary>
    public static class Components
    {
        static object SyncRoot = new object();
        static IUnityContainer InnerContainer;

        /// <summary>
        /// Gets the container.
        /// </summary>
        public static IUnityContainer Container
        {
            get
            {
                lock (SyncRoot)
                {
                    if (InnerContainer == null)
                    {
                        InnerContainer = new UnityContainer();

                        UnityConfigurationSection section = (UnityConfigurationSection)ConfigurationManager.GetSection(UnityConfigurationSection.SectionName);

                        if (section != null && section.Containers.Count > 0)
                            section.Configure(InnerContainer);
                    }

                    return InnerContainer;
                }
            }
        }
    }
}
