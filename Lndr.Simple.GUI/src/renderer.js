// This file is required by the index.html file and will
// be executed in the renderer process for that window.
// All of the Node.js APIs are available in this process.
var edge = require('electron-edge-js');
var dialog = require('electron').remote.dialog;

var assemblyFile = './resources/Lndr.Simple.CLR.dll';
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



document.getElementById('foo').addEventListener('click', () => {

    dialog.showOpenDialog({}, (filepaths) => {
        adicionarPacotes(filepaths, (error, result) => {
            console.log(error);
            console.log(result);
        });
    });
});

document.getElementById('bar').addEventListener('click', () => {

    listarEventosEmpresa(1, (error, result) => {
        console.log(error);
        console.log(result);
    });
});

document.getElementById('baz').addEventListener('click', () => {

    listarEmpresas({}, (error, result) => {
        console.log(error);
        console.log(result);
    });
});