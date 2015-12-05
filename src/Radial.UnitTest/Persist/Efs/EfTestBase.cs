using Radial.Persist.Efs;
using Microsoft.Practices.Unity;
using NUnit.Framework;

namespace Radial.UnitTest.Persist.Efs
{
    public class EfTestBase
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Radial.Dependency.Container.RegisterType<IDbContextPoolInitializer, DefaultPoolInitializer>();
        }
    }
}
