using NUnit.Framework;
using Radial.Persist.Nhs.Param;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.Persist.Nhs
{
    [TestFixture]
    public class KvParamTest
    {
        [Test]
        public void Get()
        {
            KvParam kvp = new KvParam();

            var obj = kvp.Get("test1");

            Assert.AreEqual(true, obj.HasNext);
        }

        [Test]
        public void Next()
        {
            KvParam kvp = new KvParam();

            var objs = kvp.Next("test1");

            int total;
            var objs2 = kvp.Next("test1", 1, 1, out total);
        }

        [Test]
        public void Search()
        {
            KvParam kvp = new KvParam();

            var objs = kvp.Search("test");

            int total;
            var objs2 = kvp.Search("test", 1, 1, out total);
        }

        [Test]
        public void Delete()
        {
            KvParam kvp = new KvParam();
            kvp.Delete("test1.abc.abc");
        }

        [Test]
        public void Save()
        {
            KvParam kvp = new KvParam();
            kvp.Save("test2.abc.abc", Guid.NewGuid().ToString());
            kvp.Save("test2.abc.abc", Guid.NewGuid().ToString());
            kvp.Delete("test2.abc");
        }
    }
}
