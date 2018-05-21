using Lndr.Simple.CLR.Models.Entities;
using System.Collections.Generic;

namespace Lndr.Simple.CLR.Services
{
    interface IEventoService
    {
        int SalvarEmpresa(Empresa empresa);

        int SalvarListaEventos(List<Evento> eventos, int idEmpresa);
    }
}
