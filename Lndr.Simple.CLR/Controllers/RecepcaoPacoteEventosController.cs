using Lndr.Simple.CLR.Helpers.Extensions;
using Lndr.Simple.CLR.Models;
using Lndr.Simple.CLR.Models.Entities;
using Lndr.Simple.CLR.Models.Enums;
using Lndr.Simple.CLR.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
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
                        var idEmpresa = this.SalvarEmpresa(arquivo.Empresa);
                        quantidadeEventosNovos += this.SalvarEventos(arquivo.Eventos, idEmpresa);
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

        private int SalvarEmpresa(Empresa empresa)
        {
            var empresaRepository = new EmpresaRepository();
            var empresaBase = empresaRepository.GetByCnpj(empresa.CNPJ);
            if (empresaBase == null)
            {
                return empresaRepository.Add(empresa);
            }
            return empresaBase.Id;
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

        public async Task<object> ComecarEnvioLotesEmpresaAsync(int idEmpresa)
        {
            return null;
        }

        public async Task<object> PararEnvioLotesEmpresaAsync(int idEmpresa)
        {
            var certificado = SelecionarCertificadoAssinaturaArquivo();

            return certificado;
        }

        public async Task<object> DownloadPacoteRetornoEmpresaAsync(int idEmpresa)
        {            

            var arquivo = File.ReadAllBytes(@"C:\\users\lndr2\desktop\teste.json");
            return arquivo;
        }

        private static string SelecionarCertificadoAssinaturaArquivo()
        {
            X509Certificate2 vRetorna;
            var oX509Cert = new X509Certificate2();
            var store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            var collection = store.Certificates;
            var collection1 = collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            var collection2 = collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);
            var scollection = X509Certificate2UI.SelectFromCollection(collection2, "Certificado(s) Digital(is) disponível(is)", "Selecione o certificado digital para uso no aplicativo", X509SelectionFlag.SingleSelection);

            if (scollection.Count == 0)
            {
                vRetorna = null;
            }
            else
            {
                oX509Cert = scollection[0];
                vRetorna = oX509Cert;
            }

            return vRetorna.FriendlyName;
        }
    }
}
