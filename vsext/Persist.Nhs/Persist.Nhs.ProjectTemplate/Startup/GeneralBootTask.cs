using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;$endif$
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using $safeprojectname$.Application;
using $safeprojectname$.Application.Impl;
using Radial;
using Radial.Boot;
using Radial.Persist;
using Radial.Persist.Nhs;

namespace $safeprojectname$.Startup
{
    /// <summary>
    /// GeneralBootTask
    /// </summary>
    public abstract class GeneralBootTask : IBootTask
    {
        /// <summary>
        /// System initialize process.
        /// </summary>
        public virtual void Initialize()
        {
            Components.Container.RegisterType<IUnitOfWork, ContextualUnitOfWork>();

            Components.Container.RegisterType<IUserService, UserService>();
        }

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
