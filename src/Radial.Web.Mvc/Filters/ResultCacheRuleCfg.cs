using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Radial.Web.Mvc.Filters
{
    /// <summary>
    /// Action result cache rule configuration class.
    /// </summary>
    public sealed class ResultCacheRuleCfg
    {
        static object SyncRoot = new object();
        static HashSet<ResultCacheRule> s_rules = new HashSet<ResultCacheRule>();
        /// <summary>
        /// The Xml namespace.
        /// </summary>
        const string XmlNs = "urn:radial-web-mvc-result-cache-rule";

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultCacheRuleCfg"/> class.
        /// </summary>
        static ResultCacheRuleCfg()
        {
            Initial(ConfigurationPath);
            FileWatcher.CreateMonitor(ConfigurationPath, Initial);
        }


        /// <summary>
        /// Gets the configuration path.
        /// </summary>
        private static string ConfigurationPath
        {
            get
            {
                return SystemSettings.GetConfigPath("ResultCache.config");
            }
        }

        /// <summary>
        /// Builds the name with xmlns.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private static XName BuildXName(string name)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(name), "name can not be empty or null");
            XNamespace ns = XmlNs;
            return ns + name;
        }

        /// <summary>
        /// Initials the specified config file path.
        /// </summary>
        /// <param name="configFilePath">The config file path.</param>
        private static void Initial(string configFilePath)
        {
            lock (SyncRoot)
            {
                s_rules.Clear();

                if (File.Exists(ConfigurationPath))
                {
                    var doc = XDocument.Load(configFilePath);

                    foreach (var e in doc.Descendants(BuildXName("add")))
                    {
                        var eau = e.Attribute("url");
                        var eag = e.Attribute("groups");
                        var eae = e.Attribute("expires");
                        var eac = e.Attribute("ignoreCase");

                        string url = eau != null ? eau.Value : null;
                        bool ignoreCase = true;

                        if (eac != null&&!string.IsNullOrWhiteSpace(eac.Value))
                        {
                            if (eac.Value.Trim() == "0" || eac.Value.Trim().ToLower() == "false")
                                ignoreCase = false;
                        }

                        string groups = eag != null ? eag.Value : null;
                        TimeSpan expires;

                        ResultCacheRule rule = null;

                        if (eae != null && TimeSpan.TryParse(eae.Value, out expires))
                            rule = new ResultCacheRule(url, ignoreCase, groups.Split(','), expires);
                        else
                            rule = new ResultCacheRule(url, ignoreCase, groups.Split(','));

                        if (!s_rules.Contains(rule))
                            s_rules.Add(rule);
                    }
                }
            }
        }


        /// <summary>
        /// Gets the matched staticize rule.
        /// </summary>
        /// <param name="requestUrl">The request URL.</param>
        /// <returns></returns>
        public static ResultCacheRule GetMatchedRule(string requestUrl)
        {
            if (string.IsNullOrWhiteSpace(requestUrl))
                return null;

            requestUrl = HttpKits.MakeRelativeUrl(requestUrl).TrimStart('/');

            lock (SyncRoot)
            {
                foreach (var o in s_rules)
                {
                    if (string.Compare(o.Url, requestUrl, o.IgnoreCase) == 0)
                        return o;

                    Regex reg = new Regex(o.Url, o.IgnoreCase ? RegexOptions.IgnoreCase : RegexOptions.None);

                    if (reg.IsMatch(requestUrl))
                        return o;
                }
            }

            return null;
        }


        /// <summary>
        /// Gets the rules.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        public static ResultCacheRule[] GetRules(params string[] groups)
        {
            IList<string> newGroups = new List<string>();
            if (groups != null)
            {
                for (int i = 0; i < groups.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(groups[i]))
                        newGroups.Add(groups[i].Trim().ToLower());
                }
            }

            lock (SyncRoot)
            {
                if (newGroups.Count == 0)
                    return s_rules.ToArray();

                HashSet<ResultCacheRule> aset = new HashSet<ResultCacheRule>();

                foreach (var o in s_rules)
                {
                    if (o.Groups.Any(g => newGroups.Contains(g)))
                        aset.Add(o);
                }

                return aset.ToArray();
            }
        }
    }
}
