using Lndr.Simple.CLR.Models.Entities;
using Lndr.Simple.CLR.Repositories;
using System;
using System.IO;

namespace Lndr.Simple.CLR.Services.Implementations
{
    class ErrorLogger
    {
        public static void Error(Exception ex)
        {
            SaveError(ex.StackTrace, ex.Message, "error");
        }

        public static void Error(string msg)
        {
            SaveError(null, msg, "error");
        }

        public static void Log(string msg)
        {
            SaveError(null, msg, "log");
        }

        public static void Warn(Exception ex)
        {
            SaveError(ex.StackTrace, ex.Message, "warn");
        }

        public static void Warn(string msg)
        {
            SaveError(null, msg, "warn");
        }

        private static void SaveError(string stacktrace, string msg, string tipo)
        {
            try
            {
                SaveErrorToFile(stacktrace, msg, tipo);

                new BaseRepository<Error>().Add(new Error
                {
                    Data = DateTime.Now,
                    Mensagem = msg,
                    Stacktrace = stacktrace,
                    Tipo = tipo
                });
            }
            catch (Exception)
            {
            }            
        }

        private static void SaveErrorToFile(string stacktrace, string msg, string tipo)
        {
            var pathMeuDocumentos = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(pathMeuDocumentos, "Lndr", "log.txt");
            var diretorio = Path.GetDirectoryName(path);
            if (Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }
            var contents = string.Format("{0} ~ {1} - {2} - {3}", DateTime.Now.ToString("(dd/MM/yyyy hh:mm:ss)"), tipo, msg, stacktrace);
            File.AppendAllText(path, contents);
        }
    }
}
