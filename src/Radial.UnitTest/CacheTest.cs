using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Practices.Unity;
using Radial.Cache;
using System.IO;
using Radial.Serialization;

namespace Radial.UnitTest
{
    [TestFixture]
    public class CacheTest
    {
        class Temp
        {
            public Temp()
            {
                Time = DateTime.Now;
            }

            public string Name { get; set; }
            public DateTime Time { get; set; }
        }

        struct Temp2
        {
            public string Name { get; set; }
        }

        [Test]
        public void Memcached()
        {
            Components.Container.RegisterType<ICache, MemCache>();

            IList<Temp> list = new List<Temp>();

            for (int i = 0; i < 100; i++)
                list.Add(new Temp { Name = Guid.NewGuid().ToString("N") });

            CacheStatic.Set("test1", list, 100);

            IList<Temp> list2 = CacheStatic.Get<IList<Temp>>("test1");

            if (list2 != null)
            {
                foreach (var o in list2)
                {
                    Console.WriteLine("Name: {0}, Time: {1}", o.Name, o.Time);
                }
            }


            CacheStatic.Set("test2", new Temp2 { Name = "sdfsd" }, 100);
            Console.WriteLine(CacheStatic.Get("test2"));

            CacheStatic.Set("test3",SerializeFormat.Json, 100);
            Console.WriteLine(CacheStatic.Get("test3"));
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
