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
            //use log4net writer must  invoke prepare at bootup
            Log4NetWriter.Prepare();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
        }
    }
}
