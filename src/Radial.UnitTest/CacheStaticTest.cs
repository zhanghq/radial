using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Cache;

namespace Radial.UnitTest
{
    [TestFixture]
    public class CacheStaticTest
    {
        class TObj
        {
            public string name;
            public string value;
        }

        [Test]
        public void Memcached_IList()
        {
            IList<TObj> list = new List<TObj>();
            list.Add(new TObj { name = "a", value = "c" });
            list.Add(new TObj { name = "a", value = "c" });
            list.Add(new TObj { name = "a", value = "c" });

            CacheStatic.Set<IList<TObj>>("a", list, 100);

            IList<TObj> list2 = CacheStatic.Get<IList<TObj>>("a");
            //CacheStatic.Set<string>("abc", "<xml></xml>");
            //string s= CacheStatic.Get<string>("abc");
        }
    }
}
