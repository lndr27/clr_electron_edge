const fs = require('fs');
const {
    remote,
    ipcRenderer
} = require('electron');
const {
    dialog,
    getCurrentWindow,
    Menu
} = remote;

var app = angular.module("App", []);

const browserWindow = getCurrentWindow();

app.controller("MainController", function ($scope, AppService) {

    $scope.processando = false;

    $scope.isMaximizado = false;

    $scope.empresas = [];

    $scope.empresaSelecionada = null;

    $scope.lotes = [];

    $scope.eventos = [];

    $scope.paginacaoEventos = {
        pagina: 1,
        tamanhoPagina: 50,
        totalPaginas: 0
    };

    /**
     * Texto exibido no rodape da janela
     */
    $scope.statusAplicacao = '';

    /**
     * Funcao acionada ao carregar a janela principal
     */
    $scope.init = function () {
        var loadingAlert = alertify.loadingDialog('Carregando');

        AppService.listarEmpresas()
            .then(empresas => $scope.empresas = empresas)
            .catch(err => console.error(err))
            .finally(() => {
                loadingAlert.close();
                $scope.$apply();
            });

        $('body').on('keydown', evt => {
            if (evt.keyCode === 123)
                browserWindow.webContents.toggleDevTools();
        });

        var el = new SimpleBar(document.getElementById('scrollable2'));
        $(el.getScrollElement()).on("scroll",function() { $(this).find("thead").css("transform", `translate(0, ${this.scrollTop}px)`); });
    };

    /**
     * Downloa dos lotes de retorno da empresa selecionada
     */
    $scope.downloadLoteRetornoEmpresa = () => {
        AppService.downloadLoteRetornosEmpresa($scope.empresaSelecionada.Id)
            .then(response => {
                dialog.showSaveDialog({
                    buttonLabel: 'Salvar',
                    filters: [{
                        name: 'lote',
                        extensions: ['.pkg']
                    }]
                }, filename => {
                    fs.writeFile(filename, new Buffer(response), (err) => {
                        if (err) console.log(err);
                    });
                });
            })
            .catch(err => {
                console.log(err);
            });
    };

    /**
     * Envia pacotes para serem adicionados ao banco de dados local
     */
    $scope.adicionarPacoteEventos = () => {
        if ($scope.processando) return;

        $scope.processando = true;

        dialog.showOpenDialog({
            buttonLabel: 'Abrir Lote',
            filters: [{
                name: 'Pacote',
                extensions: ['reinf']
            }],
            properties: ['multiSelections']
        }, filenames => {
            if (!filenames) return;

            let loadingAlert = alertify.loadingDialog('Aguarde, processando lotes.');

            AppService.adicionarPacoteEventos(filenames)
                .then(response => {
                    loadingAlert.close();
                })
                .catch(err => console.log(err))
                .finnaly(() => {
                    $scope.processando = false;
                    loadingAlert.close();
                });
        });
    };

    /**
     * Fechar aplicacao
     */
    $scope.sair = () => ipcRenderer.send('sair');

    /**
     * Minimizar Janela
     */
    $scope.minimizar = () => browserWindow.minimize();

    /**
     * Maximiza ou Restaura janela
     */
    $scope.maximizar = () => {
        browserWindow.isMaximized() ? browserWindow.restore() : browserWindow.maximize();
        $scope.isMaximizado = !$scope.isMaximizado;
    };
    
    $scope.onChangeEmpresa = () => {
        $scope.eventos = [];

        AppService.listarEventosEmpresa(
            $scope.empresaSelecionada.Id,
            $scope.paginacaoEventos.pagina,
            $scope.paginacaoEventos.tamanhoPagina
        )
        .then(response => {
            $scope.eventos = response.eventos;
            $scope.$apply();
        })
        .catch(err => console.error(err))
        .finally(_ => {

        });
    };

});