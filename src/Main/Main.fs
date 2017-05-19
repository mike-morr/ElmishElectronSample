module SpOpsCenter

open Fable.Core
open Fable.Core.JsInterop
open Fable.Import
open Fable.Import.Electron

let mutable mainWindow: BrowserWindow option = Option.None
let createWindow () =
    let options = createEmpty<BrowserWindowOptions>
    options.width <- Some 800.
    options.height <- Some 600.

    let window = electron.BrowserWindow.Create(options)

    window.loadURL("file://" + Node.Globals.__dirname + "/../public/index.html");

    // #if DEBUG
    // Node.Fs.fs.watchFile(Node.Globals.__dirname + "/renderer.js", fun _ ->
    //     // window.webContents.reloadIgnoringCache()
    // )
    // #endif

    // Emitted when the window is closed.
    window.on("closed", !!(fun () ->
        // Dereference the window object, usually you would store windows
        // in an array if your app supports multi windows, this is the time
        // when you should delete the corresponding element.
        mainWindow <- Option.None
    )) |> ignore

    mainWindow <- Some window

    
// This method will be called when Electron has finished
// initialization and is ready to create browser windows.
electron.app.on("ready", !!createWindow) |> ignore

// Quit when all windows are closed.
electron.app.on("window-all-closed", !!(fun () ->
    // On OS X it is common for applications and their menu bar
    // to stay active until the user quits explicitly with Cmd + Q
    if Node.Globals.``process``.platform <> Node.Base.NodeJS.Darwin then
        electron.app.quit()
)) |> ignore

electron.app.on("activate", !!(fun () ->
    // On OS X it's common to re-create a window in the app when the
    // dock icon is clicked and there are no other windows open.
    if mainWindow.IsNone then
        createWindow()
)) |> ignore