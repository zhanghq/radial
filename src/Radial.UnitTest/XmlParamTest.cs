using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            Components.Container.RegisterType<IParam, XmlParam>();
        }

        [Test]
        public void Get()
        {
            Console.WriteLine(AppParam.GetValue("test2.levelx"));
        }
    }
}
