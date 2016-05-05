#r "System.Speech.dll"

#load "NaiveSwedish.fs"
#load "Speaker.fs"

open System.Speech.Synthesis
open System.Text.RegularExpressions
open knackaknacka.NaiveSwedish
open knackaknacka.Speaker

// Say something
sayIpa "təmei̥ɾou̥"

// Installed voices
knackaknacka.Speaker.synth.GetInstalledVoices() 
|> Seq.map (fun x -> printfn "%A" x.VoiceInfo.Name)


mc "att"

let a = [consonantLookup; shortLookup; longLookup]
let first l = List.take 1 l
let first3 l = List.take 3 l
let seSmallList = a |> List.map (fun x -> first x) |> List.concat 
let seSmallPattern = seSmallList |> makePattern
let mat p s = Regex.Match(s, p)
let matches p s = (mat p s).Success

seSmallList
seSmallPattern
(mat seSmallPattern "abb").Groups.[0]

seBigPattern
