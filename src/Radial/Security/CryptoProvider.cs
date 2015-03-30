using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using Radial;
using System.IO;

namespace Radial.Security
{
    /// <summary>
    /// Crypto provider
    /// </summary>
    public static class CryptoProvider
    {

        #region SHA1

        /// <summary>
        /// SHA1 encrypt.
        /// </summary>
        /// <param name="clearText">The cleartext.</param>
        /// <returns>
        /// The ciphertext in string format.
        /// </returns>
        public static string SHA1Encrypt(string clearText)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(clearText), "cleartext can not be empty or null.");

            byte[] clearTextBytes = StaticVariables.Encoding.GetBytes(clearText);

            byte[] encryptedBytes = SHA1Encrypt(clearTextBytes);

            StringBuilder ret = new StringBuilder();
            foreach (byte b in encryptedBytes)
            {
                ret.AppendFormat("{0:x2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// SHA1 encrypt.
        /// </summary>
        /// <param name="clearTextBytes">The cleartext.</param>
        /// <returns>The ciphertext in binary format.</returns>
        public static byte[] SHA1Encrypt(byte[] clearTextBytes)
        {
            Checker.Parameter(clearTextBytes != null, "clearTextBytes can not be null.");

            SHA1 sha1 = SHA1.Create();
            return sha1.ComputeHash(clearTextBytes);
        }

        #endregion

        #region MD5
        /// <summary>
        /// MD5 encrypt.
        /// </summary>
        /// <param name="clearText">The cleartext.</param>
        /// <returns>
        /// The ciphertext in string format.
        /// </returns>
        public static string MD5Encrypt(string clearText)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(clearText), "cleartext can not be empty or null.");

            byte[] clearTextBytes = StaticVariables.Encoding.GetBytes(clearText);

            byte[] encryptedBytes = MD5Encrypt(clearTextBytes);

            StringBuilder ret = new StringBuilder();
            foreach (byte b in encryptedBytes)
            {
                ret.AppendFormat("{0:x2}", b);
            }
            return ret.ToString();
        }

        /// <summary>
        /// MD5 encrypt.
        /// </summary>
        /// <param name="clearTextBytes">The cleartext.</param>
        /// <returns>The ciphertext in binary format.</returns>
        public static byte[] MD5Encrypt(byte[] clearTextBytes)
        {
            Checker.Parameter(clearTextBytes != null, "cleartext can not be null.");

            MD5 md5 = MD5.Create();
            return md5.ComputeHash(clearTextBytes);
        }

        #endregion

        #region Symmetric

        /// <summary>
        /// Symmetric encrypt.
        /// </summary>
        /// <param name="alg">The symmetric algorithm.</param>
        /// <param name="input">The input bytes</param>
        /// <returns></returns>
        public static byte[] SymmetricEncrypt(SymmetricAlgorithm alg,byte[] input)
        {
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, alg.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Symmetric decrypt.
        /// </summary>
        /// <param name="alg">The symmetric algorithm.</param>
        /// <param name="input">The input bytes</param>
        /// <returns></returns>
        public static byte[] SymmetricDecrypt(SymmetricAlgorithm alg, byte[] input)
        {
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, alg.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cs.Write(input, 0, input.Length);
                cs.FlushFinalBlock();
                return ms.ToArray();
            }
        }


        #endregion
    }
}
