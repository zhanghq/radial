using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Radial
{
    /// <summary>
    /// Sequence based on time.
    /// </summary>
    public static class TimingSeq
    {
        static object SyncRoot = new object();

        const string Alphabet = "0123456789ABCDEFHKNT";

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
            lock (SyncRoot)
            {
                Thread.Sleep(1);
                return Prefix + EncodeLong((long)(DateTime.Now - new DateTime(2014, 1, 1)).TotalMilliseconds);
            }
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
