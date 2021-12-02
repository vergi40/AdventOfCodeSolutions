module Interactive

let list = [1; 1000; 2; 3; 555; 1010; 1020; 666]
let list2 = [1; 1000; 2; 3; 555; 1010; 1020; 666]

let solveA list =
    let list2 = List.pairwise list
    let increased = List.filter (fun (x,y) -> y > x) list2
    increased.Length

solveA list

let solveB seq =
    let list = Seq.toList seq
    let listsOf3 = List.windowed 3 list
    // Important to define target mapping type (each member is int list)
    let sumsOf3 = List.map (fun (values: int list) -> (values[0] + values[1] + values[2])) listsOf3

    let result2 = solveA sumsOf3
    printfn "%A" result2

solveB list2