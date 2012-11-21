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
        string _xmlContent;
        string _sha1;

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
        public string XmlContent
        {
            get
            {
                return _xmlContent;
            }
            set
            {
                _xmlContent = value;

                if (_xmlContent == null)
                    _xmlContent = string.Empty;

                _xmlContent = _xmlContent.Trim();

                _sha1 = Radial.Security.CryptoProvider.SHA1Encrypt(_xmlContent);

            }
        }


        /// <summary>
        /// Gets the Sha1 code of content.
        /// </summary>
        public string Sha1
        {
            get { return _sha1; }
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
