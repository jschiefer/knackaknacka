#r "System.Speech.dll"

#load "NaiveSwedish.fs"
#load "Speaker.fs"

open System.Speech.Synthesis
open System.Text.RegularExpressions
open knackaknacka.NaiveSwedish
open knackaknacka.Speaker

// Say something
sayIpa "təmei̥ɾou̥"
sayIpa "ɕøːttbɵllɑːr"   // naive
sayIpa "ɕœtbɵlːar"      // correct


// Installed voices
knackaknacka.Speaker.synth.GetInstalledVoices() 
|> Seq.map (fun x -> printfn "%A" x.VoiceInfo.Name)

let theBigPattern = makePattern allLookups
let mc = ipaTranslate theBigPattern

let f = mc "köttbullar"
theBigPattern

let metaPattern = makePattern allLookups |> sprintf @"(%s){1}(.*)" 

metaPattern
