using Dapper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using Dapper;
using Dapper.Contrib.Extensions;
using System.Linq;

namespace Lndr.Simple.CLR.Repositories
{
    class BaseRepository<TEntity> : IRepository<TEntity> where TEntity: class
    {
        public virtual int Add(TEntity entity)
        {
            using (var connection = this.GetDbConnection())
            {
                return (int)connection.Insert(entity);
            }
        }

        public virtual TEntity Get(int id)
        {
            using (var connection = this.GetDbConnection())
            {
                return connection.Get<TEntity>(id);
            }
        }

        public virtual List<TEntity> GetAll()
        {
            using (var connection = this.GetDbConnection())
            {
                return connection.GetAll<TEntity>().ToList();
            }
        }

        public virtual void Update(TEntity entity)
        {
            using (var connection = this.GetDbConnection())
            {
                SqlMapperExtensions.Update(connection, entity);
            }
        }

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
            #region Empresas
            connection.Execute(
@"CREATE TABLE Empresas (
     Id      INTEGER PRIMARY KEY AUTOINCREMENT
    ,Nome    VARCHAR NOT NULL
    ,CNPJ    VARCHAR NOT NULL
)");
            #endregion

            #region Eventos
            connection.Execute(
@"CREATE TABLE Eventos (
     Id                              INTEGER PRIMARY KEY AUTOINCREMENT
    ,IdEmpresa                       INTEGER NOT NULL
    ,IdEvento                        VARCHAR(36) NOT NULL
    ,XmlEvento                       VARCHAR
    ,TipoEvento                      INTEGER NOT NULL
    ,DataUpload                      DATETIME NOT NULL
    ,DataAtualizacao                 DATETIME NOT NULL
    ,StatusEvento                    INTEGER NOT NULL
    ,NumeroRecibo                    VARCHAR(50) NULL
)");
            #endregion

            #region EventoOcorrencias
            connection.Execute(
@"CREATE TABLE EventoOcorrencias (
     Id              INTEGER PRIMARY KEY AUTOINCREMENT
    ,IdEvento        INTEGER NOT NULL
    ,Codigo          VARCHAR(100)
    ,Descricao       VARCHAR(8000)
    ,Localizacao     VARCHAR(8000)
    ,TipoOcorrencia  INTEGER    
)");
            #endregion

            #region LogErros
            connection.Execute(
@"CREATE TABLE LogErros (
     Id              INTEGER PRIMARY KEY AUTOINCREMENT
    ,Tipo            VARCHAR
    ,Mensagem        VARCHAR
    ,Stacktrace      VARCHAR
    ,Data            DATETIME
)");
            #endregion

            #region Job
            connection.Execute(
@"CREATE TABLE Jobs (
     Id              INTEGER PRIMARY KEY AUTOINCREMENT
    ,IdEmpresa       INTEGER
    ,Tipo            INTEGER
    ,StatusJob       INTEGER
    ,DataInicio      DATETIME
    ,DataFim         DATETIME
    ,DataAtualizacao DATETIME
)");
            #endregion
        }
    }
}
