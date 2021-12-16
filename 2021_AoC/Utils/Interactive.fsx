module Interactive
open System

let ex1 = "D2FE28"

// Other methods: Text.Encoding.ASCII.GetBytes, Byte.Parse(hex, Globalization.NumberStyles.AllowHexSpecifier)
let hexToBinary (c:char) = 
    let hex = new String(c, 1)
    let b = Int16.Parse(hex, Globalization.NumberStyles.AllowHexSpecifier)
    let c = Convert.ToString(b, 2)
    let d = c.PadLeft(4, '0')
    d

// The "F# way" 1
let hexToBinary' (c:char) = 
    c
    |> fun x -> new String(x, 1)
    |> fun x -> Int16.Parse(x, Globalization.NumberStyles.AllowHexSpecifier)
    |> fun x -> Convert.ToString(x, 2)
    |> fun x -> x.PadLeft(4, '0')   

let toBinaryText (s:string) =
    let binarySeq = Seq.map(fun i -> hexToBinary' i) s
    let binaryArray = Seq.concat binarySeq
    new String(Seq.toArray binaryArray)


// -------------
let solveA (input:string) =
    let binaryText = toBinaryText input
    printfn "%A" binaryText
    binaryText
    // 110100101111111000101000

solveA ex1


// reference
let fromHex (s:string) = 
  s
  |> Seq.windowed 2
  |> Seq.mapi (fun i j -> (i,j))
  |> Seq.filter (fun (i,j) -> i % 2=0)
  |> Seq.map (fun (_,j) -> Byte.Parse(new System.String(j),System.Globalization.NumberStyles.AllowHexSpecifier))
  |> Array.ofSeq