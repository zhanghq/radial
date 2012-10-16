using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using Autofac;
using Autofac.Configuration;
using Autofac.Core;
using Autofac.Builder;
using System.Configuration;

namespace Radial
{
    /// <summary>
    /// The container of all components, use Ioc to retrieve instance.
    /// </summary>
    public static class Components
    {
        /// <summary>
        /// The additional register handler during system default initialize procedure.
        /// </summary>
        public static Action<ContainerBuilder> AdditionalRegister;

        static IContainer S_Container;
        static object S_SyncRoot = new object();

        /// <summary>
        /// Container build options.
        /// </summary>
        public static ContainerBuildOptions ContainerBuildOptions;


        /// <summary>
        /// Gets components container.
        /// </summary>
        public static IContainer Container
        {
            get
            {
                lock (S_SyncRoot)
                {
                    if (S_Container == null)
                    {
                        var builder = new ContainerBuilder();

                        object section = ConfigurationManager.GetSection(ConfigurationSettingsReader.DefaultSectionName);

                        if (section != null)
                            builder.RegisterModule(new ConfigurationSettingsReader());

                        if (AdditionalRegister != null)
                            AdditionalRegister(builder);

                        if (ContainerBuildOptions != Autofac.Builder.ContainerBuildOptions.Default)
                            S_Container = builder.Build(ContainerBuildOptions);
                        else
                            S_Container = builder.Build();
                    }

                    return S_Container;
                }
            }
        }


        /// <summary>
        /// Determines whether the specified service type was registered.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>
        ///   <c>true</c> if the specified service type was registered; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsRegistered(Type serviceType)
        {
            return Container.IsRegistered(serviceType);
        }

        /// <summary>
        /// Determines whether the specified service type was registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>
        ///   <c>true</c> if the specified service type was registered; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsRegistered<TService>()
        {
            return Container.IsRegistered<TService>();
        }

        /// <summary>
        /// Retrieve a component instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>Return a component instance, if not properly configured throw an exception</returns>
        public static TService Resolve<TService>() where TService : class
        {
            return Container.Resolve<TService>();
        }

        /// <summary>
        /// Retrieve a component instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="parameters">The parameters for the service.</param>
        /// <returns>Return a component instance, if not properly configured throw an exception</returns>
        public static TService Resolve<TService>(params Parameter[] parameters) where TService : class
        {
            return Container.Resolve<TService>(parameters);
        }

        /// <summary>
        /// Retrieve a component instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="parameters">The parameters for the service.</param>
        /// <returns>
        /// Return a component instance, if not properly configured throw an exception
        /// </returns>
        public static TService Resolve<TService>(IEnumerable<Parameter> parameters) where TService : class
        {
            return Container.Resolve<TService>(parameters);
        }

        /// <summary>
        /// Retrieve a component instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="parameters">The parameters for the service.</param>
        /// <returns>
        /// Return a component instance, if not properly configured throw an exception
        /// </returns>
        public static TService Resolve<TService>(IDictionary<string, object> parameters) where TService : class
        {
            if (parameters == null)
                return Container.Resolve<TService>();

            IList<NamedParameter> nps = new List<NamedParameter>();

            foreach (KeyValuePair<string, object> item in parameters)
                nps.Add(new NamedParameter(item.Key, item.Value));

            return Container.Resolve<TService>(nps);
        }

        /// <summary>
        /// Retrieve a component instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>
        /// Return a component instance, if not properly configured throw an exception
        /// </returns>
        public static object Resolve(Type serviceType)
        {
            return Container.Resolve(serviceType);
        }

        /// <summary>
        /// Retrieve a component instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters for the service.</param>
        /// <returns>
        /// Return a component instance, if not properly configured throw an exception
        /// </returns>
        public static object Resolve(Type serviceType, params Parameter[] parameters)
        {
            return Container.Resolve(serviceType, parameters);
        }

        /// <summary>
        /// Retrieve a component instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters for the service.</param>
        /// <returns>
        /// Return a component instance, if not properly configured throw an exception
        /// </returns>
        public static object Resolve(Type serviceType, IEnumerable<Parameter> parameters)
        {
            return Container.Resolve(serviceType, parameters);
        }

        /// <summary>
        /// Retrieve a component instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="parameters">The parameters for the service.</param>
        /// <returns>
        /// Return a component instance, if not properly configured throw an exception
        /// </returns>
        public static object Resolve(Type serviceType, IDictionary<string, object> parameters)
        {
            if (parameters == null)
                return Container.Resolve(serviceType);

            IList<NamedParameter> nps = new List<NamedParameter>();

            foreach (KeyValuePair<string, object> item in parameters)
                nps.Add(new NamedParameter(item.Key, item.Value));

            return Container.Resolve(serviceType, nps);
        }
    }
}
