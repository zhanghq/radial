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
        /// Converts a virtual (relative) image path to an absolute url.
        /// </summary>
        /// <param name="helper">The UrlHelper object.</param>
        /// <param name="imagePath">The virtual path of the image.</param>
        /// <returns>The image absolute url.</returns>
        public static string Image(this UrlHelper helper, string imagePath)
        {
            return Resource(helper, imagePath, "ImageBaseUrl");
        }

        /// <summary>
        /// Converts a virtual (relative) style sheet path to an absolute url.
        /// </summary>
        /// <param name="helper">The UrlHelper object.</param>
        /// <param name="stylePath">The virtual path of the style sheet.</param>
        /// <returns>The style sheet absolute url.</returns>
        public static string Style(this UrlHelper helper, string stylePath)
        {
            return Resource(helper, stylePath, "StyleBaseUrl");
        }

        /// <summary>
        /// Converts a virtual (relative) script path to an absolute url.
        /// </summary>
        /// <param name="helper">The UrlHelper object.</param>
        /// <param name="scriptPath">The virtual path of the script.</param>
        /// <returns>The script absolute url.</returns>
        public static string Script(this UrlHelper helper, string scriptPath)
        {
            return Resource(helper, scriptPath, "ScriptBaseUrl");
        }

        /// <summary>
        /// Converts a virtual (relative) path to a resource absolute url.
        /// </summary>
        /// <param name="helper">The UrlHelper object.</param>
        /// <param name="contentPath">The virtual path of the content.</param>
        /// <param name="baseUrlConfigName">The base url configuration name.</param>
        /// <returns>The resource absolute url.</returns>
        public static string Resource(this UrlHelper helper, string contentPath, string baseUrlConfigName)
        {
            string baseUrl = ConfigurationManager.AppSettings[baseUrlConfigName];

            if (string.IsNullOrWhiteSpace(baseUrlConfigName) || string.IsNullOrWhiteSpace(baseUrl))
                return helper.Content(contentPath);

            return baseUrl.TrimEnd('/', ' ') + "/" + contentPath.TrimStart('~', '/', ' ');
        }
    }
}
