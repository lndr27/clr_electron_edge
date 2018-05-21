using Lndr.Simple.CLR.Models.Entities;
using Lndr.Simple.CLR.Models.Enums;
using Lndr.Simple.CLR.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lndr.Simple.CLR.Services.Implementations
{
    class JobService
    {
        private readonly IJobRepository _jobRepository;

        public JobService()
        {
            this._jobRepository = new JobRepository();
        }

        public void ComecarJob(TipoJobEnum tipoJob, int idEmpresa)
        {
            var job = this._jobRepository.GetByTipo((int)tipoJob, idEmpresa);
            if (job == null)
            {
                job = new Job
                {
                    DataInicio = DateTime.Now,
                    IdEmpresa = idEmpresa,
                    StatusJob = (int)StatusJobEnum.Executando
                };
                this._jobRepository.Add(job);

            }
            else if (job.StatusJob == (int)StatusJobEnum.Executando)
            {
                return;
            }
        }
    }
}
