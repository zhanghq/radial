using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Radial.Cache;
using System.IO;

namespace Radial.UnitTest
{
    [TestFixture]
    public class CacheTest
    {
        [Test]
        public void Memcached()
        {
            Components.Container.RegisterType<ICache, MemCache>();


            CacheStatic.SetString("name", "abc", 100);

            Console.WriteLine(CacheStatic.GetString("name"));
        }

        //[Test]
        //public void Redis()
        //{
        //    Components.Container.RegisterType<ICache, RedisCache>();


        //    //CacheStatic.SetString("name", "abc", 100);

        //    Console.WriteLine(CacheStatic.GetString("name"));            

        //}

        //[Test]
        //public void RedisDirect()
        //{
        //    RedisCache rca = new RedisCache();

        //    rca.Remove("test1");

        //    byte[] buffer = new byte[1024 * 1024];
        //    IDictionary<string, byte[]> dict = new Dictionary<string, byte[]>();
        //    for (int i = 0; i < 1000; i++)
        //    {
        //        RandomCode.NewInstance.NextBytes(buffer);
        //        dict.Add(new KeyValuePair<string, byte[]>(Guid.NewGuid().ToString("N"), buffer));
        //    }

        //    rca.SetHash("test1", dict);

        //    RandomCode.NewInstance.NextBytes(buffer);
        //    rca.SetHashBinary("test1", Guid.NewGuid().ToString("N"), buffer);

        //    Console.WriteLine(rca.GetHash("test1").Count);
        //}
    }
}
