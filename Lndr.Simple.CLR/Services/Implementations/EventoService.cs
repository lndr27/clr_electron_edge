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
    class EventoService : IEventoService
    {
        #region Campos e Ctor +
        private readonly JobService _jobService;

        private readonly IEmpresaRepository _empresaRepository;

        private readonly IEventoRepository _eventoRepository;

        public EventoService()
        {
            this._empresaRepository = new EmpresaRepository();
            this._eventoRepository = new EventoRepository();
            this._jobService = new JobService();
        }
        #endregion

        public int AdicionarPacoteEventos(ArquivoEntradaDTO arquivo)
        {
            var idEmpresa = this.SalvarEmpresa(arquivo.Empresa);
            return this.SalvarListaEventos(arquivo.Eventos, idEmpresa);
        }

        public int SalvarEmpresa(Empresa empresa)
        {
            var empresaBase = this._empresaRepository.GetByCnpj(empresa.CNPJ);
            if (empresaBase == null)
            {
                return this._empresaRepository.Add(empresa);
            }
            return empresaBase.Id;
        }

        public int SalvarListaEventos(List<Evento> eventos, int idEmpresa)
        {
            var novosEventos = 0;
            var dataAtualizacao = DateTime.Now;

            foreach (var evento in eventos)
            {
                var eventoBase = this._eventoRepository.GetByIdEvento(evento.IdEvento);
                if (eventoBase == null)
                {
                    this._eventoRepository.Add(new Evento
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

            var evento = this._eventoRepository.GetEventoNaoAssinados(idEmpresa);
            while (evento != null && StatusJobEnum.Executando == this._jobService.GetStatusJob(TipoJobEnum.Assinatura, idEmpresa))
            {
                evento.XmlEvento = AssinadorXml.Assinar(evento.XmlEvento);
                this._eventoRepository.Update(evento);
                evento = this._eventoRepository.GetEventoNaoAssinados(idEmpresa);
            }
            this._jobService.AtualizarStatusJob(TipoJobEnum.Assinatura, idEmpresa, StatusJobEnum.Concluido);
        }

        public void AtualizarStatusAssinatura(int idEmpresa, StatusJobEnum status)
        {
            this._jobService.AtualizarStatusJob(TipoJobEnum.Assinatura, idEmpresa, status);
        }
    }
}
