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
        public LockTest()
        {
            LockEntry.AcquireFailed += new AcquireFailedEventHandler(LockEntry_AcquireFailed);
        }

        void LockEntry_AcquireFailed(object sender, AcquireFailedEventArgs args)
        {
            Console.WriteLine(args.RetryTimes);
            if (args.RetryTimes < 10)
                args.KeepOnRetry = true;
        }

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
