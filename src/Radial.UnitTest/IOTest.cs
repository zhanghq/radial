
using NUnit.Framework;
using Radial.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest
{
    [TestFixture]
    public class IOTest
    {
        [Test]
        public void Flies()
        {
            string filePath = "iotest.txt";
            byte[] bytes = new byte[1024];
            

            StreamKits.Writer.SaveToFile(bytes, filePath);

            Console.WriteLine(StreamKits.Reader.GetBytes(filePath).Length);

            StreamKits.Writer.AppendToFile(bytes, filePath);


            Console.WriteLine(StreamKits.Reader.GetText(filePath).Length);

        }
    }
}
