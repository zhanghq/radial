using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

namespace Radial.Drawing
{
    /// <summary>
    /// Toolkits class for image.
    /// </summary>
    public static class ImageKits
    {
        /// <summary>
        /// Crop the original image.
        /// </summary>
        /// <param name="original">The original image.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the image.</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the image.</param>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image.</param>
        /// <returns>
        /// The cropped image .
        /// </returns>
        public static Image Crop(Image original, int x, int y, int width, int height)
        {
            Checker.Parameter(original != null, "original image can not be null");

            Bitmap bmp = new Bitmap(width, height);

            using (original)
            using (System.Drawing.Graphics gr = System.Drawing.Graphics.FromImage(bmp))
            {
                gr.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                gr.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                gr.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

                gr.DrawImage(original, 0, 0, new Rectangle(x, y, width, height), GraphicsUnit.Pixel);
            }

            return bmp;
        }

        /// <summary>
        /// Generates the thumbnail of the original image.
        /// </summary>
        /// <param name="original">The original image.</param>
        /// <param name="width">The width, in pixels, of the requested thumbnail image.</param>
        /// <param name="height">The height, in pixels, of the requested thumbnail image.</param>
        /// <returns>
        /// An Image that represents the thumbnail.
        /// </returns>
        public static Image Thumbnail(Image original, int width, int height)
        {
            Checker.Parameter(original != null, "original image can not be null");

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
