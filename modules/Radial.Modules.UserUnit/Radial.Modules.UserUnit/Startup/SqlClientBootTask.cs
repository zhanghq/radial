using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Radial;
using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.Web;
using Radial.Modules.UserUnit.Infras.Persist;
using Radial.Modules.UserUnit.Infras.Persist.Initializer;
using Radial.Modules.UserUnit.Domain.Repos;
using Radial.Modules.UserUnit.Infras.Repos.SqlClient;

namespace Radial.Modules.UserUnit.Startup
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
