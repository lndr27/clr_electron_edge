using Lndr.Simple.CLR.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SQLite;
using Dapper.Contrib.Extensions;

namespace Lndr.Simple.CLR.Repositories
{
    class JobRepository : BaseRepository<Job>, IJobRepository
    {
        public Job GetByTipo(int tipoJob, int idEmpresa)
        {
            using (var conn = base.GetDbConnection())
            {
                return conn.QueryFirst<Job>(@"
SELECT *
FROM Jobs
WHERE   IdEmpresa = @idEmpresa
    AND Tipo = @tipoJob
", new { tipoJob, idEmpresa });
            }
        }
    }
}
