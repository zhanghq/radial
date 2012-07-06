using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.SessionState;
using System.IO;
using System.Web;

namespace Radial.Web.Session
{
    /// <summary>
    /// SessionStateStoreDataExtensions
    /// </summary>
    public static class SessionStateStoreDataExtensions
    {
        /// <summary>
        /// Serializes the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static byte[] Serialize(this SessionStateStoreData data)
        {
            if (data == null)
                return new byte[] { };

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter writer = new BinaryWriter(ms))
            {
                ((SessionStateItemCollection)data.Items).Serialize(writer);
                return ms.ToArray();
            }
        }
    }
}
