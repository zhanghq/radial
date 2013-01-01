using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Param;
using Autofac;
using Radial.Data.Nhs.Param;
using Radial.Data.Nhs;
using System.Threading;

namespace Radial.UnitTest.Nhs.Param
{
    [TestFixture]
    public class NhParamTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Components.AdditionalRegister = o =>
            {
                //o.RegisterType<NewFactoryPoolInitializer>().As<IFactoryPoolInitializer>();
                o.RegisterType<NhParam>().As<IParam>();
            };
        }

        [Test]
        public void Create()
        {
            string path = "testcreate";

            AppParam.Save(ParamObject.BuildPath(path), "斯蒂芬", "");
            AppParam.Save(ParamObject.BuildPath(path, "level1"), "阿萨德", "1");
            //Assert.True(AppParam.Next(path)[0].Path == po2.Path);
            AppParam.Save(ParamObject.BuildPath(path, "level2"), "阿萨德", "2");
            AppParam.Delete(ParamObject.BuildPath(path, "level2"));
            AppParam.Delete(ParamObject.BuildPath(path, "level1"));
            AppParam.Delete(path);

        }

        [Test]
        public void Update()
        {
            string path = "testcreate";

            AppParam.Save(path, "斯蒂芬", "");
            AppParam.Save(path + ".level1", "阿萨德", "");

             AppParam.Save(path + ".level1", "adf", "sdf");

            //Assert.AreEqual("sdf", po2.Value);

            AppParam.Delete(path + ".level1");
            AppParam.Delete(path);

        }

        [Test]
        public void Next()
        {

            string path = "testcreate";

            AppParam.Save(path, "斯蒂芬", "");
            AppParam.Save(path + ".level1", "阿萨德", "");
            AppParam.Save(path + ".level2", "阿萨德", "");


            int objectTotal;

            IList<ParamObject> list = AppParam.Next(path, 2, 1, out objectTotal);

            Assert.AreEqual(2, objectTotal);

            AppParam.Delete(path + ".level2");
            AppParam.Delete(path + ".level1");
            AppParam.Delete(path);
        }
    }
}
