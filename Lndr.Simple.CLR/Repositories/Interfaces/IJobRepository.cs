using Lndr.Simple.CLR.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lndr.Simple.CLR.Repositories
{
    interface IJobRepository : IRepository<Job>
    {
        Job GetByTipo(int tipoJob, int idEmpresa);
    }
}
