using System;
using System.Linq;
using BookSleeve;
using NUnit.Framework;

namespace Radial.UnitTest
{
    [TestFixture]
    public class RedisTest
    {
        [Test]
        public void StringsDemo()
        {
            using (var conn = new RedisConnection("subu.cloudapp.net"))
            {
                conn.Open();

                conn.Strings.Set(0, "foo", "test", 100);

                string val =conn.Strings.GetString(0, "foo").Result;

                Console.WriteLine(val);
            }
        }

        [Test]
        public void HashDemo()
        {
            using (var conn = new RedisConnection("subu.cloudapp.net"))
            {
                conn.Open();
                conn.Hashes.Set(0, "user2s", "1", "name");
                conn.Hashes.Set(0, "user2s", "2", "name1");

                Console.WriteLine(conn.Hashes.GetKeys(0, "user2s").Result.Count());
            }
        }            
    }
}
