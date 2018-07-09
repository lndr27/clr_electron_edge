using Dapper;
using Dapper.Contrib.Extensions;
using Lndr.Simple.CLR.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Lndr.Simple.CLR.Repositories
{
    class EFinanceiraLoteRepository: BaseRepository<EFinanceiraLote>, IEFinanceiraLoteRepository
    {
        public bool LoteExists(int idLote)
        {
            using (var conn = base.GetDbConnection())
            {
                return conn.ExecuteScalar<bool>("SELECT 1 FROM EFinanceiraLotes WHERE IdLote = @idLote", new { idLote });
            }
        }
    }
}
