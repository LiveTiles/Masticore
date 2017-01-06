using Microsoft.VisualStudio.TestTools.UnitTesting;
using Masticore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Masticore.Tests
{
    /// <summary>
    /// Unit tests for the StringEncryption class
    /// </summary>
    [TestClass()]
    public class StringEncryptionTests
    {
        /// <summary>
        /// Unit test for encrypting a string
        /// </summary>
        [TestMethod()]
        public void EncryptTest()
        {
            string plainText = "Hello World";
            string passPhrase = "Key";

            string encryptedText = StringEncryption.Encrypt(plainText, passPhrase);

            Assert.IsNotNull(encryptedText);
        }

        /// <summary>
        /// Unit test for ecrypting then decrypting strings
        /// </summary>
        [TestMethod()]
        public void DecryptTest()
        {
            string plainText = "Hello World";
            string passPhrase = "Key";

            string encryptedText = StringEncryption.Encrypt(plainText, passPhrase);

            string decryptedText = StringEncryption.Decrypt(encryptedText, passPhrase);

            Assert.IsTrue(plainText == decryptedText);
        }
    }
}