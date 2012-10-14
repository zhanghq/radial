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

            string str1 = Base62Encoder.ToBase62String(1231717);
            Assert.AreEqual(1231717, Base62Encoder.FromBase62String(str1));
        }
    }
}
