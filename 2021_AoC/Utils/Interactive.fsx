module Interactive
open System

// 110100101111111000101000
let ex1 = "D2FE28"


type LengthType = 
    /// Total length in next 15 bits
    | TotalLength 
    /// Total number of packets in next 11 bits
    | NumberOfSubPackets

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

let binaryToInt (s:string) =
    Convert.ToInt32(s, 2)


let toBinaryText (s:string) =
    let binarySeq = Seq.map(fun i -> hexToBinary' i) s
    let binaryArray = Seq.concat binarySeq
    new String(Seq.toArray binaryArray)

let rec readValuePacket (s:string) depth =
    match s with
    | s when s.StartsWith('1') -> 
        printfn "%i: %s" depth s[0..4]
        readValuePacket s.[5..] (depth+1)
    | s when s.StartsWith('0') -> printfn "%i: %s" depth s
    | _ -> ()

//printfn "%i: %s" 5 "asd"



//
let readPacketOfLength (s:string) =
    let length = binaryToInt s.[0..14]
    ()

let readPacketOfAmount (s:string) =
    let amount = binaryToInt s.[0..10]
    // todo rec function
    // example
    //> let rev l =
    //    let rec loop acc = function
    //        | [] -> acc
    //        | x::xs -> loop (x::acc) xs
    //    loop [] l
    ()

 //
let readNewPacket (s:string) =
    if(s[0] = '0') then readPacketOfLength s.[1..]
    else readPacketOfAmount s.[1..]

let readSubPacket (s:string) =
    let packetVersion = binaryToInt s.[0..2]
    printfn "Received packet version: %i" packetVersion
    let typeID = binaryToInt s.[3..5]
    if typeID = 4 then readValuePacket s.[6..] 1
    else readNewPacket s.[6..]
    
// 110100101111111000101000
// 110 100 10111 11110 00101 000
// -------------
let solveA (input:string) =
    let binaryText = toBinaryText input
    printfn "%A" binaryText
    let packetVersion = binaryToInt binaryText.[0..2]
    printfn "Received packet version: %i" packetVersion
    let typeID = binaryToInt binaryText.[3..5]
    if typeID = 4 then readValuePacket binaryText.[6..] 1
    else readNewPacket binaryText.[6..]       
    ()

solveA ex1


// reference
let fromHex (s:string) = 
  s
  |> Seq.windowed 2
  |> Seq.mapi (fun i j -> (i,j))
  |> Seq.filter (fun (i,j) -> i % 2=0)
  |> Seq.map (fun (_,j) -> Byte.Parse(new System.String(j),System.Globalization.NumberStyles.AllowHexSpecifier))
  |> Array.ofSeq
  

