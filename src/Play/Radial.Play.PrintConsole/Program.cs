using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;

namespace Radial.Play.PrintConsole
{
    class Program
    {

        static void Main(string[] args)
        {
            PrintDocument pdoc = new PrintDocument();
            pdoc.DocumentName = "上海电信DNS设置.PNG";
            PaperSize psize = new PaperSize();
            psize.RawKind = 9;
            pdoc.DefaultPageSettings.Margins = new Margins(0, 0, 0, 0);
            pdoc.DefaultPageSettings.Landscape = true; 
            pdoc.PrintPage += pdoc_PrintPage;
            pdoc.Print();
        }

        static void pdoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            PrintDocument pdoc = sender as PrintDocument;

            Image image = Thumbnail(Image.FromFile("img.jpg"), 1120, 840);

            System.Drawing.Rectangle destRect = new System.Drawing.Rectangle(0, 0, image.Width, image.Height);
            e.Graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel);

            image.Dispose();
        }

        public static Image Thumbnail(Image original, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);

            using (original)
            using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp))
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                gr.DrawImage(original, 0, 0, width, height);
            }

            return bmp;
        }
    }
}
