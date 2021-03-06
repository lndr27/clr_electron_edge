﻿using Lndr.Simple.CLR.Models.Entities;

namespace Lndr.Simple.CLR.Repositories
{
    interface IEmpresaRepository : IRepository<Empresa>
    {
        Empresa GetByCnpj(string cnpj);

        Empresa GetByEntidade(int entidade);

        bool EmpresaExists(int entidade);
    }
}
