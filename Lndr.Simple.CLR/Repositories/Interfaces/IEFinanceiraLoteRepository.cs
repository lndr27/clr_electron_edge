using Lndr.Simple.CLR.Models.Entities;

namespace Lndr.Simple.CLR.Repositories
{
    interface IEFinanceiraLoteRepository : IRepository<EFinanceiraLote>
    {
        bool LoteExists(int idLote);
    }
}
