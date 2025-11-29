namespace SimpleStoreSimulator.GUI

open System
open Avalonia

module Program =

    [<CompiledName "BuildAvaloniaApp">] 
    let buildAvaloniaApp () = 
        AppBuilder
            .Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace(areas = Array.empty)

    [<EntryPoint; STAThread>]
    let main argv =
        try
            buildAvaloniaApp().StartWithClassicDesktopLifetime(argv)
        with ex ->
            printfn "CRITICAL ERROR: %O" ex
            1
