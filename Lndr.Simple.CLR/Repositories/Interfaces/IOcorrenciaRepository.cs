using Lndr.Simple.CLR.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Lndr.Simple.CLR.Repositories
{
    interface IOcorrenciaRepository : IRepository<Ocorrencia>
    {
        List<Ocorrencia> ListOcorrenciasEvento(int idEvento);
    }
}
