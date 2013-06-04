using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace Radial.Tools.NhDbFirst.Kernel
{
    /// <summary>
    /// Toolkits class.
    /// </summary>
    public static class Toolkits
    {
        /// <summary>
        /// Upper case the first char of the input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <returns>new string.</returns>
        public static string FirstCharUpperCase(string input)
        {
            if (!string.IsNullOrWhiteSpace(input))
                return input[0].ToString().ToUpper() + (input.Length > 1 ? input.Substring(1) : string.Empty);
            return input;
        }


        /// <summary>
        /// Normalizes the name.
        /// </summary>
        /// <param name="inputName">Name of the input.</param>
        /// <param name="spliters">The spliters.</param>
        /// <returns></returns>
        public static string NormalizeName(string inputName, char [] spliters)
        {
            if (spliters != null && spliters.Length > 0)
            {
                IList<string> sps = new List<string>(inputName.Split(spliters, StringSplitOptions.RemoveEmptyEntries));

                for (int i = 0; i < sps.Count; i++)
                {
                    sps[i] = Toolkits.FirstCharUpperCase(sps[i]);
                }

                return string.Join(string.Empty, sps.ToArray());
            }

            return FirstCharUpperCase(inputName);
        }
    }
}
