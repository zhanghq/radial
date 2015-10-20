using System.Collections.Generic;

namespace Radial.Persist.Nhs
{
    /// <summary>
    /// Indicate table can separate into shards .
    /// </summary>
    public interface ITableShardable
    {
        /// <summary>
        /// Gets the shard table mappings.
        /// </summary>
        /// <param name="storageAlias">The storage alias.</param>
        /// <returns></returns>
        ISet<ShardTableMapping> GetTableMappings(string storageAlias);
    }
}
