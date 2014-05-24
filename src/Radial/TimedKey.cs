using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radial
{
    /// <summary>
    /// Generate key string based on time.
    /// </summary>
    public static class TimedKey
    {
        /// <summary>
        /// The alphabet.
        /// </summary>
        const string Alphabet = "123456789ABCDEFHKRST";

        /// <summary>
        /// Create a new key.
        /// </summary>
        /// <returns>The key string.</returns>
        public static string New()
        {
            return EncodeTime((long)(DateTime.Now - new DateTime(2014, 1, 1)).TotalMilliseconds) +
                Guid.NewGuid().ToString("n").ToUpper().Substring(0, 6);
        }

        /// <summary>
        /// Encodes the time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static string EncodeTime(long value)
        {
            string str = string.Empty;

            while (value > 0)
            {
                str = Alphabet[(int)(value % Alphabet.Length)].ToString() + str;
                value /= Alphabet.Length;
            }

            return str;
        }
    }
}
