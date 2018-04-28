(function () {

    var edge = require('electron-edge-js');
    var dialog = require('electron').remote.dialog;

    var assemblyFile = './src/resources/Lndr.Simple.CLR.dll';
    var typeName = 'Lndr.Simple.CLR.Controllers.EventosController';

    var adicionarPacotes = edge.func({
        assemblyFile: assemblyFile,
        typeName: typeName,
        methodName: 'AdicionarPacoteEventosAsync'
    });

    var listarEventosEmpresa = edge.func({
        assemblyFile: assemblyFile,
        typeName: typeName,
        methodName: 'ListarEventosEmpresaAsync'
    });

    var listarEmpresas = edge.func({
        assemblyFile: assemblyFile,
        typeName: typeName,
        methodName: 'ListarEmpresasAsync'
    });


    var app = angular.module("App", []);

    app.controller("MainController", ['$scope', function ($scope) {

        $scope.eventos = [];

        $scope.init = function () {
            listarEmpresas(1, function (err, eventos) {
                if (err) {
                    return alert(err);
                }
                $scope.eventos = eventos;
            });

        }
    }]);

})();