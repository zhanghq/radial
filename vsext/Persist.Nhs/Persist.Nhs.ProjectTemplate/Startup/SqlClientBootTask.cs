using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;$endif$
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Radial;
using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.Web;
using $safeprojectname$.Infras.Persist;
using $safeprojectname$.Infras.Persist.Initializer;
using $safeprojectname$.Domain.Repos;
using $safeprojectname$.Infras.Repos.SqlClient;

namespace $safeprojectname$.Startup
{
    /// <summary>
    /// SqlClientBootTask
    /// </summary>
    public class SqlClientBootTask : GeneralBootTask
    {
        public override void Initialize()
        {
            base.Initialize();

            Components.Container.RegisterType<IFactoryPoolInitializer, SqlClientFactoryPoolInitializer>(new ContainerControlledLifetimeManager());

            //Your code here
            Components.Container.RegisterType<IUserRepository, UserRepository>();
        }
    }
}
