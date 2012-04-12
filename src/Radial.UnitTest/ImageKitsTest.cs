using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Drawing;
using System.Drawing;

namespace Radial.UnitTest
{
    [TestFixture]
    public class ImageKitsTest
    {
        Image _image;

        public ImageKitsTest()
        {
            _image = Image.FromFile(SystemVariables.BaseDirectory.Replace(@"\bin\Debug", string.Empty) + @"\Chrysanthemum.jpg");
        }

        [Test]
        public void Thumbnail()
        {
            Image thumb = ImageKits.Thumbnail(_image, 640, 480);

            thumb.Save(@"C:\thumb.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            thumb.Dispose();
        }

        [Test]
        public void Crop()
        {
            Image crop = ImageKits.Crop(_image, 120, 120, 240, 240);

            crop.Save(@"C:\crop.jpg", System.Drawing.Imaging.ImageFormat.Jpeg);

            crop.Dispose();
        }
       
    }
}
