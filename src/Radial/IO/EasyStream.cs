using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.IO
{
    /// <summary>
    /// Easy stream.
    /// </summary>
    public static class EasyStream
    {
        /// <summary>
        /// Easy stream reader.
        /// </summary>
        public static class Reader
        {
            /// <summary>
            /// Gets stream text.
            /// </summary>
            /// <param name="stream">The stream.</param>
            /// <returns>Stream text.</returns>
            public static string GetText(Stream stream)
            {
                stream.Position = 0;
                using (StreamReader sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }

            /// <summary>
            /// Gets file content text.
            /// </summary>
            /// <param name="filePath">The file path.</param>
            /// <returns>
            /// File content text.
            /// </returns>
            public static string GetText(string filePath)
            {
                using (Stream fs = new FileStream(filePath, FileMode.Open))
                {
                    return GetText(fs);
                }
            }

            /// <summary>
            /// Gets stream bytes.
            /// </summary>
            /// <param name="stream">The stream.</param>
            /// <param name="count">The bytes count.</param>
            /// <returns>
            /// Stream bytes.
            /// </returns>
            public static byte[] GetBytes(Stream stream, int? count = null)
            {
                stream.Position = 0;

                if (count.HasValue)
                {
                    using (BinaryReader br = new BinaryReader(stream))
                    {
                        return br.ReadBytes(count.Value);
                    }
                }

                IList<byte> bytes = new List<byte>();

                int b = 0;
                while ((b = stream.ReadByte()) > -1)
                {
                    bytes.Add(Convert.ToByte(b));
                }

                return bytes.ToArray();

            }

            /// <summary>
            /// Gets file content bytes.
            /// </summary>
            /// <param name="filePath">The file path.</param>
            /// <param name="count">The bytes count.</param>
            /// <returns>
            /// File content bytes.
            /// </returns>
            public static byte[] GetBytes(string filePath, int? count = null)
            {
                using (Stream fs = new FileStream(filePath, FileMode.Open))
                {
                    return GetBytes(fs, count);
                }
            }

            /// <summary>
            /// Gets stream text lines.
            /// </summary>
            /// <param name="stream">The stream.</param>
            /// <returns>Stream text lines</returns>
            public static string[] GetLines(Stream stream)
            {
                stream.Position = 0;
                IList<string> lines = new List<string>();
                using (StreamReader sr = new StreamReader(stream))
                {
                    if (sr.Peek() > -1)
                        lines.Add(sr.ReadLine());
                }

                return lines.ToArray();
            }

            /// <summary>
            /// Gets file content text lines.
            /// </summary>
            /// <param name="filePath">The file path.</param>
            /// <returns>
            /// File content text lines
            /// </returns>
            public static string[] GetLines(string filePath)
            {
                using (Stream fs = new FileStream(filePath, FileMode.Open))
                {
                    return GetLines(fs);
                }
            }
        }

        /// <summary>
        /// Easy stream writer.
        /// </summary>
        public static class Writer
        {
            /// <summary>
            /// Saves bytes to stream.
            /// </summary>
            /// <param name="stream">The stream.</param>
            /// <param name="bytes">The bytes.</param>
            public static void Save(Stream stream, byte[] bytes)
            {
                stream.Write(bytes, 0, bytes.Length);
            }

            /// <summary>
            /// Saves text to stream.
            /// </summary>
            /// <param name="stream">The stream.</param>
            /// <param name="text">The text.</param>
            public static void Save(Stream stream, string text)
            {
                using (StreamWriter sw = new StreamWriter(stream))
                {
                    sw.Write(text);
                }
            }

            /// <summary>
            /// Saves the content to a new file.
            /// </summary>
            /// <param name="content">The content.</param>
            /// <param name="filePath">The file path.</param>
            public static void SaveToFile(byte[] content, string filePath)
            {
                using (Stream fs = new FileStream(filePath, FileMode.Create))
                {
                    Save(fs, content);
                }
            }

            /// <summary>
            /// Saves the content to a new file.
            /// </summary>
            /// <param name="content">The content.</param>
            /// <param name="filePath">The file path.</param>
            public static void SaveToFile(string content, string filePath)
            {
                using (Stream fs = new FileStream(filePath, FileMode.Create))
                {
                    Save(fs, content);
                }
            }

            /// <summary>
            /// Appends the content to file.
            /// </summary>
            /// <param name="content">The content.</param>
            /// <param name="filePath">The file path.</param>
            public static void AppendToFile(byte[] content, string filePath)
            {
                using (Stream fs = new FileStream(filePath, FileMode.Append))
                {
                    Save(fs, content);
                }
            }

            /// <summary>
            /// Appends the content to file.
            /// </summary>
            /// <param name="content">The content.</param>
            /// <param name="filePath">The file path.</param>
            public static void AppendToFile(string content, string filePath)
            {
                using (Stream fs = new FileStream(filePath, FileMode.Append))
                {
                    Save(fs, content);
                }
            }
        }
    }
}
