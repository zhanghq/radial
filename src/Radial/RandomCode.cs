using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial
{
    /// <summary>
    /// Random code class.
    /// </summary>
    public static class RandomCode
    {
        const string Character = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

        /// <summary>
        /// Initializes a new instance of the System.Random class.
        /// </summary>
        public static Random NewInstance
        {
            get
            {
                return new Random(Guid.NewGuid().ToString().GetHashCode());
            }
        }

        /// <summary>
        /// Create random string.
        /// </summary>
        /// <param name="length">The length of random code.</param>
        /// <returns>New random string.</returns>
        public static string Create(int length)
        {
            Checker.Parameter(length > 0, "length of random code must greater than 0.");

            char[] charArray = Character.ToCharArray();

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.Append(charArray[NewInstance.Next(0, charArray.Length - 1)]);
            }

            return sb.ToString();
        }

        ///// <summary>
        ///// Create a time-related key value. 
        ///// </summary>
        ///// <remarks>The last 2 characters are random string, the rest are Unix timestamp includes milliseconds in Base36 format.</remarks>
        ///// <returns>Time-related key value.</returns>
        //public static string TimeRelatedKey()
        //{
        //    DateTime now = DateTime.Now;

        //    ulong longTime = (ulong)(Toolkits.ToUnixTimeStamp(now) * 1000 + now.Millisecond);

        //    string guid = Guid.NewGuid().ToString("n").ToUpper();

        //    return string.Format("{0}{1}", Base36Encoder.ToBase36String(longTime), guid.Substring(RandomCode.NewInstance.Next(0, guid.Length - 2), 2));
        //}

    }
}
