using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Radial.UnitTest
{
    [TestFixture]
    public class ToolkitsTest
    {
        [Test]
        public void Ping()
        {
            ((List<System.Net.NetworkInformation.PingReply>)Toolkits.GetPingReplies("www.baidu.com")).ForEach(o =>
            {
                if (o.Status == System.Net.NetworkInformation.IPStatus.Success)
                    Console.WriteLine("Reply from {0} Bytes={1} Time={2}ms TTL={3}", o.Address, o.Buffer.Length, o.RoundtripTime, o.Options.Ttl);
                else
                    Console.WriteLine(o.Status);
            });
        }
    }
}
