using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Radial.Net
{
    /// <summary>
    /// The Http file form data. 
    /// </summary>
    public sealed class FileFormData : IMultipartFormData
    {
        /// <summary>
        /// The default ContentType
        /// </summary>
        public const string DefaultContentType = "application/octet-stream";

        string _paramName;
        byte[] _fileContent;
        string _fileName;
        string _contentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="paramName">The post parameter name.</param>
        public FileFormData(string filePath, string paramName) : this(filePath, paramName, string.Empty) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="paramName">The post parameter name.</param>
        /// <param name="contentType">The post content type.</param>
        public FileFormData(string filePath, string paramName, string contentType) : this(filePath, paramName, contentType, Encoding.UTF8) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <param name="paramName">The post parameter name.</param>
        /// <param name="contentType">The post content type.</param>
        /// <param name="encoding">The encoding of post data bytes.</param>
        public FileFormData(string filePath, string paramName, string contentType, Encoding encoding)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(filePath), "file path can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(paramName), "post parameter name can not be empty or null");
            Checker.Parameter(encoding != null, "the encoding of post data bytes can not be null");

            Checker.Requires(File.Exists(filePath), "file not exists:{0}", filePath);

            _fileName = Path.GetFileName(filePath);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                int b = -1;
                List<byte> fcbList = new List<byte>();
                while ((b = fileStream.ReadByte()) > -1)
                {
                    fcbList.Add((byte)b);
                }
                _fileContent = fcbList.ToArray();
            }

            _paramName = paramName.Trim();
            if (!string.IsNullOrEmpty(contentType))
                _contentType = contentType.Trim();
            else
                _contentType = DefaultContentType;

            Encoding = encoding;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="fileStream">The file stream.</param>
        /// <param name="paramName">The post parameter name.</param>
        public FileFormData(Stream fileStream, string paramName) : this(string.Empty, fileStream, paramName) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="fileName">The post file name.</param>
        /// <param name="fileStream">The post file stream.</param>
        /// <param name="paramName">The post parameter name.</param>
        public FileFormData(string fileName, Stream fileStream, string paramName) : this(fileName, fileStream, paramName, string.Empty) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="fileName">The post file name.</param>
        /// <param name="fileStream">The post file stream.</param>
        /// <param name="paramName">The post parameter name.</param>
        /// <param name="contentType">The post content type.</param>
        public FileFormData(string fileName, Stream fileStream, string paramName, string contentType) : this(fileName, fileStream, paramName, contentType, Encoding.UTF8) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="fileName">The post file name.</param>
        /// <param name="fileStream">The post file stream.</param>
        /// <param name="paramName">The post parameter name.</param>
        /// <param name="contentType">The post content type.</param>
        /// <param name="encoding">The encoding of post data bytes.</param>
        public FileFormData(string fileName, Stream fileStream, string paramName, string contentType, Encoding encoding)
        {
            Checker.Parameter(fileStream != null, "post file stream can not be null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(paramName), "post parameter name can not be empty or null");
            Checker.Parameter(encoding != null, "the encoding of post data bytes can not be null");

            if (!string.IsNullOrEmpty(fileName))
                _fileName = fileName.Trim();

            using (fileStream)
            {
                int b = -1;
                List<byte> fcbList = new List<byte>();
                while ((b = fileStream.ReadByte()) > -1)
                {
                    fcbList.Add((byte)b);
                }
                _fileContent = fcbList.ToArray();
            }

            _paramName = paramName.Trim();
            if (!string.IsNullOrEmpty(contentType))
                _contentType = contentType.Trim();
            else
                _contentType = DefaultContentType;

            Encoding = encoding;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="fileContent">The post file content.</param>
        /// <param name="paramName">The post parameter name.</param>
        public FileFormData(byte[] fileContent, string paramName)
            : this(string.Empty, fileContent, paramName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="fileName">The post file name.</param>
        /// <param name="fileContent">The post file content.</param>
        /// <param name="paramName">The post parameter name.</param>
        public FileFormData(string fileName, byte[] fileContent, string paramName)
            : this(fileName, fileContent, paramName, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="fileName">The post file name.</param>
        /// <param name="fileContent">The post file content.</param>
        /// <param name="paramName">The post parameter name.</param>
        /// <param name="contentType">The post file content type.</param>
        public FileFormData(string fileName, byte[] fileContent, string paramName, string contentType)
            : this(fileName, fileContent, paramName, contentType, Encoding.UTF8)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="FileFormData"/> class.
        /// </summary>
        /// <param name="fileName">The post file name.</param>
        /// <param name="fileContent">The post file content.</param>
        /// <param name="paramName">The post parameter name.</param>
        /// <param name="contentType">The post file content type.</param>
        /// <param name="encoding">The encoding of post data bytes.</param>
        public FileFormData(string fileName, byte[] fileContent, string paramName, string contentType, Encoding encoding)
        {
            Checker.Parameter(fileContent != null, "post file content can not be null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(paramName), "post parameter name can not be empty or null");
            Checker.Parameter(encoding != null, "the encoding of post data bytes can not be null");

            if (!string.IsNullOrWhiteSpace(fileName))
                _fileName = fileName.Trim();

            _fileContent = fileContent;
            _paramName = paramName.Trim();

            if (!string.IsNullOrEmpty(contentType))
                _contentType = contentType.Trim();
            else
                _contentType = DefaultContentType;

            Encoding = encoding;
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
        /// Gets or sets the encoding of post data bytes.
        /// </summary>
        public Encoding Encoding
        {
            get;
            set;
        }

        /// <summary>
        /// Write data to the request stream
        /// </summary>
        /// <param name="reqStream">The request stream.</param>
        public void Write(Stream reqStream)
        {
            Checker.Parameter(reqStream != null, "the request stream can not be null");

            StringBuilder sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(_fileName))
                sb.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", _paramName, _fileName));
            else
                sb.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\";", _paramName));
            sb.AppendLine(string.Format("Content-Type: {0}", _contentType));

            sb.AppendLine();

            byte[] postBytesHeader = Encoding.GetBytes(sb.ToString());
            reqStream.Write(postBytesHeader, 0, postBytesHeader.Length);
            reqStream.Write(_fileContent, 0, _fileContent.Length);
            byte[] postBytesEnd = Encoding.GetBytes("\r\n");
            reqStream.Write(postBytesEnd, 0, postBytesEnd.Length);
        }
    }
}
