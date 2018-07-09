using System;

namespace Lndr.Simple.CLR.Models.Entities
{
    class EFinanceiraRetornoErro
    {
        public int Id { get; set; }

        public int IdEmpresa { get; set; }

        public int IdLote { get; set; }

        public int IdEvento { get; set; }

        public string Codigo { get; set; }

        public string Descricao { get; set; }

        public string Localizacao { get; set; }

        public DateTime DataProcessamento { get; set; }

        public int TipoOcorrencia { get; set; }
    }
}
