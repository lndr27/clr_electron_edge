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
                return connection.QueryFirstOrDefault<Empresa>("SELECT * FROM Empresas WHERE CNPJ = @cnpj", new { cnpj });
            }
        }

        public Empresa GetByEntidade(int entidade)
        {
            using (var connection = base.GetDbConnection())
            {
                return connection.QueryFirstOrDefault<Empresa>("SELECT * FROM Empresas WHERE Entidade = @entidade", new { entidade });
            }
        }

        public bool EmpresaExists(int entidade)
        {
            using (var connection = base.GetDbConnection())
            {
                return connection.ExecuteScalar<bool>("SELECT 1 FROM Empresas WHERE Entidade = @entidade", new { entidade });
            }
        }
    }
}
