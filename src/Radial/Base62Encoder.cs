using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radial
{
    /// <summary>
    /// Base62 Encoder Class
    /// </summary>
    public sealed class Base62Encoder
    {
        string _alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

        /// <summary>
        /// Initializes a new instance of the <see cref="Base62Encoder"/> class.
        /// </summary>
        public Base62Encoder() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Base62Encoder"/> class.
        /// </summary>
        /// <param name="alphabet">The alphabet.</param>
        public Base62Encoder(string alphabet)
        {
            if (!string.IsNullOrWhiteSpace(alphabet))
                _alphabet = alphabet;
        }


        /// <summary>
        /// Gets the alphabet.
        /// </summary>
        public string Alphabet
        {
            get
            {
                return _alphabet;
            }
        }

        /// <summary>
        /// Convert long value to Base62 string.
        /// </summary>
        /// <param name="value">The input value.</param>
        /// <returns>The Base62 string.</returns>
        public string ToBase62String(ulong value)
        {
            Checker.Parameter(value >= 0, "input value must greater than or equal to 0");

            if (value == 0)
                return "0";

            string str = string.Empty;
            while (value > 0)
            {
                str = Alphabet[((int)(value % 62))] + str;
                value /= 62;
            }

            return str;
        }

        /// <summary>
        /// Convert Base62 string to long value.
        /// </summary>
        /// <param name="input">The input Base62 string.</param>
        /// <returns>The long value.</returns>
        public ulong FromBase62String(string input)
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
