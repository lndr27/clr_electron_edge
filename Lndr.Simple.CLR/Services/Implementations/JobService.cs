using Lndr.Simple.CLR.Helpers.Extensions;
using Lndr.Simple.CLR.Models.Entities;
using Lndr.Simple.CLR.Models.Enums;
using System;

namespace Lndr.Simple.CLR.Services
{
    class JobService : BaseService, IJobService
    {
        public void ComecarJob(TipoJobEnum tipoJob, int idEmpresa)
        {
            var job = base.JobRepository.GetByTipo((int)tipoJob, idEmpresa);
            if (job == null)
            {
                job = new Job
                {
                    DataInicio = DateTime.Now,
                    IdEmpresa = idEmpresa,
                    StatusJob = (int)StatusJobEnum.Executando
                };
                base.JobRepository.Add(job);

            }
            else if (job.StatusJob.In(
                (int)StatusJobEnum.Concluido,
                (int)StatusJobEnum.Cancelado,
                (int)StatusJobEnum.Parado
            ))
            {
                job.StatusJob = (int)StatusJobEnum.Executando;
                job.DataInicio = DateTime.Now;
                job.DataAtualizacao = job.DataInicio;
                base.JobRepository.Update(job);
            }
        }

        public StatusJobEnum GetStatusJob(TipoJobEnum tipojob, int idEmpresa)
        {
            var job = base.JobRepository.GetByTipo((int)tipojob, idEmpresa);
            return job != null ? (StatusJobEnum)job.StatusJob : StatusJobEnum.Inexistente;
        }

        public void AtualizarStatusJob(TipoJobEnum tipojob, int idEmpresa, StatusJobEnum status)
        {
            var job = base.JobRepository.GetByTipo((int)tipojob, idEmpresa);
            if (job != null)
            {
                job.StatusJob = (int)status;
                job.DataAtualizacao = DateTime.Now;
                if (((int)status).In(
                    (int)StatusJobEnum.Concluido,
                    (int)StatusJobEnum.Cancelado
                ))
                {
                    job.DataFim = job.DataAtualizacao;
                }
                base.JobRepository.Update(job);
            }
        }
    }
}
