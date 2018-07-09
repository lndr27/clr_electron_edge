using System;
using System.Text;
using System.Linq;
using Ionic.Zip;
using System.IO;
using System.Collections.Generic;

namespace Lndr.Simple.CLR.Helpers.Extensions
{
    public static class ByteArrayExtensions
    {
        public static string ToBase64String(this byte[] b)
        {
            return Convert.ToBase64String(b);
        }

        public static string GetString(this byte[] b)
        {
            return b.GetString<UTF8Encoding>();
        }

        public static string GetString<TEncoding>(this byte[] b)
            where TEncoding : Encoding, new()
        {
            return (new TEncoding()).GetString(b);
        }

        public static byte[] Combine(this byte[] b, params byte[][] arrays)
        {
            var rv = new byte[arrays.Sum(a => a.Length) + b.Length];
            System.Buffer.BlockCopy(b, 0, rv, 0, b.Length);
            int offset = b.Length;
            foreach (byte[] array in arrays)
            {
                System.Buffer.BlockCopy(array, 0, rv, offset, array.Length);
                offset += array.Length;
            }
            return rv;
        }

        public static byte[] Zip(this byte[] bytes)
        {
            Guard.ForArgumentNull(bytes, "bytes");

            using (var ms = new MemoryStream())
            using (var zip = new ZipFile())
            {
                zip.AddEntry(Guid.NewGuid().ToString(), bytes);
                zip.Save(ms);
                return ms.ToArray();
            }
        }

        public static byte[] Unzip(this byte[] bytes)
        {
            Guard.ForArgumentNull(bytes, "bytes");

            using (var input = new MemoryStream(bytes))
            using (var output = new MemoryStream())
            using (var zip = ZipFile.Read(input))
            {
                var outFile = zip.FirstOrDefault();
                if (outFile != null)
                {
                    outFile.Extract(output);
                    return output.ToArray();
                }
                return null;
            }
        }

        public static IEnumerable<byte[]> UnzipAll(this byte[] bytes)
        {
            Guard.ForArgumentNull(bytes, "bytes");

            var result = new List<byte[]>();

            using (var input = new MemoryStream(bytes))
            using (var output = new MemoryStream())
            using (var zip = ZipFile.Read(input))
            {
                foreach (var entry in zip)
                {
                    entry.Extract(output);
                    result.Add(output.ToArray());
                    output.Seek(0, SeekOrigin.Begin);
                }
                return result;
            }
        }

        public static byte[] Zip(this IEnumerable<byte[]> bytes)
        {
            using (var ms = new MemoryStream())
            using (var zip = new ZipFile())
            {
                foreach (var b in bytes)
                {
                    zip.AddEntry(Guid.NewGuid().ToString(), b);
                }
                zip.Save(ms);
                return ms.ToArray();
            }
        }

        public static byte[] Encrypt(this byte[] bytes, string publicKey)
        {
            return CryptographyHelper.Encrypt(bytes, publicKey);
        }

        public static byte[] Encrypt(this byte[] bytes, string pubilcKey, string header)
        {
            return CryptographyHelper.Encrypt(bytes, pubilcKey, header);
        }

        public static byte[] Descrypt(this byte[] bytes, string privateKey)
        {
            return CryptographyHelper.Decrypt(bytes, privateKey);
        }

        public static bool IsZipFile(this byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {
                return ZipFile.IsZipFile(ms, true);
            }                
        }
    }
}
