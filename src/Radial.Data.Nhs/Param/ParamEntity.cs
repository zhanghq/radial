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
        int _version = 0;

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
        public string XmlContent
        {
            get;
            set;
        }


        /// <summary>
        /// Gets the version.
        /// </summary>
        public int Version
        {
            get { return _version; }
        }


        /// <summary>
        /// To string which will saved in cached
        /// </summary>
        /// <returns></returns>
        public string ToCacheString()
        {
            return XmlContent;
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

            return new ParamEntity { XmlContent = cacheString };
        }
    }
}
