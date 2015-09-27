using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Radial
{
    /// <summary>
    /// The entrance class of the dependency injection.
    /// </summary>
    public static class Dependency
    {
        static object SyncRoot = new object();
        static IUnityContainer InnerContainer;

        /// <summary>
        /// Gets the dependency injection container.
        /// </summary>
        public static IUnityContainer Container
        {
            get
            {
                if (InnerContainer == null)
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
                    }
                }

                return InnerContainer;
            }
        }
    }
}
