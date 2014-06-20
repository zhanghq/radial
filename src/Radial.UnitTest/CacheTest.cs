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
using Radial.Persist.Nhs.Cache;

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

            //primitive cache
            CacheStatic.Put("test0", 34.34, 100);
            Console.WriteLine(CacheStatic.Get<decimal>("test0"));

            //collection cache
            IList<Temp> list = new List<Temp>();

            for (int i = 0; i < 100; i++)
                list.Add(new Temp { Name = Guid.NewGuid().ToString("N") });

            CacheStatic.Put("test1", list, 100);

            IList<Temp> list2 = CacheStatic.Get<IList<Temp>>("test1");

            if (list2 != null)
            {
                foreach (var o in list2)
                {
                    Console.WriteLine("Name: {0}, Time: {1}", o.Name, o.Time);
                }
            }

            //object cache
            CacheStatic.Put("test2", new Temp2 { Name = "sdfsd" }, 100);
            Console.WriteLine(CacheStatic.Get("test2"));

            //enum cache
            CacheStatic.Put("test3", SerializeFormat.Json, 100);
            Console.WriteLine(CacheStatic.Get<SerializeFormat>("test3"));
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

        [Test]
        public void GroupUse()
        {
            Components.Container.RegisterType<ICache, LocalCache>();
            Components.Container.RegisterType<ICacheGroupPersister, NhCacheGroupPersister>();

            string name = "123244";

            var keyDepends = new[] { new KeyValuePair<string, object>("name", name), new KeyValuePair<string, object>("name2", "234") };

            Temp t1 = new Temp { Name = name };

            CacheGroup.Put(typeof(Temp).Name, keyDepends, t1);

            Temp t2 = CacheGroup.Get<Temp>(typeof(Temp).Name, keyDepends);

            Assert.IsNotNull(t2);

            CacheGroup.Drop(typeof(Temp).Name, keyDepends);

            Temp t3 = CacheGroup.Get<Temp>(typeof(Temp).Name, keyDepends);

            Assert.IsNull(t3);

            CacheGroup.DropGroup(typeof(Temp).Name);
        }
    }
}
