namespace ChallengeHelpers

module Day00 =

    let is2020 a b =
        // printfn "sum test: %d %d = %d" a b (a+b)
        if a + b = 2020 then true
        else false

    let rec iterate' items =
        match items with
            | [] -> (0,0)
            | head::tail -> 
                // todo 2020 check here
                iterate' tail

    let rec iterateAndMatchToA items a =
        match items with
            | [] -> 
                (0,0)
            | head::_ when is2020 head a ->
                printfn "found 2020 pair: %d and %d" a head
                (a, head)
            | _::tail -> 
                // todo 2020 check here
                iterateAndMatchToA tail a


    let rec highIteration items =
        match items with 
            | [] -> (0,0)
            | head::tail -> 
                let result = iterateAndMatchToA tail head
                if result = (0,0) then highIteration tail
                else result

    let solveA list = 
        let list2 = List.ofSeq(list)
        printfn "find2020 start"
        let result = highIteration list2
        printfn "find2020 finished"
        result

module Day01 = 

    let solveA seq =
        let list = Seq.toList seq
        let list2 = List.pairwise list
        let increased = List.filter (fun (x,y) -> y > x) list2
        increased.Length

    let solveB seq =
        let list = Seq.toList seq
        let listsOf3 = List.windowed 3 list
        // Important to define target mapping type (each member is int list)
        let sumsOf3 = List.map (fun (values: int list) -> (values[0] + values[1] + values[2])) listsOf3
    
        // Run the sums as part A
        solveA sumsOf3
    
