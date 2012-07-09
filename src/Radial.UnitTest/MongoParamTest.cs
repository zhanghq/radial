using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Param;
using Radial.Data.Mongod.Param;

namespace Radial.UnitTest
{
    [TestFixture]
    public class MongoParamTest
    {
        MongoDefaultParam _param;

        public MongoParamTest()
        {
            _param = new MongoDefaultParam();
        }


        [Test]
        public void Create()
        {
            string path = "testcreate";

            ParamObject po = _param.Create(ParamObject.BuildPath(path), "斯蒂芬", "");
            ParamObject po2 = _param.Create(ParamObject.BuildPath(path, "level1"), "阿萨德", "");
            Assert.True(_param.Next(path)[0].Path == po2.Path);
            _param.Create(ParamObject.BuildPath(path, "level2"), "阿萨德", "");
            _param.Delete(ParamObject.BuildPath(path, "level2"));
            _param.Delete(ParamObject.BuildPath(path, "level1"));
            _param.Delete(path);

        }

        [Test]
        public void Update()
        {
            string path = "testcreate";

            _param.Create(path, "斯蒂芬", "");
            ParamObject po2 = _param.Create(path + ".level1", "阿萨德", "");

            po2 = _param.Update(path + ".level1", "adf", "sdf");

            Assert.AreEqual("sdf", po2.Value);

            _param.Delete(path + ".level1");
            _param.Delete(path);

        }

        [Test]
        public void Next()
        {

            string path = "testcreate";

            _param.Create(path, "斯蒂芬", "");
            _param.Create(path + ".level1", "阿萨德", "");
            _param.Create(path + ".level2", "阿萨德", "");


            int objectTotal;

            IList<ParamObject> list = _param.Next(path, 2, 1, out objectTotal);

            Assert.AreEqual(2, objectTotal);

            _param.Delete(path + ".level2");
            _param.Delete(path + ".level1");
            _param.Delete(path);
        }
    }
}
