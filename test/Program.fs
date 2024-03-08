open nem12_parser
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
"nem12_sample.txt"
|>nem12.readFile
|>nem12.getNMIs
|> Seq.iter (fun s -> printfn $"{s}")