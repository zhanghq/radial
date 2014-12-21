using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.IO
{
    /// <summary>
    /// StreamKits.
    /// </summary>
    public static class StreamKits
    {
        /// <summary>
        /// Gets stream text.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// Stream text.
        /// </returns>
        public static string GetText(Stream stream)
        {
            return GetText(stream, StaticVariables.Encoding);
        }

        /// <summary>
        /// Gets stream text.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// Stream text.
        /// </returns>
        public static string GetText(Stream stream, Encoding encoding)
        {
            stream.Position = 0;
            using (StreamReader sr = new StreamReader(stream, encoding))
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
            return GetText(filePath, StaticVariables.Encoding);
        }

        /// <summary>
        /// Gets file content text.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// File content text.
        /// </returns>
        public static string GetText(string filePath, Encoding encoding)
        {
            using (Stream fs = new FileStream(filePath, FileMode.Open))
            {
                return GetText(fs, encoding);
            }
        }

        /// <summary>
        /// Gets stream bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="count">The bytes count.</param>
        /// <returns>Stream bytes.</returns>
        public static byte[] GetBytes(Stream stream, int offset, int count)
        {
            stream.Position = 0;

            byte[] buffer = new byte[] { };

            stream.Read(buffer, offset, count);

            return buffer;
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
        /// <param name="offset">The offset.</param>
        /// <param name="count">The bytes count.</param>
        /// <returns>
        /// File content bytes.
        /// </returns>
        public static byte[] GetBytes(string filePath, int offset, int count)
        {
            using (Stream fs = new FileStream(filePath, FileMode.Open))
            {
                return GetBytes(fs, offset, count);
            }
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
            return GetLines(stream, StaticVariables.Encoding);
        }

        /// <summary>
        /// Gets stream text lines.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// Stream text lines
        /// </returns>
        public static string[] GetLines(Stream stream, Encoding encoding)
        {
            stream.Position = 0;
            IList<string> lines = new List<string>();
            using (StreamReader sr = new StreamReader(stream, encoding))
            {
                while (sr.Peek() > -1)
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
            return GetLines(filePath, StaticVariables.Encoding);
        }

        /// <summary>
        /// Gets file content text lines.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="encoding">The encoding.</param>
        /// <returns>
        /// File content text lines
        /// </returns>
        public static string[] GetLines(string filePath, Encoding encoding)
        {
            using (Stream fs = new FileStream(filePath, FileMode.Open))
            {
                return GetLines(fs, encoding);
            }
        }

        /// <summary>
        /// Saves the content to a file using FileMode.OpenOrCreate.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="filePath">The file path.</param>
        public static void SaveToFile(byte[] content, string filePath)
        {
            SaveToFile(content, filePath, StaticVariables.Encoding);
        }

        /// <summary>
        /// Saves the content to a file using FileMode.OpenOrCreate.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="encoding">The encoding.</param>
        public static void SaveToFile(byte[] content, string filePath, Encoding encoding)
        {
            if (content == null || content.Length == 0)
                return;

            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (Stream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            using (BinaryWriter bw = new BinaryWriter(fs, encoding))
            {
                bw.Write(content);
            }
        }

        /// <summary>
        /// Saves the content to a file using FileMode.OpenOrCreate.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="filePath">The file path.</param>
        public static void SaveToFile(string content, string filePath)
        {
            SaveToFile(content, filePath, StaticVariables.Encoding);
        }

        /// <summary>
        /// Saves the content to a file using FileMode.OpenOrCreate.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="encoding">The encoding.</param>
        public static void SaveToFile(string content, string filePath, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;

            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (Stream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            using (TextWriter tw = new StreamWriter(fs, encoding))
            {
                tw.Write(content);
            }
        }

        /// <summary>
        /// Appends the content to file.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="filePath">The file path.</param>
        public static void AppendToFile(byte[] content, string filePath)
        {
            AppendToFile(content, filePath, StaticVariables.Encoding);
        }

        /// <summary>
        /// Appends the content to file.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="encoding">The encoding.</param>
        public static void AppendToFile(byte[] content, string filePath, Encoding encoding)
        {
            if (content == null || content.Length == 0)
                return;

            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (Stream fs = new FileStream(filePath, FileMode.Append))
            using (BinaryWriter bw = new BinaryWriter(fs, encoding))
            {
                bw.Write(content);
            }
        }

        /// <summary>
        /// Appends the content to file.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="filePath">The file path.</param>
        public static void AppendToFile(string content, string filePath)
        {
            AppendToFile(content, filePath, StaticVariables.Encoding);
        }

        /// <summary>
        /// Appends the content to file.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="encoding">The encoding.</param>
        public static void AppendToFile(string content, string filePath, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(content))
                return;

            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (Stream fs = new FileStream(filePath, FileMode.Append))
            using (TextWriter tw = new StreamWriter(fs, encoding))
            {
                tw.Write(content);
            }
        }


        /// <summary>
        /// Appends line to file.
        /// </summary>
        /// <param name="line">The line content.</param>
        /// <param name="filePath">The file path.</param>
        public static void AppendLine(string line, string filePath)
        {
            AppendLine(line, filePath, StaticVariables.Encoding);
        }

        /// <summary>
        /// Appends line to file.
        /// </summary>
        /// <param name="line">The line content.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="encoding">The encoding.</param>
        public static void AppendLine(string line, string filePath, Encoding encoding)
        {
            if (string.IsNullOrWhiteSpace(line))
                return;

            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (Stream fs = new FileStream(filePath, FileMode.Append))
            using (TextWriter tw = new StreamWriter(fs, encoding))
            {
                tw.WriteLine(line);
            }
        }

        /// <summary>
        /// Appends lines to file.
        /// </summary>
        /// <param name="lines">The lines content.</param>
        /// <param name="filePath">The file path.</param>
        public static void AppendLines(IEnumerable<string> lines, string filePath)
        {
            AppendLines(lines, filePath, StaticVariables.Encoding);
        }

        /// <summary>
        /// Appends lines to file.
        /// </summary>
        /// <param name="lines">The lines content.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="encoding">The encoding.</param>
        public static void AppendLines(IEnumerable<string> lines, string filePath, Encoding encoding)
        {
            if (lines == null || lines.Count() == 0)
                return;

            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            using (Stream fs = new FileStream(filePath, FileMode.Append))
            using (TextWriter tw = new StreamWriter(fs, encoding))
            {
                foreach (var line in lines)
                    tw.WriteLine(line);
            }
        }

        /// <summary>
        /// Truncate file.
        /// </summary>
        /// <param name="filePath"></param>
        public static void TruncateFile(string filePath)
        {
            if (!File.Exists(filePath))
                return;

            Stream fs = new FileStream(filePath, FileMode.Truncate);
            fs.Close();
        }
    }
}
