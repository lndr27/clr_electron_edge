using Dapper;
using Lndr.Simple.CLR.Models.Entities;

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
    AND Tipo = @tipoJob", new { tipoJob, idEmpresa });
            }
        }
    }
}
