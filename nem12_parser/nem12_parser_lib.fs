namespace nem12_parser

module nem12 =
    open System.IO
    open System
    type intervalData =
        { timestamp: DateTimeOffset
          value: float }
    type NMIdata =
        { data: intervalData array
          unit: string }
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
    
    //output of NMIdata array
    //given SUB0002199 -> {unit = KWh; data = [{timestamp = 20220525 00:00:00; value = 0}]}
    let getData (fileContent:string array) (nmi:string) =
        let getUnitInterval (line:string) =
            let lineContent = line.Split(",")
            if lineContent[0] ="200" then
                if lineContent[1] = nmi then
                    Some (Convert.ToInt32 lineContent[lineContent.Length - 2], lineContent[lineContent.Length - 3])
                else
                    None
            else
                None
        for i in [0..fileContent.Length - 1 ] do
            match getUnitInterval fileContent[i] with
            | None -> ()
            | Some (interval, unit) ->
                printfn $"interval={interval} unit={unit}"
                ()
        ()
        
    