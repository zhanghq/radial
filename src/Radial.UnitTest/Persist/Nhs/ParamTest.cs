using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Persist.Nhs.Param;

namespace Radial.UnitTest.Persist.Nhs
{
     [TestFixture]
    public class ParamTest
    {
        [Test]
        public void Test1()
        {
            NhParam p = new NhParam();
            p.Save("123", "23");
        }
    }
}
