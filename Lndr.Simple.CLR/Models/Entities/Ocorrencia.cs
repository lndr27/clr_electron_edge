namespace Lndr.Simple.CLR.Models.Entities
{
    class Ocorrencia
    {
        public int Id { get; set; }

        public int IdEvento { get; set; }

        public int TipoOcorrencia { get; set; }

        public string Descricao { get; set; }

        public string Localizacao { get; set; }

        public string Codigo { get; set; }
    }
}
