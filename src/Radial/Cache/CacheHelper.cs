using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Cache
{
    /// <summary>
    /// Cache helper class.
    /// </summary>
    public static class CacheHelper
    {
        /// <summary>
        /// Normalizes the cache key.
        /// </summary>
        /// <param name="key">The cache key.</param>
        /// <returns>Normalize cache key.</returns>
        public static string NormalizeKey(string key)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(key), "cache key can not be empty or null");

            return key.Trim().ToLower();
        }
    }
}
