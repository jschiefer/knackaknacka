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

knackaknacka.Speaker.synth.GetInstalledVoices() 
|> Seq.map (fun x -> printfn "%A" x.VoiceInfo.Name)

let pattern = "(a)" + f2c
let a = Regex.Match("att", pattern)
a.Groups.[1].Value
a.Success

let shortLookup = vowels |> Seq.map (fun (c, v) -> (c, v.short))
let longPattern x = "(" + x + ")" + f2c
let longLookup = vowels |> Seq.map (fun (c, v) -> (longPattern c, v.long))

shortLookup
longLookup

(2 * vowels.Length + consonants.Length) 
