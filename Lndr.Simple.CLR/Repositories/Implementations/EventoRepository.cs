using Dapper;
using Dapper.Contrib.Extensions;
using Lndr.Simple.CLR.Models.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Lndr.Simple.CLR.Repositories
{
    class EventoRepository : BaseRepository, IRepository<Evento>
    {
        public int Add(Evento evento)
        {
            using (var connection = base.GetDbConnection())
            {
                return (int)connection.Insert(evento);
            }
        }

        public Evento Get(int id)
        {
            using (var connection = base.GetDbConnection())
            {
                return connection.Get<Evento>(id);
            }
        }

        public List<Evento> List()
        {
            using (var connection = base.GetDbConnection())
            {
                return connection.Query<Evento>(@"SELECT * FROM Eventos").ToList();
            }
        }

        public void Update(Evento evento)
        {
            using (var connection = base.GetDbConnection())
            {
                SqlMapperExtensions.Update(connection, evento);
            }
        }

        public List<Evento> ListEventosEmpresa(int idEmpresa)
        {
            using (var connection = base.GetDbConnection())
            {
                return connection.Query<Evento>(@"SELECT e.* FROM Eventos e WHERE idEmpresa = @idEmpresa", new { idEmpresa }).ToList();
            }
        }

        public Evento GetByIdEvento(string idEvento)
        {
            using (var connection = base.GetDbConnection())
            {
                return connection.QueryFirstOrDefault<Evento>(@"SELECT * FROM Eventos WHERE IdEvento = @idEvento", new { idEvento });
            }
        }
    }
}
