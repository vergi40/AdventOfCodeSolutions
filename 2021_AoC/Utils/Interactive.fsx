module Interactive
open System

let ex1 = "D2FE28"

// Other methods: Text.Encoding.ASCII.GetBytes, Byte.Parse(hex, Globalization.NumberStyles.AllowHexSpecifier)
let hexToBinary (c:char) = 
    let hex = new String(c, 1)
    let b = Int16.Parse(hex, Globalization.NumberStyles.AllowHexSpecifier)
    let c = Convert.ToString(b, 2)
    c


let solveA (input:string) =
    hexToBinary input[0]


solveA ex1    