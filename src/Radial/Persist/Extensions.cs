﻿using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Persist
{
    /// <summary>
    /// The class contains extension methods of Radial.Persist.
    /// </summary>
    public static class Extensions
    {
        #region IUnityContainer Extension Methods

        /// <summary>
        /// Resolves the unit of work.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        public static IUnitOfWork ResolveUnitOfWork(this IUnityContainer container)
        {
            if (container == null)
                return null;

            return container.Resolve<IUnitOfWork>();
        }

        /// <summary>
        /// Resolves the unit of work.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public static IUnitOfWork ResolveUnitOfWork(this IUnityContainer container, string alias)
        {
            if (container == null)
                return null;

            return container.Resolve<IUnitOfWork>(new ParameterOverride("alias", alias));
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
    }
}
