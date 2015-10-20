using NUnit.Framework;
using Radial.IO;

namespace Radial.UnitTest.IO
{
    [TestFixture]
    public class StreamKitsTest
    {
        [Test]
        public void Test1()
        {
            string path = @"D:\bcd\d\ab2c.txt";
            StreamKits.WriteToFile("你好", path);
            StreamKits.WriteToFile("你好2", path);
            StreamKits.AddLineToFile("你好23", path);
            StreamKits.AddLineToFile("你好233", path);
            StreamKits.AddLinesToFile(new string[] { "你好2332", "你好233a2" }, path);
            //StreamKits.TruncateFile(path);
        }

        [Test]
        public void GetLines()
        {
            var lines = Radial.IO.StreamKits.GetLines("..\\..\\XmlParamTest.cs");
        }
    }
}
