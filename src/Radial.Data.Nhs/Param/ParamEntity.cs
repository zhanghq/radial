using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Radial.Serialization;

namespace Radial.Data.Nhs.Param
{
    /// <summary>
    /// Param item entity class.
    /// </summary>
    public class ParamEntity
    {
        /// <summary>
        /// Entity id.
        /// </summary>
        public const string EntityId = "ParamItem";

        string _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="ParamEntity" /> class.
        /// </summary>
        public ParamEntity()
        {
            _id = EntityId;
        }

        /// <summary>
        /// Gets the id.
        /// </summary>
        public string Id { get { return _id; } }

        /// <summary>
        /// Gets or sets the xml based content.
        /// </summary>
        /// <value>
        /// The xml based content.
        /// </value>
        public string XmlContent { get; set; }


        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        /// To string which will saved in cached
        /// </summary>
        /// <returns></returns>
        public string ToCacheString()
        {
            return JsonSerializer.Serialize(new { xml = Toolkits.Compress(this.XmlContent), version = this.Version });
        }

        /// <summary>
        /// Froms the cache string.
        /// </summary>
        /// <param name="cacheString">The cache string.</param>
        /// <returns></returns>
        public static ParamEntity FromCacheString(string cacheString)
        {
            if (string.IsNullOrWhiteSpace(cacheString))
                return null;

            dynamic o = JsonSerializer.Deserialize(cacheString);

            return new ParamEntity { XmlContent = Toolkits.Decompress((string)o.xml), Version = o.version };
        }
    }
}
