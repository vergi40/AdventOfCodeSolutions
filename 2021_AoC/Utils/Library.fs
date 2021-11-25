namespace Utils

module Printer =
    // Using F# formatting
    let printList input = 
        input |> Seq.iter (fun x -> printf "%d " x)

    // Using ToString()
    let printList2 input = 
        sprintf "%O" input

module Day00 =
    open System    

    let readInput filePath =
        IO.File.ReadAllLines(filePath)

    let convertToIntList stringList =
        stringList |> Seq.map Int32.Parse

    let solve filePath =
        let input = readInput filePath
        let list = convertToIntList input
        Printer.printList list

    
        
    let isMatch a b =
        if a + b = 2020 then true
        else false

    let find2020a list =
        for i = 0 to list - 1 do
            for j=i+1 to list do
                if isMatch i j 
                then printf "%d" (i*j)

    // https://github.com/jilleJr/adventofcode-2020-fsharp/blob/master/src/Day01/Program.fs
    let printResult result = 
        printfn "F# calculation: %A" result
        
    let find2020pair list =
        let intsSet = Set.ofArray list
        list
        |> Array.tryFind (fun i -> intsSet |> Set.contains (2020 - i))
        |> function
            | Some i -> Some [i; 2020 - i]
            | None -> None

    let find2020b list = 
        printResult <| find2020pair list



    let solveA filePath =
        let input = readInput filePath
        let list = convertToIntList input
        // find2020a list
        find2020b list
        


// Temp test
module Say =
    let hello name =
        printfn "Hello %s" name