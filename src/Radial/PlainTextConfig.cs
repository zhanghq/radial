using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;


namespace Radial
{
    /// <summary>
    /// Plain text configuration.
    /// </summary>
    public class PlainTextConfig
    {
        /*
         * property line example:
         * style 1: abc=34ax
         * style 2: abc="dfs"
         * 
         * comment line start with #, example:
         * #some comment
         * abc=3
         */


        const string PropertyLineMatchPattern1 = "^([\\w_.-]+)\\s*=\\s*([^\"\'\\s#=]*)$";
        const string PropertyLineMatchPattern2 = "^([\\w_.-]+)\\s*=\\s*\"(.*)\"$";
        const string PropertyLineMatchPattern3 = "^([\\w_.-]+)\\s*=\\s*\'(.*)\'$";

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
                Regex reg3 = new Regex(PropertyLineMatchPattern3);

                foreach (string line in textLines)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string key = string.Empty;
                    string value = string.Empty;

                    GroupCollection groups = null;

                    if (reg1.IsMatch(line.Trim()))
                        groups = reg1.Match(line.Trim()).Groups;
                    else if (reg2.IsMatch(line.Trim()))
                        groups = reg2.Match(line.Trim()).Groups;
                    else if (reg3.IsMatch(line.Trim()))
                        groups = reg3.Match(line.Trim()).Groups;

                    if (groups != null && groups.Count == 3)
                    {
                        key = groups[1].Value.Trim().ToLower();
                        value = groups[2].Value.Trim();
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
                return new PlainTextConfig(text.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries));
            return new PlainTextConfig();
        }


        /// <summary>
        /// Determines whether contains the specified property name.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>
        ///   <c>true</c> if contains the specified property name; otherwise, <c>false</c>.
        /// </returns>
        public bool Contains(string name)
        {
            Checker.Parameter(!string.IsNullOrEmpty(name), "property name can not be empty or null");

            return _entries.ContainsKey(name.ToLower().Trim());
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>
        /// If property name exist, return its value, otherwise return null.
        /// </returns>
        public string this[string name]
        {
            get
            {
                return GetValue(name);
            }
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="name">The property name.</param>
        /// <returns>
        /// If property name exist, return its value, otherwise return null.
        /// </returns>
        public string GetValue(string name)
        {
            Checker.Parameter(!string.IsNullOrEmpty(name), "property name can not be empty or null");

            name = name.ToLower().Trim();

            if (!_entries.ContainsKey(name))
                return null;

            return _entries[name];
        }
    }
}
