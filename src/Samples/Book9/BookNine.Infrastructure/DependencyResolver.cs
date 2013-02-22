using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Radial;
using Radial.Persist;

namespace BookNine.Infrastructure
{
    public static class DependencyResolver
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
    }
}
