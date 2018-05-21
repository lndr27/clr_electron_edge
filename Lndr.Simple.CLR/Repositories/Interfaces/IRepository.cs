using System.Collections.Generic;

namespace Lndr.Simple.CLR.Repositories
{
    interface IRepository<TEntity>
    {
        TEntity Get(int id);

        List<TEntity> GetAll();

        int Add(TEntity entity);

        void Update(TEntity entity);        
    }
}
