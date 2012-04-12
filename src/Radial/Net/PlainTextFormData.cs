using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Net
{
    /// <summary>
    /// The Http plain text form data. 
    /// </summary>
    public sealed class PlainTextFormData : IMultipartFormData
    {
        string _paramName;
        string _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainTextFormData"/> class.
        /// </summary>
        /// <param name="paramName">The post parameter name.</param>
        /// <param name="value">The post parameter value.</param>
        public PlainTextFormData(string paramName, string value)
            : this(paramName, value, Encoding.UTF8)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlainTextFormData"/> class.
        /// </summary>
        /// <param name="paramName">The post parameter name.</param>
        /// <param name="value">The post parameter value.</param>
        /// <param name="encoding">The encoding of post data bytes.</param>
        public PlainTextFormData(string paramName, string value, Encoding encoding)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(paramName), "post parameter name can not be empty or null");
            Checker.Parameter(encoding != null, "the encoding of post data bytes can not be null");
            _paramName = paramName.Trim();
            _value = value;
            Encoding = encoding;
        }


        /// <summary>
        /// Write data to the request stream
        /// </summary>
        /// <param name="reqStream">The request stream.</param>
        public void Write(System.IO.Stream reqStream)
        {
            Checker.Parameter(reqStream != null, "the request stream can not be null");

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(string.Format("Content-Disposition: form-data; name=\"{0}\"", _paramName));
            sb.AppendLine();
            sb.AppendLine(_value);

            byte[] postBytes = Encoding.GetBytes(sb.ToString());
            reqStream.Write(postBytes, 0, postBytes.Length);
        }

        /// <summary>
        /// Gets the name of the post parameter.
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
        }

        /// <summary>
        /// Gets or sets the encoding of post data bytes.
        /// </summary>
        public Encoding Encoding { get; set; }
    }
}
