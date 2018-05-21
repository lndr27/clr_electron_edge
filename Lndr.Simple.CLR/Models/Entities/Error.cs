using System;

namespace Lndr.Simple.CLR.Models.Entities
{
    class Error
    {
        public int Id { get; set; }

        public string Tipo { get; set; }

        public string Mensagem { get; set; }

        public string Stacktrace { get; set; }

        public DateTime Data { get; set; }
    }
}
