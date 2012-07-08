using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver.Builders;

namespace Radial.Data.Mongod.Param
{
    /// <summary>
    /// MongoParamRepository
    /// </summary>
    class MongoParamRepository : MongoRepository<ItemEntity, string>
    {
        /// <summary>
        /// Removes the specified obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        public override void Remove(ItemEntity obj)
        {
            if (obj != null)
            {
                var query = Query.EQ("_id", obj.Id);
                WriteDatabase.GetCollection<ItemEntity>(CollectionName).Remove(query);
            }
        }

        /// <summary>
        /// Exists the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public override bool Exist(string key)
        {
            return GetTotal(o => o.Id == key.Trim()) > 0;
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public override ItemEntity Get(string key)
        {
            return Get(o => o.Id == key.Trim());
        }
    }
}
