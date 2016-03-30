// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

// #r "System.Speech.dll"

#load "NaiveSwedish.fs"
// #load "Speaker.fs"

// open System.Speech.Synthesis
open System.Text.RegularExpressions
open knackaknacka.NaiveSwedish
// open knackaknacka.Speaker

// sayIpa "təmei̥ɾou̥"

let isVowel = function
    | HardVowel h -> "yes"
    | SoftVowel s -> "yes"
    | Consonant c -> "maybe"

let a = Regex.Match("abeecec", "(ee|a|b|c|e)+")
let b = a.Groups.[1].Captures |> Seq.cast<Capture> |> Seq.map (fun x -> x.Value) 

let c = tokenize "abeecec"

vowels.TryFind "a"

// knackaknacka.Speaker.synth.GetInstalledVoices()

let (|ParseRegex|_|) re s =
    let m = Regex(re).Match(s)
    if m.Success
    then Some [ for x in m.Groups -> x.Value ]
    else None

 ///Match the pattern using a cached compiled Regex
let (|CompiledMatch|_|) pattern input =
    if input = null then None
    else
        let m = Regex.Match(input, pattern, RegexOptions.Compiled)
        if m.Success then Some [for x in m.Groups -> x]
        else None

let parseString str = 
   match str with
   | CompiledMatch @"^(\w+) (\w+) (\w+)$" [_; first; middle; last] ->
        Some({First=first.Value; Middle=Some(middle.Value); Last=last.Value})
    | CompiledMatch @"^(\w+) (\w+)$" [_; first; last] ->
        Some({First=first.Value; Middle=None; Last=last.Value})
    | _ -> 
        None

let splitterOffer string = 
    ()
    
let rec step1 list =
    match list with
        | [] -> 
            "Done"
        | x :: xs -> 
            printfn "Encountered %A" x
            step1 xs

step1 [1..5]    

let (|RegexMatch|_|) pattern input =
    if input = null then None
    else
        let metaPattern = sprintf @"^(%s)((?s).*)" pattern
        let m = Regex.Match(input, metaPattern)
        if m.Success then Some [m.Groups.[1].Value; m.Groups.[2].Value]
        else None

let rec translatePhoneme letters =
    match letters with
    | RegexMatch @"ee" [mtch; rest] -> ["E"; rest] |> Some
    | RegexMatch @"(a|b)" [mtch; rest] -> ["A or B"; rest] |> Some
    | _ -> None

translatePhoneme "eeabcde"

let rec chop word =
    match word with
    | RegexMatch "." [mtch; rest] -> 
        printfn "Found %A" mtch
        printfn "Rest is %A" rest
        chop rest
    | _ ->
        printfn "Nothing"


let chop2 word = 
    let rec filterUtil word acc = 
        match word with 
        | RegexMatch "." [mtch; rest] -> 
            printfn "matched %A" mtch 
            filterUtil rest (mtch::acc)
        | _ ->  
            printfn "Done" 
            List.rev acc;    
    filterUtil word []




let a = chop2 "Fugu"