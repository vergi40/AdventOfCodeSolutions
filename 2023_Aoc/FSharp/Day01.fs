module Day01

open NUnit.Framework
open System

[<SetUp>]
let Setup () =
    ()

// A

// Find first and last digit in each line, combine and convert to int
// Redo A with more lessons learned
// https://fsharpforfunandprofit.com/posts/convenience-active-patterns/
[<Test>]
let A' () =
    let prepareDigits line =
        let first = Seq.find (fun char -> Char.IsDigit(char)) line
        let last = Seq.findBack (fun char -> Char.IsDigit(char)) line
        (first.ToString() + last.ToString()) |> int

    let solvePuzzle list =
        // collect digits. select first and last, convert to int. sum all entries
        List.map (fun line -> prepareDigits line) list
        |> List.sum

    let input = Utils.GetInputAsList "01"
    Assert.That(solvePuzzle input, Is.EqualTo(53334))


// B

// Same, but include also written digits like "one", "five"
// Redo B with more lessons learned
// https://fsharpforfunandprofit.com/posts/convenience-active-patterns/
[<Test>]
let B' () = 
    let (|LetterName|_|) (str: string) =
        let letterValues =  
            [("one", 1); ("two", 2); ("three", 3); ("four", 4); ("five", 5); 
            ("six", 6); ("seven", 7); ("eight", 8); ("nine", 9)]

        let fitsNumberLetters (str: string) (letterName: string) (value: int) =
            if letterName.Length <= str.Length && str.Substring(0, letterName.Length) = letterName 
            then value else 0

        // Try each letterName. If fits, return value (which is over 0)
        let result = letterValues |> List.map (fun (x,y) -> fitsNumberLetters str x y) |> List.filter (fun x -> x > 0)
        match result with
        | [] -> None
        | _ -> Some(result.Head)

    let (|Number|_|) (str: string) =
        if str.Length < 1 then None
        else
            match Int32.TryParse(str.Substring(0, 1)) with
            | (true, int) -> Some(int)
            | (false, _) -> None

    // Collect all numbers (written or digits) to new list
    let collectNumbers (line: string) =
        let rec iter (substr: string) (result: int list) =
            if substr.Length > 0 then 
                match substr with
                | LetterName letter -> iter (substr.Substring(1)) (result @ [letter])
                | Number number -> iter (substr.Substring(1)) (result @ [number])
                | _ -> iter (substr.Substring(1)) result
            else result
        iter line []

    let chooseFirstAndLast numList =
        let ends = [List.head numList; List.last numList]
        ends 
        |> List.map (fun x -> x |> string) 
        |> String.concat "" 
        |> Int32.Parse

    // 
    let solvePuzzle list =
        // Pick all numbers from each line, select first and last as converted integer
        let sumsList = List.map(fun line -> collectNumbers line |> chooseFirstAndLast) list
        // Sum everything as answer
        List.sum sumsList

    let input = Utils.GetInputAsList "01"
    Assert.That(solvePuzzle input, Is.EqualTo(52834))
