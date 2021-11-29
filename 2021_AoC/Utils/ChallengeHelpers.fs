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

//Hello F# call
//find2020 start
//sum test: 1914 1931 = 3845
//sum test: 1931 1892 = 3823
//sum test: 1892 1584 = 3476
//sum test: 1584 1546 = 3130
//sum test: 1546 1988 = 3534
//sum test: 1988 1494 = 3482
//sum test: 1494 1709 = 3203
//sum test: 1709 1624 = 3333
//sum test: 1624 1755 = 3379
//sum test: 1755 1849 = 3604
//sum test: 1849 1430 = 3279