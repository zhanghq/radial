using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Net;
using System.IO;
using Radial.Net;

namespace Radial.UnitTest
{
    [TestFixture]
    public class FtpClientTest
    {
        [Test]
        public void Demo()
        {
            FtpWebRequest req = FtpWebRequest.Create("ftp://ftp.adobe.com") as FtpWebRequest;
            req.Credentials = new NetworkCredential("anonymous", "me@zhanghq.net");
            req.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            req.UsePassive = true;

            FtpWebResponse resp = req.GetResponse() as FtpWebResponse;

            using (StreamReader sr =new StreamReader(resp.GetResponseStream()))
            {
                while (!sr.EndOfStream)
                    Console.WriteLine(sr.ReadLine());
            }
        }
    }
}
