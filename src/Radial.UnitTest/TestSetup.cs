using NUnit.Framework;

namespace Radial.UnitTest
{
    [SetUpFixture]
    public class TestSetup
    {

        [OneTimeSetUp]
        public void SetUp()
        {
            //StaticVariables.ConfigDirectory = StaticVariables.ConfigDirectory.Replace(@"\bin\Debug", string.Empty);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }
    }
}
