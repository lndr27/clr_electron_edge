using System.Collections.Generic;

namespace Lndr.Simple.CLR.Helpers.Extensions
{
    static class ObjectExtensions
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
    }
}
