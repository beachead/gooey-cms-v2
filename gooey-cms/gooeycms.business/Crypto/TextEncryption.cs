using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace gooeycms.business.Crypto
{
    /// <summary>
    /// Encryption object which will also base-64 encode the data to allow it to be 
    /// safely stored in a webpage without weird formatting issues and encoding issues.
    /// 
    /// Code used with permission from:
    /// Copyright (C) 2002 Obviex(TM). All rights reserved.
    /// </summary>
    public class TextEncryption
    {
        private const String key = "!@#ASDFHJASALKJAVLDKJ#Q$akfjalkfdsjhasf823t9uhalkdjfas;faw";
        private const String initVector = "qweasdtyj687iofb";
        private const int keySize = 128;

        private String salt = "!@34ghbn  ak''][3ga";
        public TextEncryption()
        {
        }

        /// <summary>
        /// Creates a new encryption object that overrides the default salt value
        /// </summary>
        /// <param name="salt"></param>
        public TextEncryption(String salt)
        {
            this.salt = salt;
        }

        /// <summary>
        /// Encrypts a plain-text string and returns the base 64 encoded string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public String Encrypt(String text)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(text);

            PasswordDeriveBytes password = new PasswordDeriveBytes(key,saltValueBytes,"SHA1",1);
            byte[] keyBytes = password.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes,initVectorBytes);

            
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,encryptor,CryptoStreamMode.Write);
            byte[] cipherTextBytes = null;
            try
            {
                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                cryptoStream.FlushFinalBlock();
                cipherTextBytes = memoryStream.ToArray();
            }
            finally
            {
                memoryStream.Close();
                cryptoStream.Close();
            }

            string cipherText = Convert.ToBase64String(cipherTextBytes);
            return cipherText;
        }

        /// <summary>
        /// Decrypts a string which was encrypted using this class
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public String Decrypt(String text)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(salt);
            byte[] cipherTextBytes = Convert.FromBase64String(text);

            PasswordDeriveBytes password = new PasswordDeriveBytes(key,saltValueBytes,"SHA1",1);
            byte[] keyBytes = password.GetBytes(keySize / 8);

            RijndaelManaged symmetricKey = new RijndaelManaged();
            symmetricKey.Mode = CipherMode.CBC;

            ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes,initVectorBytes);
            MemoryStream memoryStream = new MemoryStream(cipherTextBytes);
            CryptoStream cryptoStream = new CryptoStream(memoryStream,decryptor,CryptoStreamMode.Read);

            byte[] plainTextBytes = new byte[cipherTextBytes.Length];
            int decryptedByteCount = cryptoStream.Read(plainTextBytes,0,plainTextBytes.Length);

            memoryStream.Close();
            cryptoStream.Close();

            string plainText = Encoding.UTF8.GetString(plainTextBytes,0,decryptedByteCount);
            return plainText;
        }
    }
}
