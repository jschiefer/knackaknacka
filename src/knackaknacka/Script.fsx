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

let theBigPattern = makePattern allLookups
let mc = ipaTranslateWithPattern theBigPattern

mc "att" 