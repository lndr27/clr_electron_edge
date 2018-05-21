using System;

namespace Lndr.Simple.CLR.Models.Entities
{
    class Job
    {
        public int Id { get; set; }

        public int IdEmpresa { get; set; }

        public int Tipo { get; set; }

        public int StatusJob { get; set; }

        public DateTime DataInicio { get; set; }

        public DateTime DataFim { get; set; }

        public DateTime DataAtualizacao { get; set; }
    }
}
