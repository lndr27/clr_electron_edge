using Dapper;
using Dapper.Contrib.Extensions;
using Lndr.Simple.CLR.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Lndr.Simple.CLR.Repositories
{
    class OcorrenciaRepository : BaseRepository, IOcorrenciaRepository
    {
        public int Add(Ocorrencia ocorrencia)
        {
            using (var connection = base.GetDbConnection())
            {
                return (int)connection.Insert(ocorrencia);
            }
        }

        public Ocorrencia Get(int id)
        {
            using (var connection = base.GetDbConnection())
            {
                return connection.Get<Ocorrencia>(id);
            }
        }

        public List<Ocorrencia> List()
        {
            using (var connection = base.GetDbConnection())
            {
                return connection.Query<Ocorrencia>(@"SELECT * FROM Ocorrencias").ToList();
            }
        }

        public List<Ocorrencia> ListOcorrenciasEvento(int idEvento)
        {
            using (var connection = base.GetDbConnection())
            {
                return connection.Query<Ocorrencia>(@"SELECT * FROM Ocorrencias WHERE idEvento = @idEvento", new { idEvento }).ToList();
            }
        }

        public void Update(Ocorrencia ocorrencia)
        {
            using (var connection = base.GetDbConnection())
            {
                SqlMapperExtensions.Update(connection, ocorrencia);
            }
        }
    }
}
