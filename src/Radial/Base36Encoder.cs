using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial
{
    /// <summary>
    /// Base36 Encoder Class
    /// </summary>
    public static class Base36Encoder
    {
        /// <summary>
        /// Base36 alphabet.
        /// </summary>
        public const string Alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Convert long value to Base36 string.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>The Base36 string.</returns>
        public static string ToBase36String(ulong value)
        {
            if (value == 0)
                return "0";

            string str = string.Empty;

            while (value > 0)
            {
                str = Alphabet[(int)(value % 36)].ToString() + str;
                value /= 36;
            }

            return str;
        }

        /// <summary>
        /// Convert Base36 string to long value.
        /// </summary>
        /// <param name="input">The input Base36 string.</param>
        /// <returns>The long value.</returns>
        public static ulong FromBase36String(string input)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(input), "input string can not be empty or null");

            input = input.Trim();

            ulong result = 0;

            for (int i = 0; i < input.Length; i++)
            {
                result += (ulong)(Alphabet.IndexOf(input[i]) * Math.Pow(36, input.Length - 1 - i));
            }

            return result;
        }
    }
}
