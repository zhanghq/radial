using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Text;

namespace Radial.Persist
{
    /// <summary>
    /// The class contains extension methods of Radial.Persist.
    /// </summary>
    public static class ExtensionMethods
    {
        #region IUnityContainer Extension Methods

        /// <summary>
        /// Resolves the unit of work.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="storageAlias">The storage alias.</param>
        /// <returns></returns>
        public static IUnitOfWork ResolveUnitOfWork(this IUnityContainer container, string storageAlias = null)
        {
            if (container == null)
                return null;

            if (string.IsNullOrWhiteSpace(storageAlias))
                storageAlias = string.Empty;

            return container.Resolve<IUnitOfWork>(new ParameterOverride("storageAlias", storageAlias));
        }


        #endregion


        #region  IUnitOfWorkEssential

        /// <summary>
        /// Resolves the repository.
        /// </summary>
        /// <typeparam name="TRepository">The type of the repository.</typeparam>
        /// <param name="uow">The unit of work.</param>
        /// <returns></returns>
        public static TRepository ResolveRepository<TRepository>(this IUnitOfWorkEssential uow)
            where TRepository : class
        {
            if (uow == null)
                return default(TRepository);

            if (!Dependency.Container.IsRegistered<TRepository>())
            {
                Logger.Default.Debug("no instance of {0} type has been registered, try to create anonymous repository", typeof(TRepository).FullName);

                AnonymousRepository <TRepository> drepo = new AnonymousRepository<TRepository>(uow);

                TRepository instance = drepo.GetInstance();

                Checker.Requires(instance != null, "no instance of {0} type has been registered, and failed to create anonymous repository", typeof(TRepository).FullName);


                Dependency.Container.RegisterInstance<TRepository>(instance);

                return instance;
            }

            return Dependency.Container.Resolve<TRepository>(new ParameterOverride("uow", uow));
        }

        #endregion
    }
}
