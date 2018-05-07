using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text.RegularExpressions;
using System.Xml;

namespace Lndr.Simple.CLR.Helpers
{
    public class AssinadorXml
    {
        private static readonly string signatureMethod = @"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
        private static readonly string digestMethod = @"http://www.w3.org/2001/04/xmlenc#sha256";

        public static string Assinar()
        {
            var xml = new XmlDocument();
            xml.LoadXml(@"<eFinanceira xmlns=""http://www.eFinanceira.gov.br/schemas/evtMovOpFin/v1_0_1"">
<evtMovOpFin id=""ID00000000001"">
<ideEvento>
<indRetificacao>1</indRetificacao>
<tpAmb>1</tpAmb>
<aplicEmi>2</aplicEmi>
<verAplic>00000000000000000001</verAplic>
</ideEvento>
<ideDeclarante>
<cnpjDeclarante>01234567891234</cnpjDeclarante>
</ideDeclarante>
<ideDeclarado>
<tpNI>2</tpNI>
<tpDeclarado>FATCA101</tpDeclarado>
<NIDeclarado>01234567891234</NIDeclarado>
<NIF>
<NumeroNIF>1234567890123456789012345</NumeroNIF>
<PaisEmissaoNIF>US</PaisEmissaoNIF>
</NIF>
<NomeDeclarado>
Nome do Declarado com 100 caracteres com 100 caracteres com 100 caracteres com 100 caracteres com 10
</NomeDeclarado>
<EnderecoLivre>
EnderecoLivre com 200 caracteres com 200 caracteres com 200 caracteres com 200 caracteres com 200 caracteres com 200 caracteres com 200 caracteres com 200 caracteres com 200 caracteres lalalal
</EnderecoLivre>
<PaisEndereco>
<Pais>BR</Pais>
</PaisEndereco>
<Proprietarios>
<tpNI>1</tpNI>
<NIProprietario>01234567891</NIProprietario>
<NIF>
<NumeroNIF>1234567890123456789012345</NumeroNIF>
<PaisEmissaoNIF>US</PaisEmissaoNIF>
</NIF>
<Nome>
Nome com 100 caracteres com 100 caracteres com 100 caracteres com 100 caracteres com 100 caracteres
</Nome>
<EnderecoLivre>
EnderecoLivre com 200 caracteres com 200 caracteres com 200 caracteres com 200 caracteres com 200 caracteres com 200 caracteres com 200 caracteres com 200 caracteres com 200 caracteres lalalal
</EnderecoLivre>
<PaisEndereco>
<Pais>BR</Pais>
</PaisEndereco>
<PaisResid>
<Pais>BR</Pais>
</PaisResid>
<PaisNacionalidade>
<Pais>BR</Pais>
</PaisNacionalidade>
<Reportavel>
<Pais>US</Pais>
</Reportavel>
</Proprietarios>
</ideDeclarado>
<mesCaixa>
<anoMesCaixa>201401</anoMesCaixa>
<movOpFin>
<Conta>
<infoConta>
<Reportavel>
<Pais>US</Pais>
</Reportavel>
<tpConta>1</tpConta>
<subTpConta>101</subTpConta>
<tpNumConta>OECD605</tpNumConta>
<numConta>1234|123|1234567890123</numConta>
<tpRelacaoDeclarado>3</tpRelacaoDeclarado>
<NoTitulares>4</NoTitulares>
<BalancoConta>
<totCreditos>150000,00</totCreditos>
<totDebitos>100000,00</totDebitos>
<totCreditosMesmaTitularidade>100000,00</totCreditosMesmaTitularidade>
<totDebitosMesmaTitularidade>100000,00</totDebitosMesmaTitularidade>
</BalancoConta>
<PgtosAcum>
<tpPgto>FATCA503</tpPgto>
<totPgtosAcum>0,00</totPgtosAcum>
</PgtosAcum>
</infoConta>
</Conta>
<Cambio>
<totCompras>1000000,00</totCompras>
<totVendas>1000000,00</totVendas>
<totTransferencias>1000000,00</totTransferencias>
</Cambio>
</movOpFin>
</mesCaixa>
</evtMovOpFin>
</eFinanceira>");

            var certificado = SelecionarCertificadoAssinaturaArquivo();
            var tag = obtemElementoAssinar(xml);

            assinarXML(xml, certificado, tag, "id");
            return null;
        }


        private static string obtemElementoAssinar(XmlDocument arquivo)
        {
            var match = Regex.Match(arquivo.OuterXml, @"<(?<tagAssinar>evt.+) id=""ID.+?>", RegexOptions.IgnoreCase);
            if (match.Success)
            {
                return match.Groups["tagAssinar"].Value;
            }
            return null;
        }

        private static XmlDocument assinarXML(XmlDocument documentoXML, X509Certificate2 certificadoX509, string tagAAssinar, string idAtributoTag)
        {
            Guard.ArgumentNullOrEmpty(tagAAssinar, "tagAAssinar");
            Guard.ArgumentNullOrEmpty(idAtributoTag, "idAtributoTag");
            
            bool temChavePrivada = false;
            try
            {
                temChavePrivada = certificadoX509.HasPrivateKey;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            if (!temChavePrivada)
            {
            }

            try
            {
                // Informando qual a tag será assinada
                var nodeParaAssinatura = documentoXML.GetElementsByTagName(tagAAssinar);
                var signedXml = new SignedXml((XmlElement)nodeParaAssinatura[0]);
                signedXml.SignedInfo.SignatureMethod = signatureMethod;

                var privateKey = (RSACryptoServiceProvider)certificadoX509.PrivateKey;

                if (!privateKey.CspKeyContainerInfo.HardwareDevice)
                {
                    var enhCsp = new RSACryptoServiceProvider().CspKeyContainerInfo;
                    var cspparams = new CspParameters(enhCsp.ProviderType, enhCsp.ProviderName, privateKey.CspKeyContainerInfo.KeyContainerName);
                    if (privateKey.CspKeyContainerInfo.MachineKeyStore)
                    {
                        cspparams.Flags |= CspProviderFlags.UseMachineKeyStore;
                    }
                    privateKey = new RSACryptoServiceProvider(cspparams);
                }

                // Adicionando a chave privada para assinar o documento
                signedXml.SigningKey = privateKey;

                // Referenciando o identificador da tag que será assinada
                var reference = new Reference("#" + nodeParaAssinatura[0].Attributes[idAtributoTag].Value);
                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(false));
                reference.AddTransform(new XmlDsigC14NTransform(false));
                reference.DigestMethod = digestMethod;

                // Adicionando a referencia de qual tag será assinada
                signedXml.AddReference(reference);

                // Adicionando informações do certificado na assinatura
                var keyInfo = new KeyInfo();
                keyInfo.AddClause(new KeyInfoX509Data(certificadoX509));
                signedXml.KeyInfo = keyInfo;

                // Calculando a assinatura
                signedXml.ComputeSignature();

                // Adicionando a tag de assinatura ao documento xml
                var sig = signedXml.GetXml();
                documentoXML.GetElementsByTagName(tagAAssinar)[0].ParentNode.AppendChild(sig);

                var xmlAssinado = new XmlDocument();
                xmlAssinado.PreserveWhitespace = true;
                xmlAssinado.LoadXml(documentoXML.OuterXml);
                return xmlAssinado;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static X509Certificate2 SelecionarCertificadoAssinaturaArquivo()
        {
            var oX509Cert = new X509Certificate2();
            var store = new X509Store("MY", StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
            var collection =  store.Certificates;
            var collection1 = collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            var collection2 = collection1.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);
            var scollection = X509Certificate2UI.SelectFromCollection(collection2, "Certificado(s) Digital(is) disponível(is)", "Selecione o certificado digital para uso no aplicativo", X509SelectionFlag.SingleSelection);

            if (scollection.Count == 0)
            {
                return null;
            }
            else
            {
                oX509Cert = scollection[0];
                return oX509Cert;
            }
        }
    }
}
