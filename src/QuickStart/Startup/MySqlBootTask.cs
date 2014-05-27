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

namespace QuickStart.Startup
{
    /// <summary>
    /// MySqlBootTask
    /// </summary>
    public class MySqlBootTask : GeneralBootTask
    {
        public override void Initialize()
        {
            base.Initialize();

            Components.Container.RegisterType<IFactoryPoolInitializer, MySqlFactoryPoolInitializer>(new ContainerControlledLifetimeManager());

            //Your code here
        }
    }
}
