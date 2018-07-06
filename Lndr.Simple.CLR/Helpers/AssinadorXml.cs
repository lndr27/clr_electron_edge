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

        public static string Assinar(string xmlString)
        {
            var xml = new XmlDocument();
            xml.LoadXml(xmlString);
            var xmlAssinado = assinarXML(xml, SelecionarCertificadoAssinaturaArquivo(), obtemElementoAssinar(xml), "id");
            return xmlAssinado != null ? xmlAssinado.OuterXml : null;
        }

        private static string obtemElementoAssinar(XmlDocument arquivo)
        {
            var match = Regex.Match(arquivo.OuterXml, @"<(?<tagAssinar>evt.+) id=""ID.+?>", RegexOptions.IgnoreCase);
            return match.Success ? match.Groups["tagAssinar"].Value : null;
        }

        private static XmlDocument assinarXML(XmlDocument documentoXML, X509Certificate2 certificadoX509, string tagAAssinar, string idAtributoTag)
        {
            Guard.ForArgumentNullOrEmpty(tagAAssinar, "tagAAssinar");
            Guard.ForArgumentNullOrEmpty(idAtributoTag, "idAtributoTag");
            
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
