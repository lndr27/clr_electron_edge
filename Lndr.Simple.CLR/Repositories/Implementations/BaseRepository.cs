using Dapper;
using System;
using System.Data;
using System.Data.SQLite;
using System.IO;

namespace Lndr.Simple.CLR.Repositories
{
    class BaseRepository
    {
        public SQLiteConnection GetDbConnection()
        {
            var arquivoBancoDados = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ObrigacoesLegais", "db.sqlite");

            if (!File.Exists(arquivoBancoDados))
            {
                this.CriarArquivoBancoDados(arquivoBancoDados);

                var success = true;
                
                using (var connection = new SQLiteConnection(string.Concat("Data Source=", arquivoBancoDados)))
                {
                    connection.Open();
                    try
                    {
                        this.CriarTabelas(connection);
                    }
                    catch (Exception)
                    {
                        success = false;
                    }                    
                }
                if (!success)
                {
                    File.Delete(arquivoBancoDados);
                    throw new Exception("Erro ao iniciar base de dados");
                }
            }

            return new SQLiteConnection(string.Concat("Data Source=", arquivoBancoDados));
        }

        private void CriarArquivoBancoDados(string path)
        {
            var diretorio = Path.GetDirectoryName(path);
            if (!Directory.Exists(diretorio))
            {
                Directory.CreateDirectory(diretorio);
            }
            SQLiteConnection.CreateFile(path);
        }

        private void CriarTabelas(IDbConnection connection)
        {

            connection.Execute(
@"CREATE TABLE Empresas (
     Id      INTEGER PRIMARY KEY AUTOINCREMENT
    ,Nome    VARCHAR NOT NULL
    ,CNPJ    VARCHAR NOT NULL
)");

            connection.Execute(
@"CREATE TABLE Eventos (
     Id                              INTEGER PRIMARY KEY AUTOINCREMENT
    ,IdEmpresa                       INTEGER NOT NULL
    ,IdEvento                        VARCHAR(36) NOT NULL
    ,EventoBase64Encriptado          VARCHAR
    ,TipoEvento                      INTEGER NOT NULL
    ,DataUpload                      DATETIME NOT NULL
    ,DataAtualizacao                 DATETIME NOT NULL
    ,StatusEvento                    INTEGER NOT NULL
    ,NumeroRecibo                    VARCHAR(50) NULL
)");

            connection.Execute(
@"CREATE TABLE EventoOcorrencias (
     Id              INTEGER PRIMARY KEY AUTOINCREMENT
    ,IdEvento        INTEGER NOT NULL
    ,Codigo          VARCHAR(100)
    ,Descricao       VARCHAR(8000)
    ,Localizacao     VARCHAR(8000)
    ,TipoOcorrencia  INTEGER    
)");

            connection.Execute(
@"CREATE TABLE LogErros (
    Id               INTEGER PRIMARY KEY AUTOINCREMENT
    ,Mensagem        VARCHAR
    ,Stacktrace      VARCHAR
    ,Data            DATETIME
)");
        }
    }
}
