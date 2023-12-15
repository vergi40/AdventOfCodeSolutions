module Utils

open System.IO

// dayName as "01" or "22"
let GetInput dayName = 
    let folder = @"C:\Git\AdventOfCodeSolutions\2023_Aoc\Input"
    let filePath = Path.Combine(folder, dayName + ".txt")
    File.ReadAllLines(filePath)

// dayName as "01" or "22"
let GetInputAsList dayName = 
    let folder = @"C:\Git\AdventOfCodeSolutions\2023_Aoc\Input"
    let filePath = Path.Combine(folder, dayName + ".txt")
    File.ReadAllLines(filePath) |> Array.toList

    