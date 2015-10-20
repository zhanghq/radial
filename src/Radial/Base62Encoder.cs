using System;

namespace Radial
{
    /// <summary>
    /// Base62 Encoder Class
    /// </summary>
    public static class Base62Encoder
    {
        /// <summary>
        /// Base62 alphabet.
        /// </summary>
        public const string Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Convert long value to Base62 string.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>The Base62 string.</returns>
        public static string ToBase62String(ulong value)
        {
            if (value == 0)
                return "0";

            string str = string.Empty;

            while (value > 0)
            {
                str = Alphabet[(int)(value % 62)] + str;
                value /= 62;
            }

            return str;
        }

        /// <summary>
        /// Convert Base62 string to long value.
        /// </summary>
        /// <param name="input">The input Base62 string.</param>
        /// <returns>The long value.</returns>
        public static ulong FromBase62String(string input)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(input), "input string can not be empty or null");

            input = input.Trim();

            ulong result = 0;

            for (int i = 0; i < input.Length; i++)
            {
                result += (ulong)(Alphabet.IndexOf(input[i]) * Math.Pow(62, input.Length - 1 - i));
            }

            return result;
        }
    }
}
