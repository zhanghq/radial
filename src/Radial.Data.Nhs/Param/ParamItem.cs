using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial.Data.Nhs.Param
{
    /// <summary>
    /// Param Item
    /// </summary>
    sealed class ParamItem
    {
        public const string ItemId = "ParamItem";

        /// <summary>
        /// Initializes a new instance of the <see cref="ParamItem" /> class.
        /// </summary>
        public ParamItem()
        {
            Version = 1;
        }

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
        /// To the cache string.
        /// </summary>
        /// <returns></returns>
        public string ToCacheString()
        {
            return Radial.Toolkits.ToBase64String(XmlContent) + "|" + Version;
        }

        /// <summary>
        /// Froms the cache string.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static ParamItem FromCacheString(string str)
        {
            if (string.IsNullOrWhiteSpace(str))
                return null;

            string[] sp = str.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);

            if (sp.Length == 2)
                return new ParamItem { XmlContent = Toolkits.FromBase64String(sp[0]), Version = int.Parse(sp[1]) };

            return null;
        }
    }
}
