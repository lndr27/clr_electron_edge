using Newtonsoft.Json;
using System;
using System.Text;

namespace Lndr.Simple.CLR.Helpers.Extensions
{
    public static class StringExtensions
    {
        public static byte[] GetBytes(this string str)
        {
            return str.GetBytes<UTF8Encoding>();
        }

        public static byte[] GetBytes<TEncoding>(this string str) 
            where TEncoding : Encoding, new()
        {
            return (new TEncoding()).GetBytes(str);
        }

        public static byte[] FromBase64String(this string str)
        {
            return Convert.FromBase64String(str);
        }

        public static TResult DeserializeObject<TResult>(this string str)
            where TResult : new()
        {
            return JsonConvert.DeserializeObject<TResult>(str);
        }
    }
}
