using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace Radial.Param
{
    /// <summary>
    /// Repersents param data contract.
    /// </summary>
    [DataContract]
    public class ParamObject
    {
        string _path;
        string _description;
        string _value;

        #region Path Helpers

        /// <summary>
        /// The path separator.
        /// </summary>
        public const string PathSeparator = ".";
        /// <summary>
        /// The path regex pattern
        /// </summary>
        public const string PathRegexPattern = @"^([A-Za-z0-9_-]\.?)+$";

        /// <summary>
        /// The Xml namespace.
        /// </summary>
        public const string XmlNs = "urn:radial-xmlparam";

        /// <summary>
        /// Determine whether the specified path is valid.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        ///   <c>true</c> if the specified path is valid; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPathValid(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return false;

            path = path.Trim();
            Regex reg = new Regex(PathRegexPattern);

            return reg.IsMatch(path);
        }

        /// <summary>
        /// Normalize the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>
        /// The path after process.
        /// </returns>
        public static string NormalizePath(string path)
        {
            Checker.Requires(IsPathValid(path), "invalid path: \"{0}\"", path);

            return path.Trim().TrimEnd(Convert.ToChar(PathSeparator)).ToLower();
        }

        /// <summary>
        /// Get the parent path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>Returns parent path if exists, otherwise return String.Empty.</returns>
        public static string GetParentPath(string path)
        {
            path = NormalizePath(path);

            string parentPath = string.Empty;

            //not S_Root find its parent
            int parentSpiltIndex = path.LastIndexOf(Convert.ToChar(PathSeparator));
            if (parentSpiltIndex > 0)
                parentPath = path.Substring(0, parentSpiltIndex);

            return parentPath;
        }

        /// <summary>
        /// Gets the name of the param.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns></returns>
        public static string GetParamName(string path)
        {
            path = NormalizePath(path);

            if (!path.Contains(PathSeparator))
                return path;
            return path.Substring(path.LastIndexOf(PathSeparator) + 1);
        }

        /// <summary>
        /// Build param path depends on path levels.
        /// </summary>
        /// <param name="levels">The path levels.</param>
        /// <returns>A param path.</returns>
        public static string BuildPath(params string[] levels)
        {
            return NormalizePath(string.Join<string>(PathSeparator, levels));
        }

        #endregion

        /// <summary>
        /// Gets or sets param path.
        /// </summary>
        [DataMember]
        [JsonProperty("path")]
        public string Path
        {
            get { return _path; }
            set { _path = NormalizePath(value); }
        }

        /// <summary>
        /// Gets or sets param description.
        /// </summary>
        [DataMember]
        [JsonProperty("description")]
        public string Description
        {
            get { return _description; }
            set { _description = value == null ? value : value.Trim(); }
        }

        /// <summary>
        /// Gets or sets param value.
        /// </summary>
        [DataMember]
        [JsonProperty("value")]
        public string Value
        {
            get { return _value; }
            set { _value = value == null ? value : value.Trim(); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether contains next level(descendant) param objects.
        /// </summary>
        /// <value>
        ///   <c>true</c> if contains next level(descendant) param objects; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        [JsonProperty("has_next")]
        public bool HasNext
        { get; set; }
    }
}
