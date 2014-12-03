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
        /// The ciphertext in Base64 string format.
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
        /// The ciphertext in Base64 string format.
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

        #region DES

        /// <summary>
        /// DES encrypt.
        /// </summary>
        /// <param name="clearText">The cleartext.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>
        /// The ciphertext in Base64 string format.
        /// </returns>
        public static string DESEncrypt(string clearText, SymmetricAlgorithm key)
        {
            Checker.Parameter(!string.IsNullOrEmpty(clearText), "cleartext can not be empty or null.");

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = StaticVariables.Encoding.GetBytes(clearText);

            return DESEncrypt(inputByteArray, key);
        }

        /// <summary>
        /// DES encrypt.
        /// </summary>
        /// <param name="clearBytes">The cleartext.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>The ciphertext in Base64 string format.</returns>
        public static string DESEncrypt(byte[] clearBytes, SymmetricAlgorithm key)
        {
            Checker.Parameter(clearBytes != null, "clearBytes can not be null.");
            Checker.Parameter(key != null, "encryption key can not be null.");

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(clearBytes, 0, clearBytes.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }


        /// <summary>
        /// DES decrypt.
        /// </summary>
        /// <param name="encryptedText">The ciphertext in Base64 string format.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>The cleartext.</returns>
        public static string DESDecrypt(string encryptedText, SymmetricAlgorithm key)
        {
            Checker.Parameter(!string.IsNullOrEmpty(encryptedText), "encryptedText can not be empty or null.");

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            return DESDecrypt(Convert.FromBase64String(encryptedText), key);;
        }

        /// <summary>
        /// DES decrypt.
        /// </summary>
        /// <param name="encryptedBytes">The ciphertext in binary format.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>The cleartext.</returns>
        public static string DESDecrypt(byte[] encryptedBytes, SymmetricAlgorithm key)
        {
            Checker.Parameter(encryptedBytes != null, "encryptedBytes can not be null.");
            Checker.Parameter(key != null, "encryption key can not be null.");


            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            

            using(MemoryStream ms = new MemoryStream(encryptedBytes))
            using (CryptoStream cs = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }


        }

        #endregion


        #region Rijndael

        /// <summary>
        /// Rijndael encrypt.
        /// </summary>
        /// <param name="clearText">The cleartext.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>
        /// The ciphertext in Base64 string format.
        /// </returns>
        public static string RijndaelEncrypt(string clearText, SymmetricAlgorithm key)
        {
            Checker.Parameter(!string.IsNullOrEmpty(clearText), "cleartext can not be empty or null.");

            return RijndaelEncrypt(StaticVariables.Encoding.GetBytes(clearText), key);
        }

        /// <summary>
        /// Rijndael encrypt.
        /// </summary>
        /// <param name="clearBytes">The cleartext.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>
        /// The ciphertext in Base64 string format.
        /// </returns>
        public static string RijndaelEncrypt(byte[] clearBytes, SymmetricAlgorithm key)
        {
            Checker.Parameter(clearBytes != null, "clearBytes can not be null.");
            Checker.Parameter(key != null, "encryption key can not be null.");


            Rijndael rijndael = Rijndael.Create();

            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, key.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(clearBytes, 0, clearBytes.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        /// <summary>
        /// Rijndael decrypt.
        /// </summary>
        /// <param name="encryptedText">The ciphertext in Base64 string format</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>
        /// The cleartext.
        /// </returns>
        public static string RijndaelDecrypt(string encryptedText, SymmetricAlgorithm key)
        {
            Checker.Parameter(!string.IsNullOrEmpty(encryptedText), "encryptedText can not be empty or null.");

            return RijndaelDecrypt(Convert.FromBase64String(encryptedText), key);
        }

        /// <summary>
        /// Rijndael decrypt.
        /// </summary>
        /// <param name="encryptedBytes">The ciphertext in binary format.</param>
        /// <param name="key">The encryption key.</param>
        /// <returns>
        /// The cleartext in binary format.
        /// </returns>
        public static string RijndaelDecrypt(byte[] encryptedBytes, SymmetricAlgorithm key)
        {
            Checker.Parameter(encryptedBytes != null, "encryptedBytes can not be null.");
            Checker.Parameter(key != null, "encryption key can not be null.");

           

            Rijndael rijndael = Rijndael.Create();

            using (MemoryStream ms = new MemoryStream(encryptedBytes, 0, encryptedBytes.Length))
            using (CryptoStream cs = new CryptoStream(ms, key.CreateDecryptor(), CryptoStreamMode.Read))
            using (StreamReader sr = new StreamReader(cs))
            {
                return sr.ReadToEnd();
            }
        }

        #endregion
    }
}
