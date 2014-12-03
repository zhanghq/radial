using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Radial.Extensions
{
    /// <summary>
    /// IO extensions
    /// </summary>
    public static class IOExtensions
    {
        /// <summary>
        /// Read all bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static byte[] ReadBytes(this Stream stream)
        {
            IList<byte> bytes = new List<byte>();

            if (stream != null && stream.CanRead)
            {
                stream.Position = 0;
                int b = 0;
                while ((b = stream.ReadByte()) > -1)
                    bytes.Add((byte)b);
            }
            return bytes.ToArray();
        }
    }
}
