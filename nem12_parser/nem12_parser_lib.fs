namespace nem12_parser

module nem12 =
    open System.IO
    let readFile filePath =
        File.ReadAllLines filePath
        
    