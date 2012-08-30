﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;
using Radial.DistLock;
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
            if (args.NextRetryTimes <= 3)
            {
                string txt = string.Format("{0} sleep 100ms before retry {1} thread {2}", args.LockKey, args.NextRetryTimes, Thread.CurrentThread.ManagedThreadId);

                Console.WriteLine(txt);
                //args.OutputLogger.Warn(args.Exception);

                Thread.Sleep(100);
                args.CancelRetry = false;
            }
        }

        [Test]
        public void Acquire()
        {
            //Parallel.For(0, 5, i =>
            //{
                using (LockEntry e = LockEntry.Acquire("abc"))
                {
                    //Assert.AreEqual(e.CreateTime, e.ExpireTime);
                }
            //});

        }
    }
}
