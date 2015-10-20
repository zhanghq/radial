using System;
using NUnit.Framework;
using Microsoft.Practices.Unity;
using Radial.Param;

namespace Radial.UnitTest
{
    [TestFixture]
    public class XmlParamTest
    {
        [TestFixtureSetUp]
        public void Setup()
        {
            Dependency.Container.RegisterType<IParam, XmlParam>();
        }

        [Test]
        public void Get()
        {
            Console.WriteLine(AppParam.GetValue("test2.levelx"));
        }

        [Test]
        public void Save()
        {
            AppParam.Save("test3", null);

            Assert.AreEqual(null, AppParam.GetValue("test3"));

            AppParam.Save("test3", 123);

            Assert.AreEqual(123, AppParam.GetValueInt32("test3"));

            AppParam.Save("test3", "使用", null);

            Assert.AreEqual(null, AppParam.GetValue("test3"));
        }
    }
}
