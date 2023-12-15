module Day01FirstVersion

open NUnit.Framework
open System

[<SetUp>]
let Setup () =
    ()

// A

// Find first and last digit in each line, combine and convert to int
// Char to int https://stackoverflow.com/questions/42253284/f-check-if-a-string-contains-only-number
// Options https://learn.microsoft.com/en-us/dotnet/fsharp/language-reference/options
// Explanation for head::tail https://thinkfunctionally.hashnode.dev/recursive-functions-in-f-sharp
[<Test>]
let A () =
    let input = Utils.GetInput("01")

    let isNumber (c: char) = Char.IsDigit c
    //let parseNumber (c: char) = Int32.Parse(c.ToString())

    // Find first digit in string and parse to int
    let findFirstDigit (line: string) =
        let rec loop = function
        | head::tail -> if isNumber head then head.ToString() else loop tail
        | [] -> "0"
        loop (Seq.toList line)

    // Convert to List<char> -> reverse -> to array -> new string(array)
    let reverseString (line: string) =
        line |> Seq.toList |> List.rev |> List.toArray |> String

    let solveLine (line: string) = 
        Int32.Parse(findFirstDigit line + findFirstDigit(reverseString line))

    let solvePuzzle (seq: string array) =
        let rec solveRec seq acc =
            match seq with
            | head :: tail -> solveRec tail (solveLine head + acc)
            | [] -> acc 

        solveRec (Array.toList seq) 0
    
    Assert.That(findFirstDigit "5aa", Is.EqualTo("5"))
    Assert.That(findFirstDigit "a5aa", Is.EqualTo("5"))
    Assert.That(findFirstDigit "aa5", Is.EqualTo("5"))
    Assert.That(findFirstDigit "5", Is.EqualTo("5"))
    
    Assert.That(solveLine "5", Is.EqualTo(55))
    Assert.That(solveLine "5a6", Is.EqualTo(56))
    Assert.That(solveLine "5a3a6", Is.EqualTo(56))


    Assert.That(solvePuzzle [|"5a6"; "9"|], Is.EqualTo(56+99))
    Assert.That(solvePuzzle [|"aaaa56aaaa"; "aaaaaa9aaaaa"|], Is.EqualTo(56+99))
    Assert.That(solvePuzzle input, Is.EqualTo(53334))

// B

// Same, but include also written digits like "one", "five"
// https://stackoverflow.com/questions/40385154/f-count-how-many-times-a-substring-contains-within-a-string
// match expression tutorial https://fsharpforfunandprofit.com/posts/match-expression/
[<Test>]
let B () = 
    let input = Utils.GetInput("01")

    let letterValues = 
        [("one", 1); ("two", 2); ("three", 3); ("four", 4); ("five", 5); 
        ("six", 6); ("seven", 7); ("eight", 8); ("nine", 9)]

    let startsWithLetter (str: string) =
        let fitsNumberLetters (str: string) (letterName: string) (value: int) =
            if letterName.Length > str.Length then 0
            else 
                if str.Substring(0, letterName.Length) = letterName then value
                else 0

        let result = List.map (fun (x,y) -> fitsNumberLetters str x y) letterValues
        let result2 = List.filter (fun x -> x > 0) result
        match result2 with
        | [] -> 0
        | _ -> result2.Head

    Assert.That(startsWithLetter "one", Is.EqualTo(1))
    Assert.That(startsWithLetter "onesd", Is.EqualTo(1))
    Assert.That(startsWithLetter "two", Is.EqualTo(2))
    Assert.That(startsWithLetter "nine", Is.EqualTo(9))
    Assert.That(startsWithLetter "0nine", Is.EqualTo(0))
    Assert.That(startsWithLetter "o", Is.EqualTo(0))


    let startsWithDigit (str: string) = 
        if str.Length < 1 then 0
        else
            match Int32.TryParse (str.Substring(0, 1)) with
            | true, value -> value
            | false, _ -> 0
    
    Assert.That(startsWithDigit "2a", Is.EqualTo(2))
    Assert.That(startsWithDigit "a2", Is.EqualTo(0))

    let evaluateIfStartWithNumber substr =
        // TODO this is awkward. How to directly return value if it's > 0
        let a = startsWithLetter substr
        if a > 0 then Some(a)
        else
            let b = startsWithDigit substr
            if b > 0 then Some(b)
            else None
    
    // Collect all numbers (written or digits) to new list
    let collectNumbers (line: string) =
        let rec iter (substr: string) (result: int list) =
            if substr.Length > 0 then 
                let next = substr.Substring(1)
                match evaluateIfStartWithNumber substr with
                | Some value -> iter next (result @ [value])
                | None -> iter next result
            else result

        let unfilteredValues = iter line []
        List.filter (fun x -> x > 0) unfilteredValues

    let chooseFirstAndLast numList =
        let ends = [List.head numList; List.last numList]
        let chars = List.map (fun x -> x |> string) ends
        // https://stackoverflow.com/questions/4278531/f-string-join-and-operator
        let concat = String.concat "" chars
        Int32.Parse concat

    // 
    let solvePuzzle (input: string array) =
        let rec sumLoop lines acc = 
            match lines with
            | [] -> acc
            | head::tail ->
                let lineRes = collectNumbers head |> chooseFirstAndLast
                sumLoop tail (acc+lineRes)
        sumLoop (Array.toList input) 0


    // jptwoeight6fourfrbpgsmkgl 24
    // twone 21
    Assert.That(collectNumbers "jptwoeight6fourfrbpgsmkgl" |> chooseFirstAndLast, Is.EqualTo(24))
    Assert.That(collectNumbers "twone" |> chooseFirstAndLast, Is.EqualTo(21))
    Assert.That(solvePuzzle input, Is.EqualTo(52834))
