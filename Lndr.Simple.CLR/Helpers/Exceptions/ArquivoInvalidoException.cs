using System;
using System.Runtime.Serialization;

namespace Lndr.Simple.CLR.Helpers.Exceptions
{
    public class ArquivoInvalidoException : Exception
    {
        public ArquivoInvalidoException()
            : base ("Arquivo inválido")
        {
        }

        public ArquivoInvalidoException(string message) : base(message)
        {
        }

        public ArquivoInvalidoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ArquivoInvalidoException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
