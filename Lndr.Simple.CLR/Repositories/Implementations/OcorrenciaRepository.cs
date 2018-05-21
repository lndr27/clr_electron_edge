using Dapper;
using Dapper.Contrib.Extensions;
using Lndr.Simple.CLR.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Lndr.Simple.CLR.Repositories
{
    class OcorrenciaRepository : BaseRepository<Ocorrencia>, IOcorrenciaRepository
    {        
        public List<Ocorrencia> ListOcorrenciasEvento(int idEvento)
        {
            using (var connection = base.GetDbConnection())
            {
                return connection.Query<Ocorrencia>(@"SELECT * FROM Ocorrencias WHERE idEvento = @idEvento", new { idEvento }).ToList();
            }
        }
    }
}
