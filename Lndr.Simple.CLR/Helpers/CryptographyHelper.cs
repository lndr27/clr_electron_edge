using Lndr.Simple.CLR.Helpers.Extensions;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Web.Security;

namespace Lndr.Simple.CLR.Helpers
{
    public static class CryptographyHelper
    {
        #region Constants +
        const int HEADER_LENGTH              = 50;
        const int RSA_RESULT_LENGTH          = 128;
        const int RSA_KEY_LENGTH             = 2048;
        const int PASSWORD_RANDOM_ITERATIONS = 1000;
        const int PASSWORD_LENGTH            = 64;
        const int SALT_LENGTH                = 32;
        #endregion

        #region Encryption +
        public static byte[] Encrypt(byte[] bytesToBeEncrypted, string publicKey)
        {
            return Encrypt(bytesToBeEncrypted, publicKey, string.Empty);
        }

        public static byte[] Encrypt(byte[] bytesToBeEncrypted, string publicKey, string encryptedBytesHeader)
        {
            Guard.ForArgumentNull(bytesToBeEncrypted, "bytesToBeEncrypted");
            Guard.ForArgumentNullOrEmpty(publicKey, "publicKey");
            Guard.ForArgumentNullOrEmpty(encryptedBytesHeader, "encryptedBytesHeader");

            var header            = encryptedBytesHeader.PadRight(HEADER_LENGTH, ' ').GetBytes();
            var password          = Membership.GeneratePassword(PASSWORD_LENGTH, PASSWORD_LENGTH / 2).GetBytes();
            var encryptedPassword = RSA_Encryption(password, publicKey);
            var salt              = GenerateRandomSalt();
            var encryptedSalt     = RSA_Encryption(salt, publicKey);
            var encryptedData     = AES_Encrypt(bytesToBeEncrypted, publicKey, header, password, salt);
            return header.Combine(encryptedPassword, encryptedSalt, encryptedData);
        }

        private static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, string publicKey, byte[] header, byte[] passwordBytes, byte[] saltBytes)
        {
            Guard.ForArgumentNull(bytesToBeEncrypted, "bytesToBeEncrypted");
            Guard.ForArgumentNullOrEmpty(publicKey, "publicKey");
            Guard.ForArgumentNull(header, "header");

            using (var ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, PASSWORD_RANDOM_ITERATIONS);
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    return ms.ToArray();
                }
            }
        }

        private static byte[] GenerateRandomSalt()
        {
            var data = new byte[SALT_LENGTH];

            using (var rng = new RNGCryptoServiceProvider())
            {
                for (int i = 0; i < 10; i++)
                {
                    rng.GetBytes(data);
                }
            }
            return data;
        }

        private static byte[] RSA_Encryption(byte[] input, string publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider(RSA_KEY_LENGTH))
            {
                try
                {
                    rsa.FromXmlString(publicKey);
                    return rsa.Encrypt(input, true);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
        #endregion

        #region Decryption +
        public static byte[] Decrypt(byte[] bytesToBeDecrypted, string privateKey)
        {
            var header = new byte[HEADER_LENGTH];
            var passwordBytesEncrypted = new byte[RSA_RESULT_LENGTH];
            var saltBytesEncrypted = new byte[RSA_RESULT_LENGTH];
            var encryptedData = new byte[bytesToBeDecrypted.Length - (RSA_RESULT_LENGTH * 2) - HEADER_LENGTH]; 

            Buffer.BlockCopy(bytesToBeDecrypted, 0, header, 0, header.Length);
            Buffer.BlockCopy(bytesToBeDecrypted, HEADER_LENGTH, passwordBytesEncrypted, 0, passwordBytesEncrypted.Length);
            Buffer.BlockCopy(bytesToBeDecrypted, HEADER_LENGTH + RSA_RESULT_LENGTH, saltBytesEncrypted, 0, saltBytesEncrypted.Length);
            Buffer.BlockCopy(bytesToBeDecrypted, HEADER_LENGTH + (RSA_RESULT_LENGTH * 2), encryptedData, 0, encryptedData.Length);

            var passwordBytes = RSA_Decryption(passwordBytesEncrypted, privateKey);
            var saltBytes = RSA_Decryption(saltBytesEncrypted, privateKey);

            return AES_Decrypt(encryptedData, passwordBytes, saltBytes, privateKey);
        }

        private static byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] saltBytes, string privateKey)
        {
            using (var ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, PASSWORD_RANDOM_ITERATIONS);
                    AES.KeySize = 256;
                    AES.BlockSize = 128;
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);
                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    return ms.ToArray();
                }
            }
        }

        private static byte[] RSA_Decryption(byte[] bytesToDecrypt, string privateKey)
        {
            using (var rsa = new RSACryptoServiceProvider(RSA_KEY_LENGTH))
            {
                try
                {
                    rsa.FromXmlString(privateKey);
                    return rsa.Decrypt(bytesToDecrypt, true);
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
        }
        #endregion
    }
}
