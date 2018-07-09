using Lndr.Simple.CLR.Helpers;
using Lndr.Simple.CLR.Helpers.Exceptions;
using Lndr.Simple.CLR.Helpers.Extensions;
using Lndr.Simple.CLR.Models;
using Lndr.Simple.CLR.Models.Dto;
using Lndr.Simple.CLR.Models.Entities;
using Lndr.Simple.CLR.Models.Enums;
using Lndr.Simple.CLR.Repositories;
using Lndr.Simple.CLR.Services;
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
            return new EmpresaRepository().GetAll();
        }

        public async Task<object> ListarEventosEmpresaAsync(dynamic input)
        {
            var repository = new EventoRepository();

            var totalEventos = repository.QuantidadeEventosEmpresa((int)input.idEmpresa);
            var totalPaginas = Math.Ceiling(totalEventos / (double)input.pagina);
            var pagina = (int)((int)input.pagina > totalPaginas ? totalPaginas : (int)input.pagina);
            pagina = pagina < 0 ? 0 : pagina;
            return new
            {
                eventos = repository.ListEventosEmpresa((int)input.idEmpresa, (int)input.tamanhoPagina, pagina),
                totalPaginas,
                pagina
            };
        }

        public async Task<object> AdicionarPacoteEventosAsync(object input)
        {
            try
            {
                var service = new EventoService();

                var quantidadeEventosNovos = 0;
                var arquivos = input.ToStringArray();
                foreach (var caminhoArquivo in arquivos)
                {
                    var arquivo = JsonConvert.DeserializeObject<ArquivoEntradaDTO>(File.ReadAllText(caminhoArquivo));
                    if (arquivo != null)
                    {
                        quantidadeEventosNovos += service.AdicionarPacoteEventos(arquivo);
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

        public async Task<object> AdicionarLotes(object input)
        {
            var service = new EventoService();
            var data = DateTime.Now;

            var arquivos = input.ToStringArray();
            foreach (var caminhoArquivo in arquivos)
            {
                var arquivoEncriptado = File.ReadAllBytes(caminhoArquivo);
                var arquivosEncriptadosBytes = new List<byte[]>();

                if (arquivoEncriptado.IsZipFile())
                {
                    arquivosEncriptadosBytes.AddRange(arquivoEncriptado.UnzipAll());
                }
                else
                {
                    arquivosEncriptadosBytes.Add(arquivoEncriptado);
                }

                foreach (var arquivoEncriptadoBytes in arquivosEncriptadosBytes)
                {
                    if (!CryptographyHelper.IsHeaderValid(arquivoEncriptadoBytes, Resources.HeaderEFinanceira))
                    {
                        throw new ArquivoInvalidoException();
                    }

                    var arquivoDecriptado = CryptographyHelper.Decrypt(arquivoEncriptadoBytes, Resources.CryptoPrivateKey);
                    var dadosArquivo = JsonConvert.DeserializeObject<ArquivoComunicacaoEFinanceiraDto>(arquivoDecriptado.GetString());

                    var empresa = new Empresa
                    {
                        Entidade = dadosArquivo.Entidade,
                        CNPJ = dadosArquivo.EntidadeCnpj,
                        Nome = dadosArquivo.EntidadeNome
                    };

                    var lote = new EFinanceiraLote
                    {
                        DataAtualizacao = data,
                        DataUpload = data,
                        IdLote = dadosArquivo.LoteId,
                        StatusEvento = (int)StatusEventoEnum.Novo,
                        TipoEvento = dadosArquivo.TipoEventoId,
                        XmlLote = dadosArquivo.LoteXmlBase64.FromBase64String()
                    };

                    service.AdicionarLote(empresa, lote);
                }
            }

            return new { };
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
