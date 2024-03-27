namespace nem12_parser

open System.Collections.Generic

module nem12 =
  open System.IO
  open System

  type intervalData =
    { timestamp: DateTimeOffset
      value: float }

  type NMI = string
  type NMISuffix = string
  type Unit = string
  type NMIdata = Dictionary<NMI, Dictionary<NMISuffix * Unit, intervalData[]>>

  let readFile filePath = File.ReadAllLines filePath

  let getNMIs (fileContent: string array) =
    let NMIs = ResizeArray<string>()

    for line in fileContent do
      let lineContent = line.Split(",")

      if lineContent.Length >= 2 then
        if lineContent[0] = "200" then
          NMIs.Add lineContent[1]

    NMIs |> Seq.distinct |> Array.ofSeq

  let parserNMISuffix (line200: string) : NMISuffix * Unit =
    let lineContent = line200.Split(",")
    let nmiSuffix = lineContent[4]
    let unit = lineContent[7]
    (nmiSuffix, unit)


  let parseLineIntervalData (line300: string) (interval_in_minute: int) : intervalData[] =
    let data = ResizeArray<intervalData>()
    let lineContent = line300.Split(",")

    if lineContent[0] <> "300" then
      failwith "Invalid line content"

    let start_date = DateTimeOffset.ParseExact(lineContent[1], "yyyyMMdd", null)

    for i in [ 2 .. lineContent.Length - 6 ] do
      { timestamp = start_date.AddMinutes((i - 2) * interval_in_minute |> float)
        value = float lineContent[i] }
      |> data.Add
    data.ToArray()

  //output of NMIdata array
  //given SUB0002199 -> {unit = KWh; data = [{timestamp = 20220525 00:00:00; value = 0}]}
  let getIntervalData (fileContent: string array) (nmi: string) =
    let getUnitInterval (line: string) =
      let lineContent = line.Split(",")

      if lineContent[0] = "200" then
        if lineContent[1] = nmi then
          Some(Convert.ToInt32 lineContent[lineContent.Length - 2], lineContent[lineContent.Length - 3])
        else
          None
      else
        None

    for i in [ 0 .. fileContent.Length - 1 ] do
      match getUnitInterval fileContent[i] with
      | None -> ()
      | Some(interval, unit) -> printfn $"interval={interval} unit={unit}"

  let parseNMISuffixData
    (suffixData: Dictionary<NMISuffix * Unit, intervalData[]>)
    (line200: string)
    (line300: string)
    =
    let nmiData = parserNMISuffix line200

    int (line200.Split(",")[8])
    |> parseLineIntervalData line300
    |> (fun d -> suffixData.Add((nmiData, d)))


  let parserNMIData (data: NMIdata) (nmi: NMI) (fileContent: string[]) =
    // let data : NMIdata = Dictionary<NMI, Dictionary<NMISuffix * Unit, intervalData []>>()
    let suffixData = Dictionary<NMISuffix * Unit, intervalData[]>()

    for i in [ 0 .. fileContent.Length - 1 ] do
          if Convert.ToInt32 (fileContent[i].Split(",")[0]) = 200 then
            if fileContent[i].Split(",")[1] = nmi then
              parseNMISuffixData suffixData fileContent[i] fileContent[i + 1]

    data.Add(nmi, suffixData)
    
  let parseNmiFile (filePath:String) : NMIdata =
      let data : NMIdata = Dictionary<NMI, Dictionary<NMISuffix * Unit, intervalData []>>()
      let NMIs = filePath |> readFile |> getNMIs
      for nmi in NMIs do
        parserNMIData data nmi (readFile filePath)
      data