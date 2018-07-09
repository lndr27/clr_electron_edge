using Newtonsoft.Json;
using System.Collections.Generic;

namespace Lndr.Simple.CLR.Helpers.Extensions
{
    public static class ObjectExtensions
    {
        public static string[] ToStringArray(this object obj)
        {
            var retorno = new List<string>();
            var list = obj as System.Collections.IEnumerable;

            if (list != null)
            {
                foreach (var el in list)
                {
                    retorno.Add(el as string);
                }
            }
            return retorno.ToArray();
        }

        public static string ToJSON(this object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }
}
