using Microsoft.Practices.Unity;
using Radial;
using Radial.Persist;
using Radial.Persist.Nhs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuickStart.Infras.Persist;
using QuickStart.Infras.Persist.Initializer;
using QuickStart.Domain.Repos;
using QuickStart.Infras.Repos.MySql;

namespace QuickStart.Startup
{
    /// <summary>
    /// MySqlBootTask
    /// </summary>
    public class MySqlBootTask : GeneralBootTask
    {
        protected override void InitializePoolInitializer()
        {
            Components.Container.RegisterType<IFactoryPoolInitializer, MySqlFactoryPoolInitializer>(new ContainerControlledLifetimeManager());
        }

        protected override void InitializeRepositories()
        {
            Components.Container.RegisterType<IUserRepository, UserRepository>();
        }
    }
}
