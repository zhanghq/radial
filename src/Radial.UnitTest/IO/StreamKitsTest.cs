using NUnit.Framework;
using Radial.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest.IO
{
    [TestFixture]
    public class StreamKitsTest
    {
        [Test]
        public void Test1()
        {
            string path = @"D:\bcd\d\ab2c.txt";
            StreamKits.SaveToFile("你好", path);
            StreamKits.SaveToFile("你好2", path);
            StreamKits.AppendLine("你好23", path);
            StreamKits.AppendLine("你好233", path);
            StreamKits.AppendLines(new string[] { "你好2332", "你好233a2" }, path);
            //StreamKits.TruncateFile(path);
        }
    }
}
