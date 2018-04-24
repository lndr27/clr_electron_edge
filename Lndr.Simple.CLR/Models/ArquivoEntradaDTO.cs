using Lndr.Simple.CLR.Models.Entities;
using System.Collections.Generic;

namespace Lndr.Simple.CLR.Models
{
    class ArquivoEntradaDTO
    {
        public Empresa Empresa { get; set; }

        public List<Evento> Eventos { get; set; }
    }
}
