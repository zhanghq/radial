using Radial.Persist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickStart.Infras;

namespace QuickStart.Application.Impl
{
    /// <summary>
    /// ServiceBase
    /// </summary>
    abstract class ServiceBase
    {
        /// <summary>
        /// Resolves the unit of work.
        /// </summary>
        /// <returns></returns>
        protected IUnitOfWork ResolveUnitOfWork()
        {
            return DependencyFactory.CreateUnitOfWork();
        }

        /// <summary>
        /// Resolves the repository.
        /// </summary>
        /// <typeparam name="TRepository">The type of the repository.</typeparam>
        /// <param name="uow">The uow.</param>
        /// <returns></returns>
        protected TRepository ResolveRepository<TRepository>(IUnitOfWorkEssential uow) where TRepository : class
        {
            return DependencyFactory.CreateRepository<TRepository>(uow);
        }
    }
}
