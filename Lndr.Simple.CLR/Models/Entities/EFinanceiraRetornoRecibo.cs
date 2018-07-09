using System;

namespace Lndr.Simple.CLR.Models.Entities
{
    class EFinanceiraRetornoRecibo
    {
        public int Id { get; set; }

        public int IdEmpresa { get; set; }

        public int IdLote { get; set; }

        public int IdEvento { get; set; }

        public int TipoEvento { get; set; }

        public DateTime DataProcessamento { get; set; }

        public string NumeroRecibo { get; set; }
    }
}
