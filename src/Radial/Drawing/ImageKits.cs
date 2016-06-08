using System;
using System.Drawing;

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
        /// Generates the thumbnail from the original image.
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

        /// <summary>
        /// Crop whitespace from image.
        /// </summary>
        /// <param name="original">The original image.</param>
        /// <param name="makeTransparent">if set to <c>true</c> [to make output image transparent].</param>
        /// <returns></returns>
        public static Image CropWhitespace(Image original, bool makeTransparent = false)
        {
            Checker.Parameter(original != null, "original image can not be null");

            Bitmap bmp = original as Bitmap;

            Checker.Requires(bmp != null, "can not cast original image to bitmap");

            int w = bmp.Width;
            int h = bmp.Height;

            Func<int, bool> allWhiteRow = row =>
            {
                for (int i = 0; i < w; ++i)
                    if (bmp.GetPixel(i, row).R != 255)
                        return false;
                return true;
            };

            Func<int, bool> allWhiteColumn = col =>
            {
                for (int i = 0; i < h; ++i)
                    if (bmp.GetPixel(col, i).R != 255)
                        return false;
                return true;
            };

            int topmost = 0;
            for (int row = 0; row < h; ++row)
            {
                if (allWhiteRow(row))
                    topmost = row;
                else break;
            }

            int bottommost = 0;
            for (int row = h - 1; row >= 0; --row)
            {
                if (allWhiteRow(row))
                    bottommost = row;
                else break;
            }

            int leftmost = 0, rightmost = 0;
            for (int col = 0; col < w; ++col)
            {
                if (allWhiteColumn(col))
                    leftmost = col;
                else
                    break;
            }

            for (int col = w - 1; col >= 0; --col)
            {
                if (allWhiteColumn(col))
                    rightmost = col;
                else
                    break;
            }

            if (rightmost == 0) rightmost = w; // As reached left
            if (bottommost == 0) bottommost = h; // As reached top.

            int croppedWidth = rightmost - leftmost;
            int croppedHeight = bottommost - topmost;

            if (croppedWidth == 0) // No border on left or right
            {
                leftmost = 0;
                croppedWidth = w;
            }

            if (croppedHeight == 0) // No border on top or bottom
            {
                topmost = 0;
                croppedHeight = h;
            }

            var target = new Bitmap(croppedWidth, croppedHeight);
            using (Graphics g = Graphics.FromImage(target))
            {
                g.DrawImage(bmp,
                  new RectangleF(0, 0, croppedWidth, croppedHeight),
                  new RectangleF(leftmost, topmost, croppedWidth, croppedHeight),
                  GraphicsUnit.Pixel);

                if (makeTransparent)
                    target.MakeTransparent();
            }
            return target;

        }
    }
}
