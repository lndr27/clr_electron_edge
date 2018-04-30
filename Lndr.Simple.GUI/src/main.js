const { app, BrowserWindow, ipcMain, Menu } = require('electron')
const path = require('path')
const url = require('url')

let mainWindow;
let menuPrincipal;

app.on('ready', () => {

    mainWindow = new BrowserWindow({ 
        width: 950, 
        height: 600,
        minWidth: 950,
        minHeight: 600,
        frame: false
    })

    mainWindow.loadURL(url.format({
        pathname: path.join(__dirname, 'views/index.html'),
        protocol: 'file:',
        slashes: true
    }));

    mainWindow.webContents.openDevTools();

    mainWindow.on('closed', () => mainWindow = null);

    menuPrincipal = Menu.buildFromTemplate([{
        label: "Arquivo",
        submenu: [            
            { label: "DevTools", click: mainWindow.webContents.openDevTools },
            { label: 'Adicionar Pacote', click: () => {} },
            { type: "separator" },
            { label: "Sair", click: () => app.exit }
        ]
    }]);

    mainWindow.setMenu(menuPrincipal);

    registrarRendererListeners();

});

app.on('window-all-closed', () => {
    if (process.platform !== 'darwin') {
        app.quit()
    }
});

app.on('activate', () => {
    if (mainWindow === null) {
        createWindow()
    }
});

const registrarRendererListeners = () => {
    ipcMain.on('toggle-item-menu', (evt, nomeMenu, habilitar) => toggleItemMenu(nomeMenu, habilitar, menuPrincipal));
    ipcMain.on('sair', app.quit);
};


const toggleItemMenu = (nome, habilitar, itens) => {
    if (!itens || !Array.isArray(itens) || !itens.length) return;
    for (let i = 0; i < itens.length; ++i)
    {
        if (itens[i].label.indexOf(nome) !== -1)
        {
            itens[i].enabled = habilitar;
            return true;
        }
        if (itens[i].submenu && itens[i].submenu.length)
        {
            if (toggleItemMenu(nome, habilitar, itens[i].submenu))
            {
                return true;
            }
        }
    }
};