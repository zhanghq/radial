using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Net;
using System.IO;
using Radial.Net;
using System.Net.FtpClient;

namespace Radial.UnitTest
{
    [TestFixture]
    public class FtpClientTest
    {
        [Test]
        public void Demo()
        {
            //FtpWebRequest req = FtpWebRequest.Create("ftp://ftp.adobe.com/Acrobat") as FtpWebRequest;
            //req.Credentials = new NetworkCredential("anonymous", "me@zhanghq.net");
            //req.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            //req.UsePassive = true;

            //FtpWebResponse resp = req.GetResponse() as FtpWebResponse;

            //using (StreamReader sr =new StreamReader(resp.GetResponseStream()))
            //{
            //    while (!sr.EndOfStream)
            //        Console.WriteLine(sr.ReadLine());
            //}

            using (FtpClient conn = new FtpClient())
            {
                conn.Host = "ftp.adobe.com";
                conn.Credentials = new NetworkCredential("anonymous", "me@zhanghq.net");

                foreach (FtpListItem item in conn.GetListing("/", FtpListOption.Modify))
                {
                    Console.WriteLine("Name: {0} Modify: {1} Size: {2}", item.Name, item.Modified, item.Size);
                }
            }
        }
    }
}
