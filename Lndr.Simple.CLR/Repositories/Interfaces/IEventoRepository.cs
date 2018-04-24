using Lndr.Simple.CLR.Models.Entities;
using System.Collections.Generic;

namespace Lndr.Simple.CLR.Repositories
{
    interface IEventoRepository: IRepository<Evento>
    {
        List<Evento> ListEventosEmpresa(int idEmpresa);
    }
}
