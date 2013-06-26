using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Radial.UnitTest
{
    [SetUpFixture]
    public class TestSetup
    {


        [SetUp]
        public void SetUp()
        {
            SystemSettings.ConfigDirectory = SystemSettings.ConfigDirectory.Replace(@"\bin\Debug", string.Empty);
        }

        [TearDown]
        public void TearDown()
        {
        }
    }
}
