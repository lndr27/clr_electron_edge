const { app, BrowserWindow } = require('electron')
const path = require('path')
const url = require('url')

let mainWindow


app.on('ready', () => {
    // Create the browser window.
    mainWindow = new BrowserWindow({ width: 800, height: 600 })

    // and load the index.html of the app.
    mainWindow.loadURL(url.format({
        pathname: path.join(__dirname, 'index.html'),
        protocol: 'file:',
        slashes: true
    }));

    mainWindow.webContents.openDevTools();

    mainWindow.on('closed', () => mainWindow = null);


/*
    mainWindow.setMenu(Menu.buildFromTemplate([{
        label: "Arquivo",
        submenu: [
            { label: "Always on top", click: this._toggleAlwaysOnTop.bind(this) },
            { type: "separator" },
            { label: "DevTools", click: this._toggleDevTools.bind(this) },
            { label: "Exit", click: this._closeApp.bind(this) }
        ]
    }]));
*/

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
