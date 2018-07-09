using System;

namespace Lndr.Simple.CLR.Models.Entities
{
    class EFinanceiraLote
    {
        public int Id { get; set; }

        public int IdEmpresa { get; set; }

        public int IdLote { get; set; }

        public byte[] XmlLote { get; set; }

        public int TipoEvento { get; set; }

        public DateTime DataUpload { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public int StatusEvento { get; set; }
    }
}
