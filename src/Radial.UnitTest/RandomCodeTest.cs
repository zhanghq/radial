using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Radial.UnitTest
{
    [TestFixture]
    public class RandomCodeTest
    {
        [Test]
        public void Test1()
        {
            Console.WriteLine(RandomCode.Create(10));
        }
    }
}
