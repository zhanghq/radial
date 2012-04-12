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
    }
}
