using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Cache.Memcached;
using System.Threading;

namespace Radial.UnitTest
{
    [TestFixture]
    public class MemcachedTest
    {
        [Test]
        public void Common()
        {
            EnyimCache cache = new EnyimCache();

            cache.Set("t", 2, 5);

            Thread.Sleep(2000);

            Console.WriteLine(cache.Get("t"));
        }
    }
}
