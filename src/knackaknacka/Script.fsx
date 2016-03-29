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

let (|TheMatch|_|) pattern input =
    if input = null then None
    else
        let metaPattern = sprintf @"^(%s)(.+)$" pattern
        let m = Regex.Match(input, metaPattern)
        if m.Success then Some (m.Groups.[1], m.Groups.[2])
        else None


let translatePhoneme letters =
    match letters with
    | TheMatch @"ee" (mtch, rest) -> Some("E", mtch)
    | _ -> None

 translatePhoneme "eedd"