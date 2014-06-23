using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial.Cache
{
    /// <summary>
    /// IClusterRegion.
    /// </summary>
    public interface IClusterRegion
    {
        /// <summary>
        /// Inserts cache key with the specified region.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="region">The cache region.</param>
        void Insert(string cacheKey, string region);
        /// <summary>
        /// Gets the cache keys contained in the specified region.
        /// </summary>
        /// <param name="region">The cache region.</param>
        /// <returns></returns>
        string[] GetKeys(string region);
        /// <summary>
        /// Deletes the specified cache key.
        /// </summary>
        /// <param name="cacheKey">The cache key.</param>
        void Delete(string cacheKey);

        /// <summary>
        /// Gets the cache regions.
        /// </summary>
        /// <returns>The cache regions.</returns>
        string[] GetRegions();
    }
}
