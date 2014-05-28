using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;$endif$
using System.Text;
using System.Threading.Tasks;
using Radial.Persist;
using $safeprojectname$.Infras;

namespace $safeprojectname$.Domain
{
    /// <summary>
    /// ManagerBase
    /// </summary>
    abstract class ManagerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ManagerBase"/> class.
        /// </summary>
        /// <param name="uow">The uow.</param>
        public ManagerBase(IUnitOfWorkEssential uow)
        {
            UnitOfWork = uow;
        }

        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value>
        /// The unit of work.
        /// </value>
        protected IUnitOfWorkEssential UnitOfWork
        {
            get;
            private set;
        }

        /// <summary>
        /// Resolves the repository.
        /// </summary>
        /// <typeparam name="TRepository">The type of the repository.</typeparam>
        /// <returns></returns>
        protected TRepository ResolveRepository<TRepository>() where TRepository : class
        {
            return DependencyFactory.CreateRepository<TRepository>(UnitOfWork);
        }
    }
}
