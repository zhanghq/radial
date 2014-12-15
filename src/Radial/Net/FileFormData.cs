using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Radial.IO;

namespace Radial.Net
{
    /// <summary>
    /// The Http file form data. 
    /// </summary>
    public sealed class FileFormData : IMultipartFormData
    {
        string _paramName;
        byte[] _fileContent;
        string _fileName;
        string _fileContentType;


        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public FileFormData(string filePath, string paramName) : this(filePath, paramName, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData" /> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="paramName">The post parameter name.</param>
        /// <param name="fileContentType">Type of the file content.</param>
        public FileFormData(string filePath, string paramName, string fileContentType)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(filePath), "file path can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(paramName), "post parameter name can not be empty or null");

            Checker.Requires(File.Exists(filePath), "file not exists:{0}", filePath);

            _fileName = Path.GetFileName(filePath);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                _fileContent = GetBytes(fileStream);
            }

            _paramName = paramName.Trim();

            if (!string.IsNullOrWhiteSpace(fileContentType))
                _fileContentType = fileContentType.Trim();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData" /> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public FileFormData(Stream fileStream, string paramName) : this(null, fileStream, paramName, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="paramName">Name of the parameter.</param>
        public FileFormData(string fileName, Stream fileStream, string paramName) : this(fileName, fileStream, paramName, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData" /> class.
        /// </summary>
        /// <param name="fileName">The post file name.</param>
        /// <param name="fileStream">The post file stream.</param>
        /// <param name="paramName">The post parameter name.</param>
        /// <param name="fileContentType">Type of the file content.</param>
        public FileFormData(string fileName, Stream fileStream, string paramName, string fileContentType)
        {
            Checker.Parameter(fileStream != null, "post file stream can not be null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(paramName), "post parameter name can not be empty or null");

            if (!string.IsNullOrEmpty(fileName))
                _fileName = fileName.Trim();

            using (fileStream)
            {
                _fileContent = GetBytes(fileStream);
            }

            _paramName = paramName.Trim();

            if (!string.IsNullOrWhiteSpace(fileContentType))
                _fileContentType = fileContentType.Trim();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData" /> class.
        /// </summary>
        /// <param name="fileContent">The post file content.</param>
        /// <param name="paramName">The post parameter name.</param>
        public FileFormData(byte[] fileContent, string paramName)
            : this(null, fileContent, paramName, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData" /> class.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="fileContent">The post file content.</param>
        /// <param name="paramName">The post parameter name.</param>
        public FileFormData(string fileName, byte[] fileContent, string paramName)
            : this(fileName, fileContent, paramName, null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData" /> class.
        /// </summary>
        /// <param name="fileName">The post file name.</param>
        /// <param name="fileContent">The post file content.</param>
        /// <param name="paramName">The post parameter name.</param>
        /// <param name="fileContentType">Type of the file content.</param>
        public FileFormData(string fileName, byte[] fileContent, string paramName, string fileContentType)
        {
            Checker.Parameter(fileContent != null, "post file content can not be null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(paramName), "post parameter name can not be empty or null");

            if (!string.IsNullOrWhiteSpace(fileName))
                _fileName = fileName.Trim();

            _fileContent = fileContent;
            _paramName = paramName.Trim();

            if (!string.IsNullOrWhiteSpace(fileContentType))
                _fileContentType = fileContentType.Trim();
        }

        /// <summary>
        /// Gets the name of the post parameter.
        /// </summary>
        public string ParamName
        {
            get
            {
                return _paramName;
            }
        }

        /// <summary>
        /// Gets stream bytes.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns>
        /// Stream bytes.
        /// </returns>
        private byte[] GetBytes(Stream stream)
        {
            stream.Position = 0;

            IList<byte> bytes = new List<byte>();

            int b = 0;
            while ((b = stream.ReadByte()) > -1)
            {
                bytes.Add(Convert.ToByte(b));
            }

            return bytes.ToArray();

        }

        /// <summary>
        /// Write data to the request stream
        /// </summary>
        /// <param name="reqStream">The request stream.</param>
        public void Write(Stream reqStream)
        {
            Checker.Parameter(reqStream != null, "the request stream can not be null");

            StringBuilder sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(_fileName))
                _fileName = Guid.NewGuid().ToString("n");

            sb.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", _paramName, _fileName));

            if (string.IsNullOrWhiteSpace(_fileContentType))
                _fileContentType = ContentTypes.BinaryStream;

            sb.AppendLine(string.Format("Content-Type: {0}", _fileContentType));

            sb.AppendLine();

            byte[] postBytesHeader = StaticVariables.Encoding.GetBytes(sb.ToString());
            reqStream.Write(postBytesHeader, 0, postBytesHeader.Length);
            reqStream.Write(_fileContent, 0, _fileContent.Length);
            byte[] postBytesEnd = StaticVariables.Encoding.GetBytes("\r\n");
            reqStream.Write(postBytesEnd, 0, postBytesEnd.Length);
        }
    }
}
