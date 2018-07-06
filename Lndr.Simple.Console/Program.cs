using Lndr.Simple.CLR;
using Lndr.Simple.CLR.Controllers;
using Lndr.Simple.CLR.Helpers;
using Lndr.Simple.CLR.Helpers.Extensions;
using System;
using System.IO;
using System.Text;
using System.Web.Security;

namespace Lndr.Simple.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var pubKey = @"<RSAKeyValue><Modulus>3tR+wUpHkv/dsrg2MHCVo3xlAJlF/K12VoWD3iSEyjWNwvu+tYnkG6gKoVVssVv+oLlPpdDqtTE9B36eXgf9X+INUaXW5IR9hJL9s/vjbOj5KZlIOHFrQDubIjo5djj1Z3nmRdQIkGwuyF6C7Sz+Svipu4Ermc5Rdfj02YEOcPk=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>";
            var header = @"LNDR.TESTE";
            var encryptedData = CryptographyHelper.AES_Encrypt("NAO NOA NONFSKDFJ SDFKJ:SDKJF SD:LKFJ ".GetBytes(), pubKey, header.GetBytes());

            File.WriteAllBytes(@"C:\\users\lndr2\Desktop\test.lndr", encryptedData);

            //var classe = new EventosController();

            //classe.AdicionarPacoteEventosAsync(new string[] { @"C:\\users\lndr2\desktop\teste.reinf" }).Wait();

            //AssinadorXml.Assinar();

        }        
    }
}
