using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using Radial.Net;

namespace Radial.Web
{
    /// <summary>
    /// Extension class of http posted file.
    /// </summary>
    public static class HttpPostedFileExtension
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="postedFile">The http posted file.</param>
        /// <param name="paramName">The post parameter name.</param>
        /// <returns>The FileFormData instance.</returns>
        public static FileFormData ToFileFormData(this HttpPostedFileBase postedFile, string paramName)
        {
            Checker.Parameter(postedFile != null, "http posted file can not be null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(paramName), "post parameter name can not be empty or null");

            int b = -1;
            List<byte> fcbList = new List<byte>();
            using (Stream stream = postedFile.InputStream)
            {
                while ((b = stream.ReadByte()) > -1)
                {
                    fcbList.Add((byte)b);
                }
            }

            return new FileFormData(postedFile.FileName, fcbList.ToArray(), paramName, postedFile.ContentType);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="postedFile">The http posted file.</param>
        /// <param name="paramName">The post parameter name.</param>
        /// <returns>The FileFormData instance.</returns>
        public static FileFormData ToFileFormData(this HttpPostedFile postedFile, string paramName)
        {
            Checker.Parameter(postedFile != null, "http posted file can not be null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(paramName), "post parameter name can not be empty or null");

            int b = -1;
            List<byte> fcbList = new List<byte>();
            using (Stream stream = postedFile.InputStream)
            {
                while ((b = stream.ReadByte()) > -1)
                {
                    fcbList.Add((byte)b);
                }
            }

            return new FileFormData(postedFile.FileName, fcbList.ToArray(), paramName, postedFile.ContentType);
        }
    }
}
