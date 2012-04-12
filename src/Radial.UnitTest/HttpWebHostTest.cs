using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Web;
using System.IO;
using Radial.Net;

namespace Radial.UnitTest
{
    [TestFixture]
    public class HttpWebHostTest
    {
        [Test]
        public void Get()
        {
            string url = "http://sh.sinaimg.cn/cr/2011/1103/2163264101.jpg";

            HttpResponseObj rsp = HttpWebHost.Get(url);

            Assert.NotNull(rsp.RawData);

            Console.WriteLine(rsp.Text);
        }
    }
}
