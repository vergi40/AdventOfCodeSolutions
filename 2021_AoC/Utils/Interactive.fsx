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


let solveA (input:string) =
    let binaryList = Seq.map(fun i -> hexToBinary i) input
    let binary = Seq.concat binaryList
    let binaryText = new String(Seq.toArray binary)
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