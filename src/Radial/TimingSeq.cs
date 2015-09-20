using System;
using System.Configuration;

namespace Radial
{
    /// <summary>
    /// Sequence based on time.
    /// </summary>
    public static class TimingSeq
    {
        const string Alphabet = "0123456789ABCDEFHKNRTU";

        /// <summary>
        /// Gets the prefix.
        /// </summary>
        static string Prefix
        {
            get
            {
                string p = ConfigurationManager.AppSettings["TimingSeqPrefix"];

                if (!string.IsNullOrWhiteSpace(p))
                    return p.ToUpper();

                return null;
            }
        }

        /// <summary>
        /// Create a next value.
        /// </summary>
        /// <returns>The value string.</returns>
        public static string Next()
        {
            var yt = DateTime.Now;
            var ytms = EncodeLong((long)yt.Year * 100000000000
                + (long)(yt - new DateTime(yt.Year, 1, 1)).TotalMilliseconds);

            string r = Guid.NewGuid().ToString("n").ToUpper();

            r = r.Substring(RandomCode.NewInstance.Next(0, r.Length - 4), 4);

            return Prefix + ytms + r;
        }

        /// <summary>
        /// Encodes the long.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        private static string EncodeLong(long value)
        {
            string str = string.Empty;

            while (value > 0)
            {
                str = Alphabet[(int)(value % Alphabet.Length)] + str;
                value /= Alphabet.Length;
            }

            return str;
        }
    }
}
