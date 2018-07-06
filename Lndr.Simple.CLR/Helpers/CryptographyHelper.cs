using Lndr.Simple.CLR.Helpers.Extensions;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Web.Security;

namespace Lndr.Simple.CLR.Helpers
{
    public static class CryptographyHelper
    {
        public static byte[] AES_Encrypt(byte[] bytesToBeEncrypted, string publicKey, byte[] encryptedBytesHeader = null)
        {
            Guard.ForArgumentNull(bytesToBeEncrypted, "bytesToBeEncrypted");
            Guard.ForArgumentNullOrEmpty(publicKey, "publicKey");

            var header = new byte[50];
            if (encryptedBytesHeader != null)
            {
                System.Buffer.BlockCopy(encryptedBytesHeader, 0, header, 0, encryptedBytesHeader.Length);
            }
            else
            {
                Array.Clear(header, 0, header.Length);
            }

            using (var ms = new MemoryStream())
            {
                using (var AES = new RijndaelManaged())
                {
                    var randomPasswordBytes = Membership.GeneratePassword(64, 32).GetBytes();
                    var saltBytes = GenerateRandomSalt();
                    var key = new Rfc2898DeriveBytes(randomPasswordBytes, saltBytes, 1000);

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

                    var encryptedPassword = RSA_Encryption(randomPasswordBytes, publicKey);
                    var encryptedSalt = RSA_Encryption(saltBytes, publicKey);
                    
                    return encryptedBytesHeader.Combine(encryptedPassword, encryptedSalt, ms.ToArray());
                }
            }
        }

        private static byte[] GenerateRandomSalt()
        {
            var data = new byte[32];

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
            using (var rsa = new RSACryptoServiceProvider(2048))
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
    }
}
