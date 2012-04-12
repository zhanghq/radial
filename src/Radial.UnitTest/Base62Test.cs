using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Radial.UnitTest
{
    [TestFixture]
    public class Base62Test
    {
        [Test]
        public void Base62()
        {
            Base62Encoder encoder = new Base62Encoder();

            string str1 = encoder.ToBase62String(1231717);
            Assert.AreEqual(1231717, encoder.FromBase62String(str1));
        }
    }
}
