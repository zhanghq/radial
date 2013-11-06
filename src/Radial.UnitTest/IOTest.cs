
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
            

            EasyStream.Writer.SaveToFile(bytes, filePath);

            Console.WriteLine(EasyStream.Reader.GetBytes(filePath).Length);

            EasyStream.Writer.AppendToFile(bytes, filePath);


            Console.WriteLine(EasyStream.Reader.GetText(filePath).Length);

        }
    }
}
