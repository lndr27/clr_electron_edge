using System.Collections.Generic;

namespace Lndr.Simple.CLR.Repositories
{
    interface IRepository<TEntity>
    {
        TEntity Get(int id);

        List<TEntity> List();

        int Add(TEntity entity);

        void Update(TEntity entity);        
    }
}
