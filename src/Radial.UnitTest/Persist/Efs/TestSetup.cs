using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Radial.Persist;
using Radial.Persist.Efs;


namespace Radial.UnitTest.Persist.Efs
{
    [SetUpFixture]
    public class TestSetup
    {

        [OneTimeSetUp]
        public void SetUp()
        {
            Dependency.Container.RegisterType<IDbContextPoolInitializer, NewDbContextPoolInitializer>();
            Dependency.Container.RegisterType<IUnitOfWork, UnitOfWork>();
            Dependency.Container.RegisterInterfaces(this.GetType().Assembly,"efs");
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }
    }
}
