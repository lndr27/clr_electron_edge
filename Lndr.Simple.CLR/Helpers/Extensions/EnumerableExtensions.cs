using System.Collections.Generic;
using System.Linq;

namespace Lndr.Simple.CLR.Helpers.Extensions
{
    static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> a)
        {
            return a != null && a.Any();
        }
    }
}
