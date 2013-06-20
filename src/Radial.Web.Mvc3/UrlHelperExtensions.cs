using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Configuration;

namespace Radial.Web.Mvc
{
    /// <summary>
    /// UrlHelper extension class.
    /// </summary>
    public static class UrlHelperExtensions
    {
        /// <summary>
        /// Resource base url config name. 
        /// </summary>
        public const string ResourceBaseUrlConfigName = "ResourceBaseUrl";

        /// <summary>
        /// Converts a virtual (relative) path to a resource absolute url.
        /// </summary>
        /// <param name="helper">The UrlHelper object.</param>
        /// <param name="contentPath">The virtual path of the content.</param>
        /// <returns>The resource absolute url.</returns>
        public static string Resource(this UrlHelper helper, string contentPath)
        {

            string baseUrl = ConfigurationManager.AppSettings[ResourceBaseUrlConfigName];

            if (string.IsNullOrWhiteSpace(ResourceBaseUrlConfigName) || string.IsNullOrWhiteSpace(baseUrl))
                return helper.Content(contentPath);

            return baseUrl.TrimEnd('/', ' ') + "/" + contentPath.TrimStart('~', '/', ' ');
        }
    }
}
