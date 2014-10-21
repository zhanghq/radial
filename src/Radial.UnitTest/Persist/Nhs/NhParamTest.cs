using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Persist.Nhs.Param;

namespace Radial.UnitTest.Persist.Nhs
{
     [TestFixture]
    public class NhParamTest
    {
         [Test]
         public void Test1()
         {
             string path = Guid.NewGuid().ToString("N");
             string value = RandomCode.NewInstance.Next(100).ToString();
             NhParam p = new NhParam();
             p.Save(path, value);
             Assert.AreEqual(value, p.GetValue(path));
         }
    }
}
