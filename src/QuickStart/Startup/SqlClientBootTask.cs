using Microsoft.Practices.Unity;
using Radial;
using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickStart.Infras.Persist;
using QuickStart.Infras.Persist.Initializer;
using QuickStart.Domain.Repos;
using QuickStart.Infras.Repos.SqlClient;

namespace QuickStart.Startup
{
    /// <summary>
    /// SqlClientBootTask
    /// </summary>
    public class SqlClientBootTask : GeneralBootTask
    {
        protected override void InitializePoolInitializer()
        {
            Components.Container.RegisterType<IFactoryPoolInitializer, SqlClientFactoryPoolInitializer>(new ContainerControlledLifetimeManager());
        }

        protected override void InitializeRepositories()
        {
            Components.Container.RegisterType<IUserRepository, UserRepository>();
        }
    }
}
