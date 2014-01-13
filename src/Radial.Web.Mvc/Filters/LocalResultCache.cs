using Radial.IO;
using Radial.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Radial.Web.Mvc.Filters
{
    /// <summary>
    /// Local result cache.
    /// </summary>
    public class LocalResultCache : IResultCacheable
    {
        /// <summary>
        /// Static cache data base directory relative path.
        /// </summary>
        const string BaseDirectoryRelativePath = "~/scd";

        /// <summary>
        /// Gets the data encoding.
        /// </summary>
        public Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }

        /// <summary>
        /// Determines whether the request context matched the conditions which generate cache data.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <returns>
        /// If matched return true, otherwise return false.
        /// </returns>
        public bool IsMatched(System.Web.Routing.RequestContext context)
        {
            return GetMatchedRule(context) != null;
        }


        /// <summary>
        /// Retrieves the HTML content based on the request context.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <param name="html">The retrived HTML content.</param>
        /// <returns>
        /// If successfully retrieved return true, otherwise return false.
        /// </returns>
        public bool Get(System.Web.Routing.RequestContext context, out string html)
        {

            html = null;

            if (context.HttpContext.Request.HttpMethod != System.Net.Http.HttpMethod.Get.Method)
                return false;

            var rule = GetMatchedRule(context);

            if (rule == null)
                return false;

            try
            {
                string filePath = BuildCacheFilePhysicalPath(context.HttpContext.Request, rule);

                if (File.Exists(filePath))
                {
                    DateTime ctut = File.GetLastWriteTimeUtc(filePath);

                    if (ctut.Add(rule.Expires) >= DateTime.UtcNow)
                    {
                        html = EasyStream.Reader.GetText(filePath, Encoding);
                        return !string.IsNullOrWhiteSpace(html);
                    }
                }

            }
            catch (Exception ex)
            {
                Logger.Default.Error(ex);
            }

            return false;
        }


        /// <summary>
        /// Writes the HTML content to cache.
        /// </summary>
        /// <param name="context">The request context.</param>
        /// <param name="html">The HTML content.</param>
        public void Set(System.Web.Routing.RequestContext context, string html)
        {
            if (context.HttpContext.Request.HttpMethod != System.Net.Http.HttpMethod.Get.Method
                || string.IsNullOrWhiteSpace(html))
                return;

            var rule = GetMatchedRule(context);

            if (rule == null)
                return;

            try
            {
                string filePath = BuildCacheFilePhysicalPath(context.HttpContext.Request, rule);

                if (File.Exists(filePath))
                {
                    DateTime ctut = File.GetLastWriteTimeUtc(filePath);

                    if (ctut.Add(rule.Expires) > DateTime.UtcNow)
                        return;
                }

                PrepareCacheDirectory(rule);

                EasyStream.Writer.SaveToFile(html, filePath, Encoding);

            }
            catch (Exception ex)
            {
                Logger.Default.Error(ex);
            }
        }


        /// <summary>
        /// Removes the cache data by the specified url.
        /// </summary>
        /// <param name="url">The specified url.</param>
        public void Remove(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return;

            var rule = GetMatchedRule(url);

            if (rule == null)
                return;

            try
            {
                string bdpp = GetBaseDirectoryPhysicalPath();
                string cfn = CreateCacheFileName(url, rule.IgnoreCase);

                if (Directory.Exists(bdpp))
                {
                    foreach (string f in Directory.EnumerateFiles(bdpp, cfn, SearchOption.AllDirectories))
                        File.Delete(f);
                }
            }
            catch (Exception ex)
            {
                Logger.Default.Error(ex);
            }
        }

        /// <summary>
        /// Batch removes the cache data by group names, or clear all if no name is given.
        /// </summary>
        /// <param name="groups">The group names.</param>
        public void BatchRemove(params string[] groups)
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

            try
            {
                if (newGroups.Count == 0)
                {
                    string bdpp = GetBaseDirectoryPhysicalPath();

                    if (Directory.Exists(bdpp))
                        Directory.Delete(bdpp, true);

                    return;
                }

                ResultCacheRule[] rules = GetRules(groups);

                foreach (var r in rules)
                {
                    string dir = BuildCacheDirectoryPhysicalPath(r);

                    if (Directory.Exists(dir))
                        Directory.Delete(dir, true);
                }

            }
            catch (Exception ex)
            {
                Logger.Default.Error(ex);
            }
        }

        /// <summary>
        /// Gets the matched staticize rule.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        private ResultCacheRule GetMatchedRule(System.Web.Routing.RequestContext context)
        {
            return GetMatchedRule(context.HttpContext.Request.Url.PathAndQuery);
        }

        /// <summary>
        /// Gets the matched staticize rule.
        /// </summary>
        /// <param name="requestPathAndQuery">The request path and query.</param>
        /// <returns></returns>
        private ResultCacheRule GetMatchedRule(string requestPathAndQuery)
        {
            return ResultCacheRuleCfg.GetMatchedRule(requestPathAndQuery);
        }

        /// <summary>
        /// Gets the rules.
        /// </summary>
        /// <param name="groups">The groups.</param>
        /// <returns></returns>
        private ResultCacheRule[] GetRules(params string[] groups)
        {
            return ResultCacheRuleCfg.GetRules(groups);
        }

        /// <summary>
        /// Normalizes the relative URL.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        private string NormalizeRelativeUrl(string url, bool ignoreCase)
        {
            url = HttpKits.MakeRelativeUrl(url).TrimStart('/');

            //new query string
            string nqs = null;

            if (url.IndexOf('?') > 0)
            {
                var querys = HttpKits.ResolveParameters(url.Substring(url.IndexOf('?')));
                SortedDictionary<string, string> sd = new SortedDictionary<string, string>();

                foreach (string k in querys)
                {
                    var v = querys[k];
                    if (!string.IsNullOrWhiteSpace(v))
                        sd.Add(k, v.Trim());
                }

                IList<string> paramList = new List<string>();
                foreach (KeyValuePair<string, string> kv in sd)
                    paramList.Add(string.Format("{0}={1}", kv.Key, kv.Value));

                nqs = string.Join("&", paramList);
            }

            string newUrl = url.Split('?')[0];

            if (ignoreCase)
                newUrl = newUrl.ToLower();

            if (!string.IsNullOrWhiteSpace(nqs))
                newUrl += "?" + nqs;

            return newUrl;
        }


        /// <summary>
        /// Builds the cache directory relative path.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        private string BuildCacheDirectoryRelativePath(ResultCacheRule rule)
        {
            if (rule.Groups != null)
                return CryptoProvider.MD5Encrypt(string.Join("-", rule.Groups));

            return null;
        }


        /// <summary>
        /// Builds the cache directory physical path.
        /// </summary>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        private string BuildCacheDirectoryPhysicalPath(ResultCacheRule rule)
        {
            return HttpContext.Current.Server.MapPath(HttpKits.CombineRelativeUrl(BaseDirectoryRelativePath, BuildCacheDirectoryRelativePath(rule)));
        }


        /// <summary>
        /// Creates the name of the cache file.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="ignoreCase">if set to <c>true</c> [ignore case].</param>
        /// <returns></returns>
        private string CreateCacheFileName(string url, bool ignoreCase)
        {
            return CryptoProvider.MD5Encrypt(NormalizeRelativeUrl(url,ignoreCase));
        }

        /// <summary>
        /// Gets the base directory physical path.
        /// </summary>
        /// <returns></returns>
        private string GetBaseDirectoryPhysicalPath()
        {
            return HttpContext.Current.Server.MapPath(BaseDirectoryRelativePath);
        }

        /// <summary>
        /// Builds the cache file physical path.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="rule">The rule.</param>
        /// <returns></returns>
        private string BuildCacheFilePhysicalPath(HttpRequestBase request, ResultCacheRule rule)
        {
            string file = CreateCacheFileName(request.RawUrl, rule.IgnoreCase);
            return request.MapPath(HttpKits.CombineRelativeUrl(BaseDirectoryRelativePath, BuildCacheDirectoryRelativePath(rule), file));
        }

        private void PrepareCacheDirectory(ResultCacheRule rule)
        {
            string bpp = GetBaseDirectoryPhysicalPath();

            if (!Directory.Exists(bpp))
                Directory.CreateDirectory(bpp);

            string rp = BuildCacheDirectoryRelativePath(rule);

            if (string.IsNullOrWhiteSpace(rp))
                return;

            string[] dirs = rp.Split(Path.DirectorySeparatorChar);

            for (int i = 0; i < dirs.Length; i++)
            {
                IList<string> list = new List<string>();

                for (int j = 0; j <= i; j++)
                    list.Add(dirs[j]);

                string pp = HttpContext.Current.Server.MapPath(HttpKits.CombineRelativeUrl(BaseDirectoryRelativePath, string.Join("/", list)));

                if (!Directory.Exists(pp))
                    Directory.CreateDirectory(pp);
            }
        }

    }
}
