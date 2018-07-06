using System;

namespace Lndr.Simple.CLR.Helpers
{
    static class Guard
    {
        public static void ForArgumentNullOrEmpty(string value, string argumentName)
        {
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(string.Format("Argumento {0} não pode ser nulo ou vazio", argumentName));
        }

        public static void ForArgumentNull(byte[] bytes, string argumentName)
        {
            if (bytes == null) throw new ArgumentNullException(string.Format("Argumento {0} não pode ser nulo", argumentName));
        }
    }
}
