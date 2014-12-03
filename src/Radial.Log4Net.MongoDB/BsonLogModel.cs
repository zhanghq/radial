using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Bson.Serialization.Attributes;

namespace Radial.Log4Net.MongoDB
{
    /// <summary>
    /// BsonLogModel
    /// </summary>
    public class BsonLogModel
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        [BsonElement("_id")]
        public ObjectId Id { get; set; }
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        [BsonElement("time")]
        public DateTime Time { get; set; }
        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>
        /// The level.
        /// </value>
        [BsonElement("level")]
        public string Level { get; set; }
        /// <summary>
        /// Gets or sets the logger name.
        /// </summary>
        /// <value>
        /// The logger name.
        /// </value>
        [BsonElement("logger")]
        public string Logger { get; set; }
        /// <summary>
        /// Gets or sets the machine name.
        /// </summary>
        /// <value>
        /// The machine.
        /// </value>
        [BsonElement("machine")]
        public string Machine { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        [BsonElement("message")]
        public string Message { get; set; }
    }
}