using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Radial.Data.Mongod.Param
{
    /// <summary>
    /// Mongo param item entity class.
    /// </summary>
    public sealed class ItemEntity
    {
        /// <summary>
        /// Entity id.
        /// </summary>
        public const string EntityId = "MongoParamItem";

        /// <summary>
        /// Gets the id.
        /// </summary>
        [BsonId]
        public string Id { get { return EntityId; } set { } }

        /// <summary>
        /// Gets or sets the xml based content.
        /// </summary>
        /// <value>
        /// The xml based content.
        /// </value>
        public string Content { get; set; }
    }
}
