using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Radial.UnitTest
{
    public struct B { public int K { get; set; } }
    public class A
    {
        public B V { get; set; }
    }

    [TestFixture]
    public class ToolkitsTest
    {
        [Test]
        public void Compress()
        {
            Assert.AreEqual("abc锁定", Toolkits.Decompress(Toolkits.Compress("abc锁定")));
        }

        [Test]
        public void GetPingResult()
        {
            Assert.DoesNotThrow(() => {
                Console.WriteLine(Toolkits.GetPingResult("localhost"));
            });
        }

        [Test]
        public void UnixTimeStamp()
        {
            DateTime time = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            Console.WriteLine(time);


            foreach (TimeZoneInfo tsi in TimeZoneInfo.GetSystemTimeZones())
            {
                Console.WriteLine("{0} {1}", tsi.Id, tsi.DisplayName);

                long ts = Toolkits.ToUnixTimeStamp(time, tsi);

                Console.WriteLine(ts);

                DateTime d = Toolkits.FromUnixTimeStamp(ts, tsi);

                Console.WriteLine(d);


                Assert.AreEqual(time, d);
            }
        }

        [Test]
        public void Pinyin()
        {
            Console.WriteLine(Radial.Pinyin.ConvertToPinyin("北京"));
            Console.WriteLine(Radial.Pinyin.ConvertToPinyin("上海", " "));
            Console.WriteLine(Radial.Pinyin.ConvertToPinyin("河南", " "));
        }
    }
}
