open nem12_parser
open System.Collections.Generic
// let lines = nem12.readFile "nem12_sample.txt"
// let NMIs = ResizeArray<string>()
// for line in lines do
//     printfn $"%s{line}"
//     let x = line.Split(",")
//     if x.Length >= 2 then
//         printfn $"%s{x[0]} %s{x[1]}"
//         if x[0] = "200" then
//             printfn $"NMI= {x[1]}"
//             NMIs.Add x[1]
// NMIs
// |> Seq.distinct
// |> Seq.iter (fun s -> printfn $"{s}")
// printfn "done"
// "nem12_sample.txt"
// |> nem12.readFile
// |> nem12.getNMIs
// |> Seq.iter (fun s -> printfn $"{s}")
// let fileContent = nem12.readFile "nem12_sample.txt"
// let NMIs = nem12.getNMIs fileContent
// nem12.getIntervalData fileContent NMIs[0]
// for nmi in NMIs do
//   nem12.parserNMIData data nmi fileContent
let nmiFile =
  "nem12_sample.txt"
  |> nem12.parseNmiFile
()