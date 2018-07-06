using System;
using System.Text;
using System.Linq;

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

    }
}
