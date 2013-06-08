using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;


namespace Radial
{
    /// <summary>
    /// Plain text configuration.
    /// </summary>
    public class PlainTextConfig
    {
        /*
         * property format example:
         * style 1: abc=34ax
         * style 2: abc="dfs"
         * comment text start with # (Unix shell style) eg. # some comment
         */
        

        const string PropertyLineMatchPattern1 = "^([\\w_.-]*)\\s*=([^\"\'#=]*)$";
        const string PropertyLineMatchPattern2 = "^([\\w_.-]*)\\s*=\\s*[\"\'](.*)[\"\']$";

        readonly IDictionary<string, string> _entries;

        /// <summary>
        /// Prevents a default instance of the <see cref="PlainTextConfig" /> class from being created.
        /// </summary>
        /// <param name="textLines">The text lines.</param>
        private PlainTextConfig(params string[] textLines)
        {
            _entries = new Dictionary<string, string>();

            if (textLines.Length > 0)
            {
                Regex reg1 = new Regex(PropertyLineMatchPattern1);
                Regex reg2 = new Regex(PropertyLineMatchPattern2);

                foreach (string line in textLines)
                {
                    string key = string.Empty;
                    string value = string.Empty;

                    GroupCollection groups = null;

                    if (reg1.IsMatch(line.Trim()))
                        groups = reg1.Match(line.Trim()).Groups;
                    else if (reg2.IsMatch(line.Trim()))
                        groups = reg2.Match(line.Trim()).Groups;

                    if (groups != null && groups.Count == 3)
                    {
                        key = groups[1].Value.Trim().ToLower();
                        value = groups[2].Value.Trim().ToLower();
                    }

                    if (!string.IsNullOrWhiteSpace(key) && !_entries.ContainsKey(key))
                        _entries.Add(key, value);
                }
            }
        }

        /// <summary>
        /// Loads configuration from file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static PlainTextConfig LoadFromFile(string filePath)
        {
            if (File.Exists(filePath))
                return new PlainTextConfig(File.ReadAllLines(filePath));
            return new PlainTextConfig();
        }

        /// <summary>
        /// Loads configuration from text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        public static PlainTextConfig LoadFromText(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
                return new PlainTextConfig(text.Split('\n', '\r'));
            return new PlainTextConfig();
        }


        /// <summary>
        /// Determines whether contains the specified property.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        ///   <c>true</c> if contains the specified property; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string property)
        {
            Checker.Parameter(!string.IsNullOrEmpty(property), "property can not be empty or null");

            property = property.ToLower().Trim();

            return _entries.ContainsKey(property);
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="property">The property.</param>
        /// <returns>
        /// If property exist, return its value or String.Empty, otherwise return null.
        /// </returns>
        public string GetValue(string property)
        {
            Checker.Parameter(!string.IsNullOrEmpty(property), "property can not be empty or null");

            property = property.ToLower().Trim();

            if (!_entries.ContainsKey(property))
                return null;

            return _entries[property];
        }
    }
}
