namespace Lndr.Simple.CLR.Models.Dto
{
    public class ArquivoComunicacaoEFinanceiraDto
    {
        public int Entidade { get; set; }

        public string EntidadeNome { get; set; }

        public string EntidadeCnpj { get; set; }

        public int Ano { get; set; }

        public int Semestre { get; set; }

        public int TipoEventoId { get; set; }

        public int LoteId { get; set; }

        public string LoteXmlBase64 { get; set; }
    }
}
