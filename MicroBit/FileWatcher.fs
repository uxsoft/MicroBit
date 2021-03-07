module MicroBit.FileWatcher

open System.IO

let applyAndWatch dir (apply: unit -> unit) =
    apply()
    let fsw = new FileSystemWatcher(dir)
    fsw.Filter <- "*.py"
    fsw.EnableRaisingEvents <- true
    fsw.Changed.Add (ignore >> apply)