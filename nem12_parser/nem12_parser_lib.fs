namespace nem12_parser

module nem12 =
    open System.IO
    let readFile filePath =
        File.ReadAllLines filePath
        
    let getNMIs (fileContent:string array) =
        let NMIs = ResizeArray<string>()
        for line in fileContent do
            let lineContent = line.Split(",")
            if lineContent.Length >= 2 then
                if lineContent[0] ="200" then
                    NMIs.Add lineContent[1]
        NMIs
        |> Seq.distinct
        |> Array.ofSeq