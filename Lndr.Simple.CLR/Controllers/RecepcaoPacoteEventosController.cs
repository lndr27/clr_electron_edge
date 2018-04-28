using Lndr.Simple.CLR.Helpers.Extensions;
using Lndr.Simple.CLR.Models;
using Lndr.Simple.CLR.Models.Entities;
using Lndr.Simple.CLR.Models.Enums;
using Lndr.Simple.CLR.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Lndr.Simple.CLR.Controllers
{
    public class EventosController : BaseController
    {
        public async Task<object> ListarEmpresasAsync(dynamic input)
        {
            return new EmpresaRepository().List();
        }

        public async Task<object> ListarEventosEmpresaAsync(int idEmpresa)
        {
            return new EventoRepository().ListEventosEmpresa(idEmpresa);
        }

        public async Task<object> AdicionarPacoteEventosAsync(object input)
        {
            try
            {
                var quantidadeEventosNovos = 0;
                var arquivos = input.ToStringArray();
                foreach (var caminhoArquivo in arquivos)
                {
                    var arquivo = JsonConvert.DeserializeObject<ArquivoEntradaDTO>(File.ReadAllText(caminhoArquivo));
                    if (arquivo != null)
                    {
                        this.SalvarEmpresa(arquivo.Empresa);
                        quantidadeEventosNovos += this.SalvarEventos(arquivo.Eventos, arquivo.Empresa.Id);
                    }
                }
                return new { status = "OK", quantidadeEventosNovos };
            }
            catch (Exception ex)
            {
                File.WriteAllText(@"C:\\users\lndr2\desktop\log.txt", ex.Message + Environment.NewLine + ex.StackTrace);
                throw;
            }
        }

        private void SalvarEmpresa(Empresa empresa)
        {
            var empresaRepository = new EmpresaRepository();
            var empresaBase = empresaRepository.Get(empresa.Id);
            if (empresaBase == null)
            {
                empresaRepository.Add(empresa);
            }
        }

        private int SalvarEventos(List<Evento> eventos, int idEmpresa)
        {
            var novosEventos = 0;
            var eventoRepository = new EventoRepository();
            var data = DateTime.Now;

            foreach (var evento in eventos)
            {
                var eventoBase = eventoRepository.GetByIdEvento(evento.IdEvento);
                if (eventoBase == null)
                {
                    eventoRepository.Add(new Evento
                    {
                        IdEmpresa              = idEmpresa,
                        DataUpload             = data,
                        IdEvento               = evento.IdEvento,
                        StatusEvento           = (int)StatusEventoEnum.Novo,
                        TipoEvento             = evento.TipoEvento,
                        DataAtualizacao        = data,
                        EventoBase64Encriptado = evento.EventoBase64Encriptado
                    });
                    novosEventos++;
                }
            }
            return novosEventos;
        }

        public async Task<object> ComecarEnvioEventos(dynamic input)
        {
            return null;
        }
    }
}
