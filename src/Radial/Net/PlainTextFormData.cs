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
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(paramName), "post parameter name can not be empty or null");
            _paramName = paramName.Trim();
            _value = value;
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

            byte[] postBytes = StaticVariables.Encoding.GetBytes(sb.ToString());
            reqStream.Write(postBytes, 0, postBytes.Length);
        }

        /// <summary>
        /// Gets the name of the post parameter.
        /// </summary>
        public string ParamName
        {
            get { return _paramName; }
        }
    }
}
