(function () {

    const fs = require('fs');
    const dialog = require('electron').remote.dialog;

    var app = angular.module("App", []);

    app.controller("MainController", function ($scope, AppService) {

        $scope.eventos = [];

        $scope.init = function () {
           
        };

        $scope.foo = function() {

            AppService.downloadLoteRetornosEmpresa(1)
            .then((response) => {
                debugger;
                dialog.showSaveDialog({
                    buttonLabel: 'Salvar',
                    filters: [{ name: 'lote', extensions: ['.pkg']}]
                }, (filename) => {
                    debugger;
                    fs.writeFile(filename, new Buffer(response), (err) => {
                        if (err) console.log(err);
                    });

                });
            })
            .catch((err) => {
                console.log(err);
            });
        };

    });

})();