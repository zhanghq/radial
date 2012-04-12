using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Data.Nhs;
using NHibernate;
using Radial.Param;

namespace Radial.UnitTest.Nhs.Param
{
    [TestFixture]
    public class SqlCeTest
    {
        [TestFixtureSetUp]
        public void SetUp()
        {
            Radial.ComponentContainer.RegisterSingleton<Radial.Data.Nhs.Param.IParamRepository, Radial.Data.Nhs.Param.SqlCeParamRepository>();
            Radial.ComponentContainer.RegisterSingleton<Radial.Param.IParam, Radial.Data.Nhs.Param.DefaultParam>();

            HibernateEngine.OpenAndBindSession();

        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            ISession session = HibernateEngine.UnbindSession();
            if (session != null)
                session.Dispose();

        }


        [Test]
        public void Create()
        {
            string path = "testcreate";

            ParamObject po = AppParam.Create(path, "斯蒂芬", "");
            ParamObject po2 = AppParam.Create(path + ".level1", "阿萨德", "");
            Assert.True(AppParam.Next(path)[0].Path == po2.Path);
            AppParam.Create(path + ".level2", "阿萨德", "");
            AppParam.Delete(path + ".level2");
            AppParam.Delete(path + ".level1");
            AppParam.Delete(path);

        }

        [Test]
        public void Update()
        {
            string path = "testcreate";

            AppParam.Create(path, "斯蒂芬", "");
            ParamObject po2 = AppParam.Create(path + ".level1", "阿萨德", "");

            po2=AppParam.Update(path + ".level1", "adf", "sdf");

            Assert.AreEqual("sdf",po2.Value);

            AppParam.Delete(path + ".level1");
            AppParam.Delete(path);

        }

        [Test]
        public void Next()
        {
            string path = "testcreate";

            AppParam.Create(path, "斯蒂芬", "");
            AppParam.Create(path + ".level1", "阿萨德", "");
            AppParam.Create(path + ".level2", "阿萨德", "");


            int objectTotal;

            IList<ParamObject> list = AppParam.Next(path, 2, 1, out objectTotal);

            Assert.AreEqual(2, objectTotal);

            AppParam.Delete(path + ".level2");
            AppParam.Delete(path + ".level1");
            AppParam.Delete(path);
        }
    }
}
