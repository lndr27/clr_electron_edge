using Lndr.Simple.CLR.Repositories;

namespace Lndr.Simple.CLR.Services
{
    abstract class BaseService
    {
        private IEmpresaRepository _empresaRepository;

        protected IEmpresaRepository EmpresaRepository
        {
            get
            {
                return this._empresaRepository = this._empresaRepository ?? new EmpresaRepository();
            }
        }

        private IEFinanceiraLoteRepository _efinanceiraLoteRepository;

        protected IEFinanceiraLoteRepository EfinanceiraLoteRepository
        {
            get
            {
                return this._efinanceiraLoteRepository = this._efinanceiraLoteRepository ?? new EFinanceiraLoteRepository();
            }
        }

        private IJobRepository _jobRepository;

        protected IJobRepository JobRepository
        {
            get
            {
                return this._jobRepository = this._jobRepository ?? new JobRepository();
            }
        }

        private IEventoRepository _eventoRepository;

        protected IEventoRepository EventoRespository
        {
            get
            {
                return this._eventoRepository = this._eventoRepository ?? new EventoRepository();
            }
        }
    }
}
