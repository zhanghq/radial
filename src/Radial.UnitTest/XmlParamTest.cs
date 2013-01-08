using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Param;
using System.Xml.Linq;

namespace Radial.UnitTest
{
    [TestFixture]
    public class XmlParamTest
    {

        [TestFixtureSetUp]
        public void SetUp()
        {
            ComponentContainer.RegisterPerThread<IParam, XmlParam>();
        }

        [Test]
        public void Exist()
        {
            Assert.True(AppParam.Exists("test1.level1"));
            Console.WriteLine(AppParam.Get("test1.level1").Description);
        }

        [Test]
        public void Get()
        {
            ParamObject o = AppParam.Get("test1.level1.level1-1.level1-1-1");
            Assert.IsNotNull(o);
            Console.WriteLine(o.Value);
        }
        [Test]
        public void ContainsNext()
        {
            Assert.True(AppParam.Get("test1.level1.level1-1").ContainsNext);
        }

        [Test]
        public void Next()
        {
            Assert.AreEqual(1,AppParam.Next("").Count);
            Assert.AreEqual(5,AppParam.Next("test1").Count);
            Assert.AreEqual(1, AppParam.Next("test1.level1").Count);

            int objectTotal;

            IList<ParamObject> list = AppParam.Next("test1", 2, 1, out objectTotal);

            Assert.AreEqual(2, list.Count);

            Assert.AreEqual(5, objectTotal);
        }

        [Test]
        public void Create()
        {
            string path = "testcreate";

            AppParam.Save(path, "斯蒂芬", "");
            AppParam.Save(path + ".level1", "阿萨德", "");
            AppParam.Save(path + ".level2", "阿萨德", "");
            AppParam.Delete(path + ".level2");
            AppParam.Delete(path + ".level1");
            AppParam.Delete(path);

        }

        [Test]
        public void Update()
        {
            string path = "testupdate";

            AppParam.Save(path, "234", "d");
            AppParam.Save(path + ".level1", "阿萨德", "");
            AppParam.Save(path + ".level1", "", "");
            AppParam.Delete(path + ".level1");
            AppParam.Delete(path);
        }

        [Test]
        public void Search()
        {
            int objectTotal;

            IList<ParamObject> list = AppParam.Search("test", 2, 1, out objectTotal);

            Assert.AreEqual(1, objectTotal);
        }
    }
}
