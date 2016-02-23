using Microsoft.Practices.Unity;

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

        /// <summary>
        /// Resolves the repository.
        /// </summary>
        /// <typeparam name="TRepository">The type of the repository.</typeparam>
        /// <param name="container">The container.</param>
        /// <param name="uow">The unit of work.</param>
        /// <returns></returns>
        public static TRepository ResolveRepository<TRepository>(this IUnityContainer container, IUnitOfWorkEssential uow)
            where TRepository : class
        {
            if (container == null)
                return null;

            return container.Resolve<TRepository>(new ParameterOverride("uow", uow));
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
        {
            if (uow == null)
                return default(TRepository);

            return Dependency.Container.Resolve<TRepository>(new ParameterOverride("uow", uow));
        }

        #endregion
    }
}
