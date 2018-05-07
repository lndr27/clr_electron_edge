using System;

namespace Lndr.Simple.CLR.Helpers
{
    static class Guard
    {
        public static void ArgumentNullOrEmpty(string value, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(string.Format("Argumento {0} não pode ser nulo ou vazio", argumentName));
        }
    }
}
