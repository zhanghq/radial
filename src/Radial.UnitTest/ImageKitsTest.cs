using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.UnitTest
{
    [TestFixture]
    public class ImageKitsTest
    {
        [Test]
        public void CropWhitespace()
        {
            Image img = Image.FromFile(GlobalVariables.GetPath("CropWhitespace.png"));
            Stopwatch watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 10; i++)
                Drawing.ImageKits.CropWhitespace(img);
            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds / 10);
        }
    }
}
