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
            var privateKey = @"<RSAKeyValue><Modulus>3tR+wUpHkv/dsrg2MHCVo3xlAJlF/K12VoWD3iSEyjWNwvu+tYnkG6gKoVVssVv+oLlPpdDqtTE9B36eXgf9X+INUaXW5IR9hJL9s/vjbOj5KZlIOHFrQDubIjo5djj1Z3nmRdQIkGwuyF6C7Sz+Svipu4Ermc5Rdfj02YEOcPk=</Modulus><Exponent>AQAB</Exponent><P>/0jLDdULItLk3eYaO3fEia00riKNynJuvPxbcgeZahvHR102nbR6lveLVhKYmtKkm0W2AAf7Bn7qGd4Uw4/6Rw==</P><Q>33RpL4IlhnCJgranFAHLnwKApRSiLqOOVqbxT9o0AnI8ZWhBtF1S3W9aNbSaXbhh1VrvYeaXMA/UpfRaWUyavw==</Q><DP>n5rVSOqfscAE0y8fy3pP6Hvf6dc0qxPsZD+qtJpHsm13pAiFMwg5dNWDyeaKfpGf1Gw7I73ZlfAXUODmPnZv1Q==</DP><DQ>II/IHCIs8bBIeYHASwwP7fXhZSzijUVMIphlJau4VHrtDiHpIS1QO/BUButwayPJLdv4ch21/kzwQdafq8+b5Q==</DQ><InverseQ>XQaUBilkhuMaRaoZifJTjvVBn8ajQBs2GrZoXShlGe/QW3hwRVz5Ej8wiwjfL2ZbEEheYWGz7D6ayEZm7SepFw==</InverseQ><D>QxK1gNsqBj6MbjJy5UvOTyKviGmann9cQpj345McgX1zSYDbm+uAxS5k3MFI8W4ejCSbq+7h516F306mvukqw/S9VczRu8K+jlXlxsZNK0yaD6zTjbgi+g7607/gqwe0C1+AgzuemntZKJaEOX12QOetzIHkrfdpfhttl5bHR00=</D></RSAKeyValue>";
            var header = @"LNDR.TESTE";
            var encryptedData = CryptographyHelper.Encrypt("SOU LINDO".GetBytes(), pubKey, header);

            var descryptedData = CryptographyHelper.Decrypt(encryptedData, privateKey);

            //classe.AdicionarPacoteEventosAsync(new string[] { @"C:\\users\lndr2\desktop\teste.reinf" }).Wait();

            //AssinadorXml.Assinar();

        }        
    }
}
