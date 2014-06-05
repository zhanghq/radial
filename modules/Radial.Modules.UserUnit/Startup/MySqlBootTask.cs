using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Radial;
using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.Modules.UserUnit.Infras.Persist;
using Radial.Modules.UserUnit.Infras.Persist.Initializer;
using Radial.Modules.UserUnit.Domain.Repos;

namespace Radial.Modules.UserUnit.Startup
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
