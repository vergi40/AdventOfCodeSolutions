module Day01

open NUnit.Framework

[<Test>]
let A () =

    let parseToTwoNumbers (line: string) = 
        let split = line.Split("   ")
        (int split[0], int split[1])

    // Map2: two lists -> function -> one list
    let sumListItemDiffs list1 list2 =
        List.map2 (fun x y -> abs(x - y)) list1 list2 |> List.sum

    let input = Utils.GetInputAsList "01"

    // List[left, right]
    let numbers = input |> List.map parseToTwoNumbers

    // List[left], List[right]
    let numbersLeft = numbers |> List.map fst
    let numbersRight = numbers |> List.map snd

    let sorted1 = List.sort numbersLeft
    let sorted2 = List.sort numbersRight

    let result = sumListItemDiffs sorted1 sorted2
    Assert.That(result, Is.EqualTo(2166959))


[<Test>]
let B () =

    let parseToTwoNumbers (line: string) = 
        let split = line.Split("   ")
        (int split[0], int split[1])

    let timesExist number list =
        List.filter(fun x -> x = number) list |> List.length

    // The puzzle requires multiplying timeExist with the number itself
    let similarityScore number list =
        (timesExist number list) * number

    // Unoptimized and unnecessary. Should use mutable dict which is checked after each addition
    let countsToDict' leftList rightList =
        leftList
        |> List.map (fun number -> (number, timesExist number rightList))
        |> Map.ofList

    let input = Utils.GetInputAsList "01"

    // List[left, right]
    let numbers = input |> List.map parseToTwoNumbers

    // List[left], List[right]
    let numbersLeft = numbers |> List.map fst
    let numbersRight = numbers |> List.map snd

    // Unoptimized
    let result = List.map (fun x -> similarityScore x numbersRight) numbersLeft |> List.sum
    Assert.That(result, Is.EqualTo(23741109))
