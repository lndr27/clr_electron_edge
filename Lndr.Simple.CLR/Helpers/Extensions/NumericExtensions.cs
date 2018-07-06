using System.Linq;

namespace Lndr.Simple.CLR.Helpers.Extensions
{
    static class NumericExtensions
    {
        public static bool In(this int value, params int[] values)
        {
            return !values.IsNullOrEmpty() && values.Contains(value);
        }

        public static bool NotIn(this int value, params int[] values)
        {
            return !value.In(values);
        }
    }
}
