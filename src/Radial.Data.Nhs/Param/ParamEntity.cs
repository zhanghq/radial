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
        public string XmlContent
        {
            get;
            set;
        }


        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public int Version
        {
            get;
            set;
        }


        /// <summary>
        /// To string which will saved in cached
        /// </summary>
        /// <returns></returns>
        public string ToCacheString()
        {
            return Toolkits.ToBase64String(XmlContent) + "|" + Version;
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

            string[] sp = cacheString.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (sp.Length == 2)
                return new ParamEntity { XmlContent = Toolkits.FromBase64String(sp[0]), Version = int.Parse(sp[1]) };

            return null;
        }
    }
}
