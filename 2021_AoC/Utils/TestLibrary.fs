namespace Utils

module Printer =
    // Using F# formatting
    let printList input = 
        input |> Seq.iter (fun x -> printf "%d " x)

    // Using ToString()
    let printList2 input = 
        sprintf "%O" input


// Temp test
module Say =
    let hello name =
        printfn "Hello %s" name