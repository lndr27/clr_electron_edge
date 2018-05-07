using Lndr.Simple.CLR;
using Lndr.Simple.CLR.Controllers;
using Lndr.Simple.CLR.Helpers;
using System;

namespace Lndr.Simple.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            //var classe = new EventosController();

            //classe.AdicionarPacoteEventosAsync(new string[] { @"C:\\users\lndr2\desktop\teste.reinf" }).Wait();

            AssinadorXml.Assinar();

        }        
    }
}
