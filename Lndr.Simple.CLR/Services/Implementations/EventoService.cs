using Lndr.Simple.CLR.Models;
using Lndr.Simple.CLR.Models.Entities;
using Lndr.Simple.CLR.Models.Enums;
using Lndr.Simple.CLR.Repositories;
using Lndr.Simple.CLR.Services.Implementations;
using Lndr.Simple.CLR.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lndr.Simple.CLR.Helpers;

namespace Lndr.Simple.CLR.Services
{
    class EventoService : BaseService, IEventoService
    {
        private IJobService _jobService;

        public EventoService()
        {
            this._jobService = new JobService();
        }

        #region EFinanceira
        public void AdicionarLote(Empresa empresa, EFinanceiraLote lote)
        {
            empresa.Id = this.SalvarEmpresa(empresa);
            lote.IdEmpresa = empresa.Id;
            if (!base.EfinanceiraLoteRepository.LoteExists(lote.IdLote))
            {
                base.EfinanceiraLoteRepository.Add(lote);
            }            
        }
        #endregion

        public int AdicionarPacoteEventos(ArquivoEntradaDTO arquivo)
        {
            var idEmpresa = this.SalvarEmpresa(arquivo.Empresa);
            return this.SalvarListaEventos(arquivo.Eventos, idEmpresa);
        }

        public int SalvarEmpresa(Empresa empresa)
        {
            var empresaBase = base.EmpresaRepository.GetByEntidade(empresa.Entidade);
            if (empresaBase == null)
            {
                return base.EmpresaRepository.Add(empresa);
            }
            return empresaBase.Id;
        }

        public int SalvarListaEventos(List<Evento> eventos, int idEmpresa)
        {
            var novosEventos = 0;
            var dataAtualizacao = DateTime.Now;

            foreach (var evento in eventos)
            {
                var eventoBase = base.EventoRespository.GetByIdEvento(evento.IdEvento);
                if (eventoBase == null)
                {
                    base.EventoRespository.Add(new Evento
                    {
                        IdEmpresa              = idEmpresa,
                        DataUpload             = dataAtualizacao,
                        IdEvento               = evento.IdEvento,
                        StatusEvento           = (int)StatusEventoEnum.Novo,
                        TipoEvento             = evento.TipoEvento,
                        DataAtualizacao        = dataAtualizacao,
                        XmlEvento              = evento.XmlEvento
                    });
                    novosEventos++;
                }
            }
            return novosEventos;
        }

        public void ComecarAssinarEventos(int idEmpresa)
        {
            if (this._jobService.GetStatusJob(TipoJobEnum.Assinatura, idEmpresa) == StatusJobEnum.Executando)
            {
                throw new ExecucaoJobInvalidaException("Assinatura em andamento");
            }
            this._jobService.ComecarJob(TipoJobEnum.Assinatura, idEmpresa);

            var evento = base.EventoRespository.GetEventoNaoAssinados(idEmpresa);
            while (evento != null && StatusJobEnum.Executando == this._jobService.GetStatusJob(TipoJobEnum.Assinatura, idEmpresa))
            {
                evento.XmlEvento = AssinadorXml.Assinar(evento.XmlEvento);
                base.EventoRespository.Update(evento);
                evento = base.EventoRespository.GetEventoNaoAssinados(idEmpresa);
            }
            this._jobService.AtualizarStatusJob(TipoJobEnum.Assinatura, idEmpresa, StatusJobEnum.Concluido);
        }

        public void AtualizarStatusAssinatura(int idEmpresa, StatusJobEnum status)
        {
            this._jobService.AtualizarStatusJob(TipoJobEnum.Assinatura, idEmpresa, status);
        }
    }
}
