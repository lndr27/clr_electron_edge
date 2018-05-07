using System.ComponentModel;

namespace Lndr.Simple.CLR.Models.Enums
{
    enum TipoEventoEnum
    {
        [Description("Informações do Contribuinte")]
        InformacaoContribuinte  = 1000,

        [Description("Pertadores de Serviço")]
        PrestadorServicos       = 2010,

        [Description("Medidas Judiciais")]
        MedidaJudicial          = 1070,

        [Description("Tabela")]
        Tabela                  = 2070,

        [Description("Reabertura de Eventos")]
        Reabertura              = 2098,

        [Description("Fechamento")]
        Fechamento              = 2099,

        [Description("Exclusão de Evento")]
        Exclusao                = 9000
    }
}
