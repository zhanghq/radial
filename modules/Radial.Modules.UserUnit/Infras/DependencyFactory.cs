using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Radial;
using Radial.Persist;
using Microsoft.Practices.Unity;


namespace Radial.Modules.UserUnit.Infras
{
    /// <summary>
    /// Dependency factory.
    /// </summary>
    static class DependencyFactory
    {
        /// <summary>
        /// Create a new instance of IUnitOfWork.
        /// </summary>
        /// <returns>IUnitOfWork instance.</returns>
        public static IUnitOfWork CreateUnitOfWork()
        {
            return Components.Container.Resolve<IUnitOfWork>();
        }

        /// <summary>
        /// Create a new instance of TRepository.
        /// </summary>
        /// <typeparam name="TRepository">The type of repository.</typeparam>
        /// <param name="uow">The IUnitOfWork instance.</param>
        /// <returns>
        /// TRepository instance.
        /// </returns>
        public static TRepository CreateRepository<TRepository>(IUnitOfWorkEssential uow) where TRepository : class
        {
            return Components.Container.Resolve<TRepository>(new ParameterOverride("uow", uow));
        }

        /// <summary>
        /// Creates the service.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns></returns>
        public static TService CreateService<TService>() where TService : class
        {
            return Components.Container.Resolve<TService>();
        }
    }
}
