using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            var plainText = "Hello World";
            var passPhrase = "Key";

            var encryptedText = StringEncryption.Encrypt(plainText, passPhrase);

            Assert.IsNotNull(encryptedText);
        }

        /// <summary>
        /// Unit test for ecrypting then decrypting strings
        /// </summary>
        [TestMethod()]
        public void DecryptTest()
        {
            var plainText = "Hello World";
            var passPhrase = "Key";

            var encryptedText = StringEncryption.Encrypt(plainText, passPhrase);

            var decryptedText = StringEncryption.Decrypt(encryptedText, passPhrase);

            Assert.IsTrue(plainText == decryptedText);
        }
    }
}