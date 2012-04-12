using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Web;

namespace Radial.UnitTest
{
    [TestFixture]
    public class HttpKitsTest
    {
        [Test]
        public void GetContentType()
        {
            Assert.AreEqual(HttpKits.GetContentType(".jpg"),HttpKits.GetContentType(".jpeg"));
            Assert.AreEqual(HttpKits.GetContentType(".jpg"),HttpKits.GetContentType(".JPG"));
            Console.WriteLine(HttpKits.GetContentType(".jpeg"));
        }

        [Test]
        public void GetLocation()
        {
            Console.WriteLine(HttpKits.GetLocation("202.171.64.4"));
        }
    }
}
