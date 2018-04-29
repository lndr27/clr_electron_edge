(function(angular, undefined) {

    var edge = require('electron-edge-js');

    var dialog = require('electron').remote.dialog;

    angular.module("App").service("AppService", function() {

        let caminhoArquivoAssembly = `${__dirname}/../resources/Lndr.Simple.CLR.dll`;

        let namespaceControllers = 'Lndr.Simple.CLR.Controllers';

        /**
         * @returns {Promise}
         */
        this.listarEmpresas = () => {
            return new Promise((resolve, reject) => {
                _listarEmpreas(null, (err, response) => {
                    err ? reject(err) : resolve(response);
                });
            });
        };

        let _listarEmpreas = edge.func({
            assemblyFile: caminhoArquivoAssembly,
            typeName    : `${namespaceControllers}.EventosController`,
            methodName  : 'ListarEmpresasAsync'
        });
        
        /**
         * 
         * @param {Number} idEmpresa 
         * @param {Number} pagina 
         * @param {Number} tamanhoPagina
         * @returns {Promise}
         */
        this.listarEventosEmpresa = (idEmpresa, pagina, tamanhoPagina) => {
            return new Promise((resolve, reject) => {
                _listarEventosEmpresa({ idEmpresa, pagina, tamanhoPagina }, (err, response) => {
                    err ? reject(err) : resolve(response);
                });
            });
        };

        let _listarEventosEmpresa = edge.func({
            assemblyFile: caminhoArquivoAssembly,
            typeName    : `${namespaceControllers}.EventosController`,
            methodName  : 'ListarEventosEmpresaAsync'
        });

        
        /**
         * 
         * @param {*} caminhoArquivo 
         * @returns {Promise}
         */
        this.adicionarPacoteEventos = (caminhoArquivo) => {
            return new Promise((resolve, reject) => {
                _adicionarPacoteEventos(caminhoArquivo, (err, response) => {
                    err ? reject(err) : resolve(response);
                });
            });
        };

        let _adicionarPacoteEventos = edge.func({
            assemblyFile: caminhoArquivoAssembly,
            typeName    : `${namespaceControllers}.EventosController`,
            methodName  : 'AdicionarPacoteEventosAsync'
        });

        
        /**
         * 
         * @param {*} idEmpresa 
         * @returns {Promise}
         */
        this.comecarEnvioLotesEmpresa = (idEmpresa) => {
            return new Promise((resolve, reject) => {
                _adicionarPacoteEventos(idEmpresa, (err, response) => {
                    err ? reject(err) : resolve(response);
                });
            });
        };

        let _comecarEnvioLotesEmpresa = edge.func({
            assemblyFile: caminhoArquivoAssembly,
            typeName    : `${namespaceControllers}.EventosController`,
            methodName  : 'ComecarEnvioLotesEmpresaAsync'
        });

        
        /**
         * 
         * @param {*} idEmpresa 
         * @returns {Promise}
         */
        this.pararEnvioLotesEmpresa = (idEmpresa) => {
            return new Promise((resolve, reject) => {
                _pararEnvioLotesEmpresa(idEmpresa, (err, response) => {
                    err ? reject(err) : resolve(response);
                });
            });
        };

        let _pararEnvioLotesEmpresa = edge.func({
            assemblyFile: caminhoArquivoAssembly,
            typeName    : `${namespaceControllers}.EventosController`,
            methodName  : 'PararEnvioLotesEmpresaAsync'
        });

        
        /**
         * 
         * @param {Number} idEmpresa 
         * @returns {Promise}
         */
        this.downloadLoteRetornosEmpresa = (idEmpresa) => {
            return new Promise((resolve, reject) => {
                _downloadLoteRetornosEmpresa(idEmpresa, (err, response) => {
                    err ? reject(err) : resolve(response);
                });
            });
        };

        let _downloadLoteRetornosEmpresa = edge.func({
            assemblyFile: caminhoArquivoAssembly,
            typeName    : `${namespaceControllers}.EventosController`,
            methodName  : 'DownloadPacoteRetornoEmpresaAsync'
        });
    });

})(angular);