using Lndr.Simple.CLR.Models.Enums;
using System;

namespace Lndr.Simple.CLR.Models.Entities
{
    class Job
    {
        public int Id { get; set; }

        public int EmpresaId { get; set; }

        public DateTime DataCriacao { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public DateTime DataFinalizacao { get; set; }

        public StatusJobEnum StatusJob { get; set;  }
    }
}
