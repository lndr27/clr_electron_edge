(function(angular, undefined) {

    var edge = require('electron-edge-js');
    var dialog = require('electron').remote.dialog;

    angular.module("App").service("AppService", function() {

        let caminhoArquivoAssembly = './src/resources/Lndr.Simple.CLR.dll';

        this.listarEmpreas = edge.func({
            assemblyFile: assemblyFile,
            typeName: typeName,
            methodName: 'ListarEmpresasAsync'
        });

    });

})(angular);