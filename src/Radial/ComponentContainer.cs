using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Windsor;
using Castle.MicroKernel.Registration;
using System.IO;
using System.Collections;

namespace Radial
{
    /// <summary>
    /// The container of all components, use Ioc to retrieve instance.
    /// </summary>
    public static class ComponentContainer
    {
        static IWindsorContainer S_Container;
        static object S_SyncRoot = new object();

        /// <summary>
        /// Initializes the <see cref="ComponentContainer"/> class.
        /// </summary>
        static ComponentContainer()
        {
            InitialContainer(ConfigurationPath);
            FileWatcher.CreateMonitor(ConfigurationPath, InitialContainer);
        }

        /// <summary>
        /// Gets the configuration path.
        /// </summary>
        private static string ConfigurationPath
        {
            get
            {
                return SystemVariables.GetConfigurationPath("Components.config");
            }
        }

        /// <summary>
        /// Gets a value indicating whether this configuration exists.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if  the configuration exists; otherwise, <c>false</c>.
        /// </value>
        public static bool ConfigurationExists
        {
            get
            {
                return File.Exists(ConfigurationPath);
            }
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        private static Logger Logger
        {
            get
            {
                return Logger.GetInstance("Components");
            }
        }

        /// <summary>
        /// Initials the Ioc container.
        /// </summary>
        /// <param name="configFilePath">The config file path.</param>
        private static void InitialContainer(string configFilePath)
        {
            lock (S_SyncRoot)
            {
                Logger.Debug("load component configuration");

                if (ConfigurationExists)
                    S_Container = new WindsorContainer(configFilePath);
                else
                    Logger.Warn("can not find component configuration file at {0}", configFilePath);
            }
        }


        /// <summary>
        /// Determines whether the specified service type was registered.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>
        ///   <c>true</c> if the specified service type was registered; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasComponent(Type serviceType)
        {
            lock (S_SyncRoot)
            {
                if (S_Container == null)
                    return false;
                return S_Container.Kernel.HasComponent(serviceType);
            }
        }

        /// <summary>
        /// Determines whether the specified service type was registered.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>
        ///   <c>true</c> if the specified service type was registered; otherwise, <c>false</c>.
        /// </returns>
        public static bool HasComponent<TService>()
        {
            return HasComponent(typeof(TService));
        }

        /// <summary>
        /// Releases a component instance.
        /// </summary>
        /// <param name="instance">component instance.</param>
        public static void Release(object instance)
        {
            lock (S_SyncRoot)
            {
                if (S_Container != null)
                    S_Container.Release(instance);
            }
        }

        /// <summary>
        /// Retrieve a component instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>Return a component instance, if not properly configured throw an exception</returns>
        public static TService Resolve<TService>() where TService : class
        {
            lock (S_SyncRoot)
            {
                if (S_Container == null)
                    return null;

                return S_Container.Resolve<TService>();
            }
        }

        /// <summary>
        /// Retrieve a component instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="arguments">The arguments.</param>
        /// <returns>
        /// Return a component instance, if not properly configured throw an exception
        /// </returns>
        public static TService Resolve<TService>(IDictionary arguments) where TService : class
        {
            lock (S_SyncRoot)
            {
                if (S_Container == null)
                    return null;

                return S_Container.Resolve<TService>(arguments);
            }
        }

        /// <summary>
        /// Retrieve a component instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="argumentsAsAnonymousType">argumentsAsAnonymousType.</param>
        /// <returns>
        /// Return a component instance, if not properly configured throw an exception
        /// </returns>
        public static TService Resolve<TService>(object argumentsAsAnonymousType) where TService : class
        {
            lock (S_SyncRoot)
            {
                if (S_Container == null)
                    return null;

                return S_Container.Resolve<TService>(argumentsAsAnonymousType);
            }
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
            lock (S_SyncRoot)
            {
                if (S_Container == null)
                    return null;

                return S_Container.Resolve(serviceType);
            }
        }

        /// <summary>
        /// Retrieve a component instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>
        /// Return a component instance, if not properly configured throw an exception
        /// </returns>
        public static object Resolve(Type serviceType, IDictionary arguments)
        {
            lock (S_SyncRoot)
            {
                if (S_Container == null)
                    return null;

                return S_Container.Resolve(serviceType, arguments);
            }
        }

        /// <summary>
        /// Retrieve a component instance.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="argumentsAsAnonymousType">argumentsAsAnonymousType.</param>
        /// <returns>
        /// Return a component instance, if not properly configured throw an exception
        /// </returns>
        public static object Resolve(Type serviceType, object argumentsAsAnonymousType)
        {
            lock (S_SyncRoot)
            {
                if (S_Container == null)
                    return null;

                return S_Container.Resolve(serviceType, argumentsAsAnonymousType);
            }
        }

        /// <summary>
        /// Resolve all valid components that match this service the service to match
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <returns>The Array of services to match</returns>
        public static Array ResolveAll(Type serviceType)
        {
            lock (S_SyncRoot)
            {
                if (S_Container == null)
                    return new object[] { };

                return S_Container.ResolveAll(serviceType);
            }
        }

        /// <summary>
        /// Resolve all valid components that match this service the service to match
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="arguments">The arguments.</param>
        /// <returns>
        /// The Array of services to match
        /// </returns>
        public static Array ResolveAll(Type serviceType, IDictionary arguments)
        {
            lock (S_SyncRoot)
            {
                if (S_Container == null)
                    return new object[] { };

                return S_Container.ResolveAll(serviceType, arguments);
            }
        }

        /// <summary>
        /// Resolve all valid components that match this service the service to match
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="argumentsAsAnonymousType">argumentsAsAnonymousType.</param>
        /// <returns>
        /// The Array of services to match
        /// </returns>
        public static Array ResolveAll(Type serviceType, object argumentsAsAnonymousType)
        {
            lock (S_SyncRoot)
            {
                if (S_Container == null)
                    return new object[] { };

                return S_Container.ResolveAll(serviceType, argumentsAsAnonymousType);
            }
        }

        /// <summary>
        /// Resolve all valid components that match this service the service to match
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>
        /// The Array of services to match.
        /// </returns>
        public static TService[] ResolveAll<TService>() where TService : class
        {
            lock (S_SyncRoot)
            {
                if (S_Container == null)
                    return new TService[] { };

                return S_Container.ResolveAll<TService>();
            }
        }

        /// <summary>
        /// Resolve all valid components that match this service the service to match
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="arguments">The arguments.</param>
        /// <returns>
        /// The Array of services to match.
        /// </returns>
        public static TService[] ResolveAll<TService>(IDictionary arguments) where TService : class
        {
            lock (S_SyncRoot)
            {
                if (S_Container == null)
                    return new TService[] { };

                return S_Container.ResolveAll<TService>(arguments);
            }
        }

        /// <summary>
        /// Resolve all valid components that match this service the service to match
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <param name="argumentsAsAnonymousType">argumentsAsAnonymousType.</param>
        /// <returns>
        /// The Array of services to match.
        /// </returns>
        public static TService[] ResolveAll<TService>(object argumentsAsAnonymousType) where TService : class
        {
            lock (S_SyncRoot)
            {
                if (S_Container == null)
                    return new TService[] { };

                return S_Container.ResolveAll<TService>(argumentsAsAnonymousType);
            }
        }

        /// <summary>
        /// Registers the specified service type with PerThread lifestyle.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImpl">The type of the implementation.</typeparam>
        public static void RegisterPerThread<TService, TImpl>()
            where TService : class
            where TImpl : TService
        {
            lock (S_SyncRoot)
            {
                if (S_Container != null)
                    S_Container.Register(Component.For<TService>().ImplementedBy<TImpl>().LifeStyle.PerThread);
            }
        }

        /// <summary>
        /// Registers the specified service type with PerThread lifestyle.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implType">Type of the implementation.</param>
        public static void RegisterPerThread(Type serviceType, Type implType)
        {
            lock (S_SyncRoot)
            {
                if (S_Container != null)
                    S_Container.Register(Component.For(serviceType).ImplementedBy(implType).LifeStyle.PerThread);
            }
        }


        /// <summary>
        /// Registers the specified service type with PerWebRequest lifestyle.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImpl">The type of the implementation.</typeparam>
        public static void RegisterPerWebRequest<TService, TImpl>()
            where TService : class
            where TImpl : TService
        {
            lock (S_SyncRoot)
            {
                if (S_Container != null)
                    S_Container.Register(Component.For<TService>().ImplementedBy<TImpl>().LifeStyle.PerWebRequest);
            }
        }

        /// <summary>
        /// Registers the specified service type with PerWebRequest lifestyle.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implType">Type of the implementation.</param>
        public static void RegisterPerWebRequest(Type serviceType, Type implType)
        {
            lock (S_SyncRoot)
            {
                if (S_Container != null)
                    S_Container.Register(Component.For(serviceType).ImplementedBy(implType).LifeStyle.PerWebRequest);
            }
        }


        /// <summary>
        /// Registers the specified service type with Pooled lifestyle.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImpl">The type of the implementation.</typeparam>
        public static void RegisterPooled<TService, TImpl>()
            where TService : class
            where TImpl : TService
        {
            lock (S_SyncRoot)
            {
                if (S_Container != null)
                    S_Container.Register(Component.For<TService>().ImplementedBy<TImpl>().LifeStyle.Pooled);
            }
        }

        /// <summary>
        /// Registers the specified service type with Pooled lifestyle.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implType">Type of the implementation.</param>
        public static void RegisterPooled(Type serviceType, Type implType)
        {
            lock (S_SyncRoot)
            {
                if (S_Container != null)
                    S_Container.Register(Component.For(serviceType).ImplementedBy(implType).LifeStyle.Pooled);
            }
        }

        /// <summary>
        /// Registers the specified service type with Registration lifestyle.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImpl">The type of the implementation.</typeparam>
        public static void RegisterRegistration<TService, TImpl>()
            where TService : class
            where TImpl : TService
        {
            lock (S_SyncRoot)
            {
                if (S_Container != null)
                    S_Container.Register(Component.For<TService>().ImplementedBy<TImpl>().LifeStyle.Registration);
            }
        }

        /// <summary>
        /// Registers the specified service type with Registration lifestyle.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implType">Type of the implementation.</param>
        public static void RegisterRegistration(Type serviceType, Type implType)
        {
            lock (S_SyncRoot)
            {
                if (S_Container != null)
                    S_Container.Register(Component.For(serviceType).ImplementedBy(implType).LifeStyle.Registration);
            }
        }

        /// <summary>
        /// Registers the specified service type with Singleton lifestyle.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImpl">The type of the implementation.</typeparam>
        public static void RegisterSingleton<TService, TImpl>()
            where TService : class
            where TImpl : TService
        {
            lock (S_SyncRoot)
            {
                if (S_Container != null)
                    S_Container.Register(Component.For<TService>().ImplementedBy<TImpl>().LifeStyle.Singleton);
            }
        }

        /// <summary>
        /// Registers the specified service type with Singleton lifestyle.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implType">Type of the implementation.</param>
        public static void RegisterSingleton(Type serviceType, Type implType)
        {
            lock (S_SyncRoot)
            {
                if (S_Container != null)
                    S_Container.Register(Component.For(serviceType).ImplementedBy(implType).LifeStyle.Singleton);
            }
        }

        /// <summary>
        /// Registers the specified service type with Transient lifestyle.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImpl">The type of the implementation.</typeparam>
        public static void RegisterTransient<TService, TImpl>()
            where TService : class
            where TImpl : TService
        {
            lock (S_SyncRoot)
            {
                if (S_Container != null)
                    S_Container.Register(Component.For<TService>().ImplementedBy<TImpl>().LifeStyle.Transient);
            }
        }

        /// <summary>
        /// Registers the specified service type with Transient lifestyle.
        /// </summary>
        /// <param name="serviceType">Type of the service.</param>
        /// <param name="implType">Type of the implementation.</param>
        public static void RegisterTransient(Type serviceType, Type implType)
        {
            lock (S_SyncRoot)
            {
                if (S_Container != null)
                    S_Container.Register(Component.For(serviceType).ImplementedBy(implType).LifeStyle.Transient);
            }
        }

    }
}
