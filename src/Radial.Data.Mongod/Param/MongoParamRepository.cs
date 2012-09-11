using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Driver.Builders;
using MongoDB.Driver;

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
                var query = Query<ItemEntity>.EQ<string>(o => o.Id, obj.Id);
                WriteDatabase.GetCollection<ItemEntity>(CollectionName).FindAndRemove(query, null);
            }
        }

        /// <summary>
        /// Exists the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public override bool Exists(string key)
        {
            return GetTotal(o => o.Id == key.Trim()) > 0;
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public override ItemEntity Find(string key)
        {
            return Find(o => o.Id == key.Trim());
        }

        /// <summary>
        /// Adds objects to the repository.
        /// </summary>
        /// <param name="objs">The objects.</param>
        public override void Add(IEnumerable<ItemEntity> objs)
        {
            if (objs != null)
            {
                //In order to throw an exception, mandatory is set SafeMode to True
                WriteDatabase.GetCollection<ItemEntity>(CollectionName).InsertBatch<ItemEntity>(objs, SafeMode.True);
            }
        }

        /// <summary>
        /// Modify the specified object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <param name="originalSHA1">The original SHA1.</param>
        /// <returns>If successfully updated matched data item return TRUE, otherwise return FALSE.</returns>
        public bool Modify(ItemEntity obj, string originalSHA1)
        {
            if (obj != null)
            {
                var query = Query<ItemEntity>.Where(o => o.Id == obj.Id && o.SHA1 == originalSHA1);
                var update = Update<ItemEntity>.Set<string>(o => o.Content, obj.Content).Set<string>(o => o.SHA1, obj.SHA1);
                FindAndModifyResult fmr = WriteDatabase.GetCollection<ItemEntity>(CollectionName).FindAndModify(query, null, update);
                return fmr.ModifiedDocument != null;
            }
            return false;
        }
    }
}
