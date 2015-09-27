using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Radial.Extensions;

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


        #region IUnityContainer Extension Methods

        /// <summary>
        /// Register all interface implementations, with RegisterInterface attribute.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="implAssembly">The implementation assembly.</param>
        /// <param name="symbol">The symbol</param>
        /// <param name="lifetimeManagerFunc">The lifetime manager function.</param>
        /// <param name="injectionMembers">The injection members.</param>
        public static void RegisterInterfaces(this IUnityContainer container, Assembly implAssembly, string symbol = null,
            Func<LifetimeManager> lifetimeManagerFunc = null, params InjectionMember[] injectionMembers)
        {
            var intyps = implAssembly.GetTypes().Where(o =>
            o.CustomAttributes.Contains(x => x.AttributeType == typeof(RegisterInterfaceAttribute))).ToArray();
            RegisterInterfaces(container, intyps, symbol, lifetimeManagerFunc, injectionMembers);

        }

        /// <summary>
        /// Register all interface implementations, with RegisterInterface attribute.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="implTypes">The implementation types.</param>
        /// <param name="symbol">The symbol</param>
        /// <param name="lifetimeManagerFunc">The lifetime manager function.</param>
        /// <param name="injectionMembers">The injection members.</param>
        public static void RegisterInterfaces(this IUnityContainer container, Type[] implTypes, string symbol = null,
            Func<LifetimeManager> lifetimeManagerFunc = null, params InjectionMember[] injectionMembers)
        {
            if (container == null || implTypes == null)
                return;

            foreach (var type in implTypes)
            {
                var attrObjs = type.GetCustomAttributes(typeof(RegisterInterfaceAttribute), false);

                if (attrObjs == null)
                    continue;

                foreach (var attrObj in attrObjs)
                {
                    var attr = attrObj as RegisterInterfaceAttribute;

                    if (attr == null)
                        continue;

                    LifetimeManager fm = null;
                    if (lifetimeManagerFunc != null)
                        fm = lifetimeManagerFunc();

                    if (!string.IsNullOrWhiteSpace(attr.Symbol))
                    {
                        if (attr.Symbol == symbol)
                            container.RegisterType(attr.InterfaceType, type, fm, injectionMembers);
                    }
                    else
                        container.RegisterType(attr.InterfaceType, type, fm, injectionMembers);
                }
            }
        }

        #endregion
    }
}
