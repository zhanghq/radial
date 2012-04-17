using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;
using Radial.Lock;
using System.Threading.Tasks;

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
            if (args.RetryTimes <= 3)
            {
                string txt = string.Format("{0} retry {1} sleep 100 thread {2}", args.LockKey, args.RetryTimes, Thread.CurrentThread.ManagedThreadId);

                Console.WriteLine(txt);
                args.OutputLogger.Warn(txt);

                Thread.Sleep(100);
                args.CancelRetry = false;
            }
        }

        [Test]
        public void Acquire()
        {
            Parallel.For(0, 3, i =>
            {
                using (LockEntry e = LockEntry.Acquire("abc" + i))
                {
                    Assert.AreEqual(e.CreateTime, e.ExpireTime);
                }
            });
        }
    }
}
