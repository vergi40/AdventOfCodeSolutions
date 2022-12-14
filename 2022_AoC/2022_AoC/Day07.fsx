// module Day07

// Select all
// Execute in Interactive: Alt + Enter

let readLines filePath =
    System.IO.File.ReadAllLines(filePath) |> Array.toList

// readLines "Input\07.txt"

// Create data structure

// https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/active-patterns
// https://en.wikibooks.org/wiki/F_Sharp_Programming/Active_Patterns
let (|ListContent|CdChildDir|CdParentDir|Root|DirInfo|FileInfo|) (processLine : string) =
    match processLine with
    | cmd when cmd.StartsWith("$ ls") -> ListContent
    | cmd when cmd.StartsWith("$ cd ..") -> CdParentDir
    | cmd when cmd.StartsWith("$ cd ") -> CdChildDir
    | cmd when cmd.StartsWith("$ cd /") -> Root
    | cmd when cmd.StartsWith("dir") -> DirInfo
    | _ -> FileInfo

let parseFileSize (command : string) =
    printfn "Try parse %A" command
    0

// TODO also return skipped count
let calculateDirContent items =
    let isNotCommand item =
        match item with
        | FileInfo -> true
        | _ -> false
    let subList = List.takeWhile (fun item -> isNotCommand item) items
    let rec sumAll acc files =
        match files with
        | [] -> acc
        | head :: tail -> 
            let sum = acc + parseFileSize head
            sumAll sum tail
    sumAll 0 subList

// Traverse. when .., calculate dir
let rec traverse (totalSize : int) operations =
    match operations with
    | [] -> totalSize
    | head :: tail -> 
        match head with
        | FileInfo -> traverse totalSize tail
        | ListContent -> 
            let newSize = totalSize + calculateDirContent tail
            traverse newSize tail
        | CdParentDir | CdChildDir | Root -> traverse totalSize tail
        | DirInfo -> 0 // How to handle dummy case?

// Calculate file sizes


// Solve A
let content = readLines "Input\07.txt" 
let answer = traverse 0 content
printfn "%A" answer