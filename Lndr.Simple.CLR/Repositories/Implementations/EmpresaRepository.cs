using Dapper;
using Dapper.Contrib.Extensions;
using Lndr.Simple.CLR.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Lndr.Simple.CLR.Repositories
{
    class EmpresaRepository : BaseRepository, IEmpresaRepository
    {
        public List<Empresa> List()
        {
            using (var connection = base.GetDbConnection())
            {
                return connection.Query<Empresa>("SELECT * FROM Empresas").ToList();
            }
        }

        public Empresa Get(int id)
        {
            using (var connection = base.GetDbConnection())
            {
                return connection.Get<Empresa>(id);
            }
        }

        public Empresa GetByCnpj(string cnpj)
        {
            using (var connection = base.GetDbConnection())
            {
                return connection.QueryFirst<Empresa>("SELECT * FROM Empresas WHERE CNPJ = @cnpj", new { cnpj });
            }
        }

        public int Add(Empresa empresa)
        {
            using (var connection = base.GetDbConnection())
            {
                return (int)connection.Insert(empresa);
            }
        }

        public void Update(Empresa empresa)
        {
            using (var connection = base.GetDbConnection())
            {
                SqlMapperExtensions.Update(connection, empresa);
            }
        }        
    }
}
