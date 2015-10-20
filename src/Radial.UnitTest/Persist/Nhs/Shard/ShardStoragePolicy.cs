using Radial.Persist;
using Radial.Persist.Nhs;
using Radial.UnitTest.Persist.Nhs.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Radial.UnitTest.Persist.Nhs.Shard
{
    public class ShardStoragePolicy : IStoragePolicy, ITableShardable
    {

        /// <summary>
        /// Gets storage alias according to the specified object key.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <param name="key">The object key according to.</param>
        /// <returns>
        /// The storage alias.
        /// </returns>
        public string GetObjectAlias(Type type, object key)
        {
            if (type == typeof(Book))
            {
                string id = (string)key;

                int last = int.Parse(id.Last().ToString());

                if (last % 2 == 0)
                    return "shard1";
                return "shard2";
            }

            return "default";
        }

        /// <summary>
        /// Gets storage aliases supported by the specified object type.
        /// </summary>
        /// <param name="type">Type of the object.</param>
        /// <returns>
        /// The storage alias array.
        /// </returns>
        public string[] GetTypeAliases(Type type)
        {
            if (type == typeof(Book))
            {
                return new string[] { "shard1", "shard2" };
            }

            return new string[] { "default" };
        }

        /// <summary>
        /// Gets the shard table mappings.
        /// </summary>
        /// <param name="storageAlias">The storage alias.</param>
        /// <returns></returns>
        public ISet<ShardTableMapping> GetTableMappings(string storageAlias)
        {
            ISet<ShardTableMapping> set = new HashSet<ShardTableMapping>();

            if (storageAlias == "shard1")
            {
                set.Add(new ShardTableMapping { ObjectType = typeof(Book).FullName, TableName = "Book1" });
            }

            if (storageAlias == "shard2")
            {
                set.Add(new ShardTableMapping { ObjectType = typeof(Book).FullName, TableName = "Book2" });
            }

            return set;
        }
    }
}
