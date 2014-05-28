using System;
using System.Collections.Generic;
$if$ ($targetframeworkversion$ >= 3.5)using System.Linq;$endif$
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Radial;
using Radial.Persist;
using Radial.Persist.Nhs;
using $safeprojectname$.Infras.Persist;
using $safeprojectname$.Infras.Persist.Initializer;
using $safeprojectname$.Domain.Repos;

namespace $safeprojectname$.Startup
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
