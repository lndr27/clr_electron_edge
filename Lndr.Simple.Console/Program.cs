using Lndr.Simple.CLR;

namespace Lndr.Simple.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var classe = new Class1();
            classe.NOtAdicionarPacoteEventos(new object[] { @"C:\\users\lndr2\desktop\teste.json" });
        }
    }
}
