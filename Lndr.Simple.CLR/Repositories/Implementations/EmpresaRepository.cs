using Dapper;
using Dapper.Contrib.Extensions;
using Lndr.Simple.CLR.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Lndr.Simple.CLR.Repositories
{
    class EmpresaRepository : BaseRepository<Empresa>, IEmpresaRepository
    {
        public Empresa GetByCnpj(string cnpj)
        {
            using (var connection = base.GetDbConnection())
            {
                return connection.QueryFirst<Empresa>("SELECT * FROM Empresas WHERE CNPJ = @cnpj", new { cnpj });
            }
        }       
    }
}
