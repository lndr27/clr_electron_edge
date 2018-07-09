using Lndr.Simple.CLR.Models.Enums;

namespace Lndr.Simple.CLR.Services
{
    interface IJobService
    {
        void ComecarJob(TipoJobEnum tipoJob, int idEmpresa);

        StatusJobEnum GetStatusJob(TipoJobEnum tipojob, int idEmpresa);

        void AtualizarStatusJob(TipoJobEnum tipojob, int idEmpresa, StatusJobEnum status);
    }
}
