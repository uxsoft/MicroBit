open System
open MicroBit
open Argu

// microbit run
// microbit watch
// -p --port COM3

// microbit intelhex ???

 
type CliArguments =
    | [<CliPrefix(CliPrefix.None); CliPosition(CliPosition.First)>] Run
    | [<CliPrefix(CliPrefix.None); CliPosition(CliPosition.First)>] Watch
    | [<AltCommandLine("-p")>] Port of string
    | [<MainCommand; Last>] File of string
    interface IArgParserTemplate with
        member s.Usage =
            match s with
            | Run -> "executes contents of a file on the micro:bit."
            | Watch -> "re-executes the contents of a file on the micro:bit every time the file changes."
            | Port _ -> "specify a serial port over which to connect to the micro:bit."
            | File _ -> "which file to run/watch"
    
[<EntryPoint>]
let main argv =

    let parser = ArgumentParser.Create<CliArguments>(programName = "microbit")    
    let arguments = parser.Parse argv
    Console.ReadKey()
    0 // return an integer exit code