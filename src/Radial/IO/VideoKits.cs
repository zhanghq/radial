using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.IO
{
    /// <summary>
    /// VideoKits.
    /// </summary>
    public static class VideoKits
    {

        /// <summary>
        /// Extract video frame from local video file at specified position.
        /// </summary>
        /// <param name="inputFile">The path to local video file.</param>
        /// <param name="frameTime">The video position (in seconds).</param>
        /// <returns>
        /// The image of the video frame.
        /// </returns>
        public static Image ExtractFrame(string inputFile,float? frameTime = null)
        {
            var ffMpeg = new FFMpegConverter();

            using (MemoryStream ms = new MemoryStream())
            {
                ffMpeg.GetVideoThumbnail(inputFile, ms, frameTime);

                Image img0 = Bitmap.FromStream(ms);

                Image img1 = new Bitmap(img0.Width, img0.Height);

                Graphics gr = Graphics.FromImage(img1);

                gr.DrawImage(img0, 0, 0);

                gr.Dispose();
                img0.Dispose();

                return img1;
            }
        }
    }
}
