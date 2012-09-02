using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Reflection;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// Castle.Windsor dependency resolver
    /// </summary>
    public sealed class WindsorDependencyResolver : IDependencyResolver
    {
        #region IDependencyResolver Members

        /// <summary>
        /// Resolves singly registered services that support arbitrary object creation.
        /// </summary>
        /// <param name="serviceType">The type of the requested service or object.</param>
        /// <returns>
        /// The requested service or object.
        /// </returns>
        public object GetService(Type serviceType)
        {
            if (Components.IsRegistered(serviceType))
                return Components.Resolve(serviceType);
            return null;
        }

        /// <summary>
        /// Resolves multiply registered services.
        /// </summary>
        /// <param name="serviceType">The type of the requested services.</param>
        /// <returns>
        /// The requested services.
        /// </returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (Components.IsRegistered(serviceType))
                return Components.ResolveAll(serviceType).Cast<object>();

            return new object[] { };
        }

        #endregion

        /// <summary>
        /// Register EasyPortal.Web.Mvc.WindsorDependencyResolver instance to dependency resolvers.
        /// </summary>
        /// <param name="controllerAssemblyName">The controller assembly name.</param>
        public static void Register(string controllerAssemblyName)
        {
            Register<WindsorDefaultControllerFactory>(controllerAssemblyName);
        }

        /// <summary>
        /// Register EasyPortal.Web.Mvc.WindsorDependencyResolver instance to dependency resolvers.
        /// </summary>
        /// <param name="controllerAssembly">The controller assembly.</param>
        public static void Register(Assembly controllerAssembly)
        {
            Register<WindsorDefaultControllerFactory>(controllerAssembly);
        }

        /// <summary>
        /// Register IControllerFactory instance to dependency resolvers.
        /// </summary>
        /// <typeparam name="TFactory">The type of the controller factory.</typeparam>
        /// <param name="controllerAssemblyName">The controller assembly name.</param>
        public static void Register<TFactory>(string controllerAssemblyName) where TFactory : IControllerFactory
        {
            Register<TFactory>(new string[] { controllerAssemblyName });
        }

        /// <summary>
        /// Register IControllerFactory instance to dependency resolvers.
        /// </summary>
        /// <typeparam name="TFactory">The type of the controller factory.</typeparam>
        /// <param name="controllerAssembly">The controller assembly.</param>
        public static void Register<TFactory>(Assembly controllerAssembly) where TFactory : IControllerFactory
        {
            Register<TFactory>(new Assembly[] { controllerAssembly });
        }

        /// <summary>
        /// Register EasyPortal.Web.Mvc.WindsorDependencyResolver instance to dependency resolvers.
        /// </summary>
        /// <param name="controllerAssemblyNames">The array of controller assembly name.</param>
        public static void Register(string[] controllerAssemblyNames)
        {
            Register<WindsorDefaultControllerFactory>(controllerAssemblyNames);
        }

        /// <summary>
        /// Register EasyPortal.Web.Mvc.WindsorDependencyResolver instance to dependency resolvers.
        /// </summary>
        /// <param name="controllerAssemblies">The array of controller assembly.</param>
        public static void Register(Assembly[] controllerAssemblies)
        {
            Register<WindsorDefaultControllerFactory>(controllerAssemblies);
        }

        /// <summary>
        /// Register IControllerFactory instance to dependency resolvers.
        /// </summary>
        /// <typeparam name="TFactory">The type of the controller factory.</typeparam>
        /// <param name="controllerAssemblyNames">The array of controller assembly name.</param>
        public static void Register<TFactory>(string[] controllerAssemblyNames) where TFactory : IControllerFactory
        {
            Checker.Parameter(controllerAssemblyNames != null && controllerAssemblyNames.Length > 0, "the array of controller assembly name can not be null or empty");

            IList<Assembly> assemblies = new List<Assembly>();
            foreach (string assemblyName in controllerAssemblyNames)
            {
                Checker.Requires(!string.IsNullOrWhiteSpace(assemblyName), "controller assembly name can not be null or empty");
                assemblies.Add(Assembly.Load(new AssemblyName(assemblyName)));
            }

            Register<TFactory>(assemblies.ToArray());
        }

        /// <summary>
        /// Register IControllerFactory instance to dependency resolvers.
        /// </summary>
        /// <typeparam name="TFactory">The type of the controller factory.</typeparam>
        /// <param name="controllerAssemblies">The array of controller assembly.</param>
        public static void Register<TFactory>(Assembly[] controllerAssemblies) where TFactory : IControllerFactory
        {
            Checker.Parameter(controllerAssemblies != null && controllerAssemblies.Length > 0, "the array of controller assembly can not be null or empty");

            Components.RegisterSingleton<IControllerFactory, TFactory>();

            foreach (Assembly assembly in controllerAssemblies)
            {
                Checker.Requires(assembly != null, "controller assembly can not be null");

                Type[] allTypes = assembly.GetTypes();

                foreach (Type type in allTypes)
                {
                    if (type.IsSubclassOf(typeof(System.Web.Mvc.Controller)))
                    {
                        //PerWebRequest lifestyle need httpModules configuration:
                        //<add name="PerRequestLifestyle" type="Castle.MicroKernel.Lifestyle.PerWebRequestLifestyleModule, Castle.Windsor" />
                        Components.RegisterPerWebRequest(type, type);
                    }

                }
            }

            DependencyResolver.SetResolver(new WindsorDependencyResolver());
        }
    }
}
