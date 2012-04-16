using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;
using Radial.Lock;

namespace Radial.UnitTest
{
    [TestFixture]
    public class LockTest
    {
        [Test]
        public void Acquire()
        {
            using (LockEntry e = LockEntry.Acquire("abc"))
            {
                Assert.AreNotEqual(e.CreateTime, e.ExpireTime);
            }
        }
    }
}
