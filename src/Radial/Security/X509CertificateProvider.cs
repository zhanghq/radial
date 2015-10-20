using System;
using System.Text;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Radial.Security
{
    /// <summary>
    ///  X.509 certificate provider class.
    /// </summary>
    public sealed class X509CertificateProvider
    {
        string _password;
        X509Certificate2 _cert;


        /// <summary>
        /// Initializes a new instance of the <see cref="X509CertificateProvider"/> class.
        /// </summary>
        /// <param name="certFilePath">The certificate file path.</param>
        public X509CertificateProvider(string certFilePath)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(certFilePath), "certificate file path can not be empty or null");
            _cert = new X509Certificate2(certFilePath);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="X509CertificateProvider"/> class.
        /// </summary>
        /// <param name="certFilePath">The certificate file path.</param>
        /// <param name="password">The certificate password.</param>
        public X509CertificateProvider(string certFilePath, string password)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(certFilePath), "certificate file path can not be empty or null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(password), "certificate password can not be empty or null");
            _password = password.Trim();
            _cert = new X509Certificate2(certFilePath, _password);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="X509CertificateProvider"/> class.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        public X509CertificateProvider(byte[] rawData)
        {
            Checker.Parameter(rawData != null, "certificate data can not be null");
            _cert = new X509Certificate2(rawData);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="X509CertificateProvider"/> class.
        /// </summary>
        /// <param name="rawData">The raw data.</param>
        /// <param name="password">The password.</param>
        public X509CertificateProvider(byte[] rawData, string password)
        {
            Checker.Parameter(rawData != null, "certificate data can not be null");
            Checker.Parameter(!string.IsNullOrWhiteSpace(password), "certificate password can not be empty or null");
            _password = password.Trim();
            _cert = new X509Certificate2(rawData, _password);
        }


        /// <summary>
        /// Exports the specified content type.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <returns></returns>
        public byte[] Export(X509ContentType contentType)
        {
            return Export(contentType, false);
        }


        /// <summary>
        /// Exports the specified content type.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        /// <param name="usePassword">if set to <c>true</c> [use password].</param>
        /// <returns></returns>
        public byte[] Export(X509ContentType contentType, bool usePassword)
        {
            if (usePassword)
            {
                Checker.Requires(!string.IsNullOrWhiteSpace(_password), "password not exist");
                return _cert.Export(contentType, _password);
            }
            else
                return _cert.Export(contentType);
        }


        /// <summary>
        /// Performs a X.509 chain validation using basic validation policy.
        /// </summary>
        /// <returns>true if the validation succeeds; false if the validation fails.</returns>
        public bool Verify()
        {
            return _cert.Verify();
        }

        /// <summary>
        /// Encrypts the use public key.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        public string EncryptUsePublicKey(string msg)
        {
            return EncryptUsePublicKey(msg, false);
        }

        /// <summary>
        /// Encrypts the use public key.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="fOAEP">if set to <c>true</c> [f OAEP].</param>
        /// <returns></returns>
        public string EncryptUsePublicKey(string msg,  bool fOAEP)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(msg), "需要加密的消息不能为空");

            byte[] encryptedBytes = EncryptUsePublicKey(GlobalVariables.Encoding.GetBytes(msg), fOAEP);

            StringBuilder sb = new StringBuilder();
            foreach (byte eb in encryptedBytes)
                sb.Append(eb.ToString("x2"));

            return sb.ToString();
        }

        /// <summary>
        /// Encrypts the use public key.
        /// </summary>
        /// <param name="rgb">The RGB.</param>
        /// <returns></returns>
        public byte[] EncryptUsePublicKey(byte[] rgb)
        {
            return EncryptUsePublicKey(rgb, false);
        }

        /// <summary>
        /// Encrypts the use public key.
        /// </summary>
        /// <param name="rgb">The RGB.</param>
        /// <param name="fOAEP">if set to <c>true</c> [f OAEP].</param>
        /// <returns></returns>
        public byte[] EncryptUsePublicKey(byte[] rgb, bool fOAEP)
        {
            Checker.Parameter(rgb != null, "需要加密的消息数组不能为空");

            RSACryptoServiceProvider p = (RSACryptoServiceProvider)_cert.PublicKey.Key;
            return p.Encrypt(rgb, fOAEP);
        }


        /// <summary>
        /// Decrypts the use public key.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        public string DecryptUsePublicKey(string msg)
        {
            return DecryptUsePublicKey(msg, false);
        }


        /// <summary>
        /// Decrypts the use public key.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="fOAEP">if set to <c>true</c> [f OAEP].</param>
        /// <returns></returns>
        public string DecryptUsePublicKey(string msg, bool fOAEP)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(msg), "需要解密的消息不能为空");

            byte[] msgBytes = new byte[msg.Length / 2];

            for (int i = 0; i < msgBytes.Length; i++)
                msgBytes[i] = (byte)Convert.ToInt16(msg.Substring(i * 2, 2), 16);

            byte[] dencryptedBytes = DecryptUsePublicKey(msgBytes, fOAEP);

            return GlobalVariables.Encoding.GetString(dencryptedBytes);
        }

        /// <summary>
        /// Decrypts the use public key.
        /// </summary>
        /// <param name="rgb">The RGB.</param>
        /// <returns></returns>
        public byte[] DecryptUsePublicKey(byte[] rgb)
        {
            return DecryptUsePublicKey(rgb, false);
        }

        /// <summary>
        /// Decrypts the use public key.
        /// </summary>
        /// <param name="rgb">The RGB.</param>
        /// <param name="fOAEP">if set to <c>true</c> [f OAEP].</param>
        /// <returns></returns>
        public byte[] DecryptUsePublicKey(byte[] rgb, bool fOAEP)
        {
            Checker.Parameter(rgb != null, "需要解密的数组不能为空");

            RSACryptoServiceProvider p = (RSACryptoServiceProvider)_cert.PublicKey.Key;
            return p.Decrypt(rgb, fOAEP);
        }


        /// <summary>
        /// Encrypts the use private key.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        public string EncryptUsePrivateKey(string msg)
        {
            return EncryptUsePrivateKey(msg, false);
        }


        /// <summary>
        /// Encrypts the use private key.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="fOAEP">if set to <c>true</c> [f OAEP].</param>
        /// <returns></returns>
        public string EncryptUsePrivateKey(string msg,  bool fOAEP)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(msg), "message can not be empty or null");

            byte[] encryptedBytes = EncryptUsePrivateKey(GlobalVariables.Encoding.GetBytes(msg), fOAEP);

            StringBuilder sb = new StringBuilder();
            foreach (byte eb in encryptedBytes)
                sb.Append(eb.ToString("x2"));

            return sb.ToString();
        }


        /// <summary>
        /// Encrypts the use private key.
        /// </summary>
        /// <param name="rgb">The RGB.</param>
        /// <param name="fOAEP">if set to <c>true</c> [f OAEP].</param>
        /// <returns></returns>
        public byte[] EncryptUsePrivateKey(byte[] rgb, bool fOAEP)
        {
            Checker.Parameter(rgb != null, "rgb can not be null");

            Checker.Requires(_cert.HasPrivateKey, "certificate does not has private key");

            RSACryptoServiceProvider p = (RSACryptoServiceProvider)_cert.PrivateKey;

            return p.Encrypt(rgb, fOAEP);
        }


        /// <summary>
        /// Decrypts the use private key.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        public string DecryptUsePrivateKey(string msg)
        {
            return DecryptUsePrivateKey(msg, false);
        }


        /// <summary>
        /// Decrypts the use private key.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="fOAEP">if set to <c>true</c> [f OAEP].</param>
        /// <returns></returns>
        public string DecryptUsePrivateKey(string msg,  bool fOAEP)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(msg), "msg can not be empty or null");

            byte[] msgBytes = new byte[msg.Length / 2];

            for (int i = 0; i < msgBytes.Length; i++)
                msgBytes[i] = Convert.ToByte(msg.Substring(i * 2, 2), 16);

            byte[] dencryptedBytes = DecryptUsePrivateKey(msgBytes, fOAEP);

            return GlobalVariables.Encoding.GetString(dencryptedBytes);
        }


        /// <summary>
        /// Decrypts the use private key.
        /// </summary>
        /// <param name="rgb">The RGB.</param>
        /// <param name="fOAEP">if set to <c>true</c> [f OAEP].</param>
        /// <returns></returns>
        public byte[] DecryptUsePrivateKey(byte[] rgb, bool fOAEP)
        {
            Checker.Parameter(rgb != null, "rgb can not be null");

            Checker.Requires(_cert.HasPrivateKey, "certificate does not has private key");

            RSACryptoServiceProvider p = (RSACryptoServiceProvider)_cert.PrivateKey;
            return p.Decrypt(rgb, fOAEP);
        }


        /// <summary>
        /// Creates the signature use public key.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        public string CreateSignatureUsePublicKey(string msg)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(msg), "msg can not be empty or null");

            byte[] encryptedBytes = CreateSignatureUsePublicKey(GlobalVariables.Encoding.GetBytes(msg));

            StringBuilder sb = new StringBuilder();
            foreach (byte eb in encryptedBytes)
                sb.Append(eb.ToString("x2"));

            return sb.ToString();

        }


        /// <summary>
        /// Creates the signature use public key.
        /// </summary>
        /// <param name="msgBytes">The MSG bytes.</param>
        /// <returns></returns>
        public byte[] CreateSignatureUsePublicKey(byte[] msgBytes)
        {
            Checker.Parameter(msgBytes != null, "msgBytes can not be null");

            RSACryptoServiceProvider sign = (RSACryptoServiceProvider)_cert.PublicKey.Key;

            return sign.SignData(msgBytes, new System.Security.Cryptography.SHA1CryptoServiceProvider());
        }



        /// <summary>
        /// Creates the signature use private key.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <returns></returns>
        public string CreateSignatureUsePrivateKey(string msg)
        {
            Checker.Parameter(!string.IsNullOrWhiteSpace(msg), "msg can not be empty or null");

            byte[] encryptedBytes = CreateSignatureUsePrivateKey(GlobalVariables.Encoding.GetBytes(msg));

            StringBuilder sb = new StringBuilder();
            foreach (byte eb in encryptedBytes)
                sb.Append(eb.ToString("x2"));

            return sb.ToString();

        }


        /// <summary>
        /// Creates the signature use private key.
        /// </summary>
        /// <param name="msgBytes">The MSG bytes.</param>
        /// <returns></returns>
        public byte[] CreateSignatureUsePrivateKey(byte[] msgBytes)
        {
            Checker.Parameter(msgBytes != null, "msgBytes can not be null");

            Checker.Requires(_cert.HasPrivateKey, "certificate does not has private key");

            RSACryptoServiceProvider sign = (RSACryptoServiceProvider)_cert.PrivateKey;

            return sign.SignData(msgBytes, new System.Security.Cryptography.SHA1CryptoServiceProvider());
        }


        /// <summary>
        /// Verifies the signature use public key.
        /// </summary>
        /// <param name="dataStr">The data STR.</param>
        /// <param name="signature">The signature.</param>
        /// <returns></returns>
        public bool VerifySignatureUsePublicKey(string dataStr, string signature)
        {
            Checker.Parameter(!string.IsNullOrEmpty(dataStr), "dataStr can not be empty or null");
            Checker.Parameter(!string.IsNullOrEmpty(signature), "signature can not be empty or null");

            byte[] data = GlobalVariables.Encoding.GetBytes(dataStr);

            byte[] signatureBytes = new byte[signature.Length / 2];

            for (int i = 0; i < signature.Length; i++)
                signatureBytes[i] = Convert.ToByte(signature.Substring(i * 2, 2), 16);

            return VerifySignatureUsePublicKey(data, signatureBytes);
        }


        /// <summary>
        /// Verifies the signature use public key.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="signature">The signature.</param>
        /// <returns></returns>
        public bool VerifySignatureUsePublicKey(byte[] data, byte[] signature)
        {
            Checker.Parameter(data != null, "data can not be null");
            Checker.Parameter(signature != null, "signature can not be null");

            RSACryptoServiceProvider sign = (RSACryptoServiceProvider)_cert.PublicKey.Key;
            return sign.VerifyData(data, new System.Security.Cryptography.SHA1CryptoServiceProvider(), signature);
        }



        /// <summary>
        /// Verifies the signature use private key.
        /// </summary>
        /// <param name="dataStr">The data STR.</param>
        /// <param name="signature">The signature.</param>
        /// <returns></returns>
        public bool VerifySignatureUsePrivateKey(string dataStr, string signature)
        {
            Checker.Parameter(!string.IsNullOrEmpty(dataStr), "dataStr can not be empty or null");
            Checker.Parameter(!string.IsNullOrEmpty(signature), "signature can not be empty or null");

            byte[] data = GlobalVariables.Encoding.GetBytes(dataStr);

            byte[] signatureBytes = new byte[signature.Length / 2];

            for (int i = 0; i < signature.Length; i++)
                signatureBytes[i] = Convert.ToByte(signature.Substring(i * 2, 2), 16);

            return VerifySignatureUsePrivateKey(data, signatureBytes);
        }


        /// <summary>
        /// Verifies the signature use private key.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="signature">The signature.</param>
        /// <returns></returns>
        public bool VerifySignatureUsePrivateKey(byte[] data, byte[] signature)
        {
            Checker.Parameter(data != null, "data can not be null");
            Checker.Parameter(signature != null, "signature can not be null");

            Checker.Requires(_cert.HasPrivateKey, "certificate does not has private key");

            RSACryptoServiceProvider sign = (RSACryptoServiceProvider)_cert.PrivateKey;
            return sign.VerifyData(data, new System.Security.Cryptography.SHA1CryptoServiceProvider(), signature);
        }
    }
}
