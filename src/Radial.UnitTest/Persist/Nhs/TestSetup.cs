using NUnit.Framework;
using Radial.Persist.Nhs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Radial.Persist;

namespace Radial.UnitTest.Persist.Nhs
{
    [SetUpFixture]
    public class TestSetup
    {

        [OneTimeSetUp]
        public void SetUp()
        {
            Dependency.Container.RegisterType<IFactoryPoolInitializer, NewFactoryPoolInitializer>(new ContainerControlledLifetimeManager());
            Dependency.Container.RegisterType<IUnitOfWork, UnitOfWork>();
            Dependency.Container.RegisterInterfaces(this.GetType().Assembly, "nhs");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }
    }
}
