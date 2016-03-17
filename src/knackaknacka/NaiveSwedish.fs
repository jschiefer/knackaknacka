module knackaknacka.NaiveSwedish

open System.Collections
open System.Text.RegularExpressions

// Inspired by http://www2.hhs.se/isa/swedish/chap9.htm

type SoftVowel = 
    | E | I | Y | Ä | Ö

type HardVowel = 
    | A | O | U | Ø

type Consonant = 
    | B | C | D | F | G | H | J | K | L | M | N | P | Q | R | S | T | V | W | X | Y | Z

type Letter =
    | SoftVowel of SoftVowel
    | HardVowel of HardVowel
    | Consonant of Consonant

type Group =
    | EE | AA | BB

type Grapheme =
    | Letter of Letter
    | Group of Group

let Phonelist = [ 
    "ee",   Group(EE)
    "a",    Letter(HardVowel A)
    "b",    Letter(Consonant B)
    "c",    Letter(Consonant C) 
    "d",    Letter(Consonant D) 
    "e",    Letter(SoftVowel E)
] 

let translate word =
    word

let makeMatchString() =
    let concat = Phonelist |> Seq.map (fun x -> fst x) |> String.concat "|" 
    "(" + concat + ")"

let tokenize str =
    let m = Regex.Match(str, makeMatchString() + "+", RegexOptions.IgnoreCase)
    m.Groups.[1].Captures |> Seq.cast<Capture> |> Seq.map (fun x -> x.Value) 

