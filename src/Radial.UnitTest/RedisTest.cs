using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookSleeve;
using NUnit.Framework;

namespace Radial.UnitTest
{
    [TestFixture]
    public class RedisTest
    {
        [Test]
        public void Demo()
        {
            using (var conn = new RedisConnection("192.168.62.129"))
            {
                conn.Open();

                conn.Strings.Set(0, "foo", "test", 100);

                string val = conn.Strings.GetString(0, "foo").Result;

                Console.WriteLine(val);
            }
        }
    }
}
