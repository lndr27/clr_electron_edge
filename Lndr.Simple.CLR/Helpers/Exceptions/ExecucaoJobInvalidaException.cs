using System;
using System.Runtime.Serialization;

namespace Lndr.Simple.CLR.Helpers.Exceptions
{
    class ExecucaoJobInvalidaException : Exception
    {
        public ExecucaoJobInvalidaException()
        {
        }

        public ExecucaoJobInvalidaException(string message) : base(message)
        {
        }

        public ExecucaoJobInvalidaException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExecucaoJobInvalidaException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
