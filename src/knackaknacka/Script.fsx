
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

translateToIPA "köttbullar"

#I "../../packages/build/FParsec/lib/net40-client/"
#r "FParsec"
#r "FParsecCS"
open FParsec

let test p str =
    match run p str with
    | Success(result, _, _)   -> printfn "Success: %A" result
    | Failure(errorMsg, _, _) -> printfn "Failure: %s" errorMsg

test pfloat "-inf"

let str s = pstring s
let floatBetweenBrackets = str "[" >>. pfloat .>> str "]"