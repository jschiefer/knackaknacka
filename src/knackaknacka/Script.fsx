// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#r "System.Speech.dll"

#load "NaiveSwedish.fs"
#load "Speaker.fs"

open System.Speech.Synthesis
open System.Text.RegularExpressions
open knackaknacka.NaiveSwedish
open knackaknacka.Speaker

sayIpa "təmei̥ɾou̥"

let isVowel = function
    | HardVowel h -> "yes"
    | SoftVowel s -> "yes"
    | Consonant c -> "maybe"

let a = Regex.Match("abeecec", "(ee|a|b|c|e)+")
let b = a.Groups.[1].Captures |> Seq.cast<Capture> |> Seq.map (fun x -> x.Value) 

let c = tokenize "abeecec"

vowels.TryFind "a"

knackaknacka.Speaker.synth.GetInstalledVoices()
