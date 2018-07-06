using Lndr.Simple.CLR.Models.Enums;
using Lndr.Simple.CLR.Helpers.Extensions;
using System;

namespace Lndr.Simple.CLR.Models.Entities
{
    class Evento
    {
        public int Id { get; set; }

        public int IdEmpresa { get; set; }

        public int TipoEvento { get; set; }

        public string TipoEventoDescricao
        {
            get
            {
                return ((TipoEventoEnum)this.TipoEvento).GetEnumDescription();
            }
        }

        public DateTime DataUpload { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public string IdEvento { get; set; }

        public string NumeroRecibo { get; set; }

        public string XmlEvento { get; set; }

        public int StatusEvento { get; set; }

        public string StatusEventoDescricao
        {
            get
            {
                return ((StatusEventoEnum)this.StatusEvento).GetEnumDescription();
            }
        }
    }
}
