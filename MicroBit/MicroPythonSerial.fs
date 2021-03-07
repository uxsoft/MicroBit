module MicroBit.MicroPythonSerial

open System
open System.IO
open System.IO.Ports
open System.Threading

let CTRLC = '\x03'
let CTRLD = '\x04'
let CTRLE = '\x05'

let connect portName =
    let port = new SerialPort(PortName = portName, BaudRate = 115200)
    port.Open()
    port

let softReset (port: SerialPort) = 
    port.WriteLine("")
    port.WriteLine($"{CTRLD}")
    port
    
let stopProgram (port: SerialPort) =
    port.WriteLine($"{CTRLC}")
    port
    
let paste text (port: SerialPort) =
    port.WriteLine($"{CTRLE}")
    port.Write text
    port.WriteLine($"{CTRLD}")
    port
    
let mirror (sink: string -> unit) (port: SerialPort) =
    port.DataReceived.Add(fun _ ->
        let input = port.ReadExisting()
        sink input)
    port
    
let close (port: SerialPort) =
    port.Close()
    
let demo() =
    let file = File.ReadAllText(@"C:\Users\uxsoft\Desktop\python\main.py")
    
    connect "COM3"
    |> mirror Console.Write
    |> stopProgram
    |> paste file 
    |> close
    
    Console.ReadKey()
    
let watchDemo() =
    let fileName = @"C:\Users\uxsoft\Desktop\python\main.py"
    let dir = Path.GetDirectoryName fileName
    
    let port =
        connect "COM3"
        |> mirror Console.Write
    
    FileWatcher.applyAndWatch dir (fun _ ->
        Thread.Sleep(50)
        let file = File.ReadAllText(fileName)
        
        port
        |> stopProgram
        |> paste file
        |> ignore)
    
    Console.ReadKey() |> ignore
    close port