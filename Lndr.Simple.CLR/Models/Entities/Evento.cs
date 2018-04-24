using System;

namespace Lndr.Simple.CLR.Models.Entities
{
    class Evento
    {
        public int Id { get; set; }

        public int IdEmpresa { get; set; }

        public int TipoEvento { get; set; }

        public DateTime DataUpload { get; set; }

        public DateTime DataAtualizacao { get; set; }

        public string IdEvento { get; set; }

        public string NumeroRecibo { get; set; }

        public string EventoBase64Encriptado { get; set; }

        public int StatusEvento { get; set; }
    }
}
