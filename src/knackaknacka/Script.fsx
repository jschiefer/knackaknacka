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

let phonemes = ipaTranslate "abc"

let minilist = [
    "(ee)" ,"EE";
    "(a)" + followedByDoubleConsonants ,"a";
    "(a)", "ɑː";
    "(b)", "b";
    "(c)", "c";
    "(d)", "d";
    "(e)", "e";
]

let minimap = new Map<string, string>(minilist)

let pat = "(ee|a|b|c|d|e)"

let makepattern = "(" + (minilist |> Seq.map fst |> String.concat "|") + ")"
let blah = ipaTranslateWithPattern makepattern "eeabc"

hardVowelClass