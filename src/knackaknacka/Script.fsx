// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#load "NaiveSwedish.fs"

open System.Text.RegularExpressions
// open knackaknacka.Speaker
open knackaknacka.NaiveSwedish

// sayIpa "təmei̥ɾou̥"

let isVowel = function
    | HardVowel h -> "yes"
    | SoftVowel s -> "yes"
    | Consonant c -> "maybe"

let a = Regex.Match("abeecec", "(ee|a|b|c|e)+")
let b = a.Groups.[1].Captures |> Seq.cast<Capture> |> Seq.map (fun x -> x.Value) 

let c = tokenize "abeecec"

let m = 
    new Map<string, string>(
        [
        "hello", "world"; 
        "foo", "bar"
        ])

m.TryFind "helo"
