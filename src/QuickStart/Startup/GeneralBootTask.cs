using Microsoft.Practices.Unity;
using QuickStart.Application;
using QuickStart.Application.Impl;
using Radial;
using Radial.Boot;
using Radial.Param;
using Radial.Persist;
using Radial.Persist.Nhs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickStart.Startup
{
    /// <summary>
    /// GeneralBootTask
    /// </summary>
    public abstract class GeneralBootTask : IBootTask
    {
        /// <summary>
        /// System initialize process.
        /// </summary>
        public void Initialize()
        {
            Components.Container.RegisterType<IUnitOfWork, ContextualUnitOfWork>();
            
            InitializePoolInitializer();
            
            InitializeRepositories();

            InitializeServices();

            InitializeOthers();
        }

        /// <summary>
        /// Registers the pool initializer.
        /// </summary>
        protected abstract void InitializePoolInitializer();

        /// <summary>
        /// Registers the repositories.
        /// </summary>
        protected abstract void InitializeRepositories();

        /// <summary>
        /// Registers the services.
        /// </summary>
        protected virtual void InitializeServices()
        {
            //register services
            Components.Container.RegisterType<IUserService, UserService>();
        }

        /// <summary>
        /// Initializes the others.
        /// </summary>
        protected virtual void InitializeOthers() { }

        /// <summary>
        /// Start system.
        /// </summary>
        public virtual void Start()
        {
            HibernateEngine.OpenAndBindSession();
        }

        /// <summary>
        /// Stop system.
        /// </summary>
        public virtual void Stop()
        {
            HibernateEngine.CloseAndUnbindSession();
        }
    }
}
