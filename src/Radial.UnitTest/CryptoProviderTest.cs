using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Radial.Security;
using System.Security.Cryptography;

namespace Radial.UnitTest
{
    [TestFixture]
    public class CryptoProviderTest
    {
        [Test]
        public void DES()
        {
            DESCryptoServiceProvider key=new DESCryptoServiceProvider();
            key.Key = Encoding.ASCII.GetBytes("12345678");
            key.IV = Encoding.ASCII.GetBytes("12345678");
            string st = CryptoProvider.DESEncrypt("abc", key);
            Assert.AreEqual("abc", CryptoProvider.DESDecrypt(st, key));
        }

        [Test]
        public void Rijndael()
        {
            RijndaelManaged key = new RijndaelManaged();
            key.Key = Encoding.ASCII.GetBytes("12345678123456781234567812345678");
            key.IV = Encoding.ASCII.GetBytes("1234567812345678");
            string st = CryptoProvider.RijndaelEncrypt("abc萨芬的", key);
            Assert.AreEqual("abc萨芬的", CryptoProvider.RijndaelDecrypt(st, key));
        }
    }
}
