module knackaknacka.NaiveSwedish

open System.Collections
open System.Text.RegularExpressions

// Inspired by http://www2.hhs.se/isa/swedish/chap9.htm
// and https://en.wikipedia.org/wiki/Swedish_alphabet
// and https://en.wikipedia.org/wiki/Swedish_phonology

/// Vowels are assumed to be short before double consonants, long in all other cases
type Vowel = { long : string; short: string; soft : bool }
let vowels = 
        [
        "a", {long = "ɑː"; short = "a"; soft = false}; 
        "e", {long = "eː"; short = "ɛ"; soft = true};    // Some words exceptionally have ⟨e⟩ for /ɛ/, among them words with ⟨ej⟩, 
                                            // numerals, proper names and their derivations, and loanwords. Before 1889, 
                                            // ⟨e⟩ for /ɛ/ and /ɛː/ was also used for many other words, in particular 
                                            // words with ⟨je⟩ now spelled ⟨jä⟩. Many Swedes merge /ɛ/ and /e/.
                                            // The sound /eː/ at the end of loanwords and in the last syllable of Swedish 
                                            // surnames is represented by ⟨é⟩.
        "i", {long = "iː"; short = "ɪ"; soft = true};
        "o", {long = "uː"; short = "ɔ"; soft = false};    //	The phoneme /ʊ/ is relatively infrequent; short ⟨o⟩ more often represents /ɔ/. 
                                            // In a few words, long ⟨o⟩ represents /oː/.
        "u", {long = "ʉː"; short = "ɵ"; soft = false};
        "y", {long = "yː"; short = "ʏ"; soft = true};
        "å", {long = "oː"; short = "ɔ"; soft = false};    //	Most words with /ɔ/ and some words with /oː/ are spelled with ⟨o⟩.
        "ä", {long = "ɛː"; short = "ɛ"; soft = true};    //	Some words with /ɛ/ are spelled with ⟨e⟩.
        "ö", {long = "øː"; short = "œ"; soft = true};    //	The short ö is, in some dialects, pronounced as /ɵ/.
        ]

// Character class generation
let makeCharacterClass list =
    "[" + (list |> Seq.distinct |> Seq.fold (+) "") + "]"

let vowelFilter predicate = 
    vowels |> Seq.map (fun (s, v) -> s, v.soft) |> Seq.filter predicate |> Seq.map fst 
           |> makeCharacterClass


let hardVowelClass = vowelFilter (fun (_, soft) -> not soft)

let softVowelClass = vowelFilter (fun (_, soft) -> soft)

// before front vowel
let b4fv = softVowelClass

/// Consonants can be single character or multi-character
let consonantLookup =
        [
        "(b)", "b";
        "(c)", "s"    // before front vowels, otherwise /k/. ⟨e i y ä ö⟩. The letter ⟨c⟩ alone 
                    // is used only in loanwords (usually in the /s/ value) and proper names, 
                    // but ⟨ck⟩ is a normal representation for /k/ after a short vowel (as in 
                    // English and German).
        "(c)" + b4fv, "k";
        // "ch", "ɧ";  // In loanwords. The conjunction 'och' (and) is pronounced /ɔk/ or /ɔ/.
        "(ch)", "ɕ"; 
        "(d)", "d";
        "(dj)", "j";
        "(f)", "f";
        "(g)", "ɡ";   // /j/ before front vowels ⟨e i y ä ö⟩, otherwise /ɡ/
        "(g)", "j";
        "(gj)", "j";
//        "gn", "ɡn"; // /ɡn/ word-initially; /ŋn/ elsewhere
        "(gn)", "ŋn";
        "(h)", "h";
        "(hj)", "j";
        "(j)", "j";
        "(k)" + b4fv, "ɕ";   // /ɕ/ before front vowels ⟨e i y ä ö⟩, otherwise /k/
        "(k)", "k";
        "(kj)", "ɕ";
        "(l)", "l";
        "(lj)", "j";
        "(m)", "m";
        "(n)", "n";
        "(ng)", "ŋ";
//        "ng", "ŋɡ";
        "(p)", "p";
        "(r)", "r";   // Is pronounced as /ɾ/ in some words.
        "(s)", "s";
        "(sj)", "ɧ";
        "(sk)" + b4fv, "ɧ";  // /ɧ/ before front vowels ⟨e i y ä ö⟩, otherwise /sk/
        "(sk)", "sk";
        "(skj)", "ɧ";
        "(stj)", "ɧ";
        "(t)", "t";
        "(tj)", "ɕ";
        "(v)", "v";   // Before 1906, ⟨fv, hv⟩ and final ⟨f⟩ were also used for /v/. 
                    // Now these spellings are used in some proper names.
        "(w)", "v";   // Rarely used (loanwords, proper names). In loanwords from English 
                    // may be pronounced /w/.
        "(x)", "ks"; 
        "(z)", "s";   // Used in loanwords and proper names.
    ]

let singleConsonantClass = 
    consonantLookup |> Seq.map fst |> Seq.filter (fun s -> s.Length = 3) 
               |> makeCharacterClass

// followed by double consonants
let f2c = singleConsonantClass |> sprintf "(?<dc>%s)\\k<dc>" 

let longLookup = vowels |> Seq.map (fun (c, v) -> (c, v.long)) |> List.ofSeq
let shortPattern x = "(" + x + ")" + f2c
let shortLookup = vowels |> Seq.map (fun (c, v) -> (shortPattern c, v.short)) |> List.ofSeq
let allLookups = List.concat([consonantLookup; shortLookup; longLookup])
let makePattern x  = (x:(string * string) list)
                    |> List.map fst 
                    |> List.sortByDescending String.length
                    |> List.fold (+) "|"

let seBigPattern = makePattern allLookups

// Regex matching
let (|RegexMatch|_|) pattern input =
    if input = null || input.Equals "" then None
    else
        let metaPattern = sprintf @"^(%s)((?s).*)" pattern
        let m = Regex.Match(input, metaPattern, RegexOptions.Compiled)
        if m.Success then Some (m.Groups.[2].Value, m.Groups.[m.Groups.Count - 1].Value)
        else None

let ipaTranslateWithPattern pattern word = 
    let rec transUtil word acc = 
        match word with 
        | RegexMatch pattern (mtch, rest) -> transUtil rest (mtch::acc)
        | "" -> List.rev acc;    
        | _ -> List.rev acc;    
    transUtil word [] // |> Seq.fold (+) ""

let mc = ipaTranslateWithPattern seBigPattern 
