module knackaknacka.NaiveSwedish

open System.Collections
open System.Text.RegularExpressions

// Inspired by http://www2.hhs.se/isa/swedish/chap9.htm
// and https://en.wikipedia.org/wiki/Swedish_alphabet
// and https://en.wikipedia.org/wiki/Swedish_phonology
// and of course https://twitter.com/DUNSONnDRAGGAN

// Vowels can be hard or soft. A soft vowel (also known as a front vowel) changes
// the pronounciation of preciding consonants in some cases. See below in the 
// consonants list for examples.
type Vowel = { long : string; short: string; soft : bool }

// Vowels are assumed to be short before double consonants, long in all other cases. 
// This is probably an oversimplification
let vowels = [
        "a", {long = "ɑː"; short = "a"; soft = false}; 
        // Some words exceptionally have ⟨e⟩ for /ɛ/, among them words with ⟨ej⟩, 
        // numerals, proper names and their derivations, and loanwords. Before 1889, 
        // ⟨e⟩ for /ɛ/ and /ɛː/ was also used for many other words, in particular 
        // words with ⟨je⟩ now spelled ⟨jä⟩. Many Swedes merge /ɛ/ and /e/.
        // The sound /eː/ at the end of loanwords and in the last syllable of Swedish 
        // surnames is represented by ⟨é⟩.
        "e", {long = "eː"; short = "ɛ"; soft = true};    
        "i", {long = "iː"; short = "ɪ"; soft = true};
        //	The phoneme /ʊ/ is relatively infrequent; short ⟨o⟩ more often represents /ɔ/. 
        // In a few words, long ⟨o⟩ represents /oː/.
        "o", {long = "uː"; short = "ɔ"; soft = false};    
        "u", {long = "ʉː"; short = "ɵ"; soft = false};
        "y", {long = "yː"; short = "ʏ"; soft = true};
        //	Most words with /ɔ/ and some words with /oː/ are spelled with ⟨o⟩.
        "å", {long = "oː"; short = "ɔ"; soft = false};    
        //	Some words with /ɛ/ are spelled with ⟨e⟩.
        "ä", {long = "ɛː"; short = "ɛ"; soft = true};    
        //	The short ö is, in some dialects, pronounced as /ɵ/.
        "ö", {long = "øː"; short = "œ"; soft = true};    
        ]

let makeRegexCharacterClass list =
    "[" + (list |> Seq.distinct |> Seq.fold (+) "") + "]"

let vowelFilter pred = 
    vowels  |> Seq.map (fun (s, v) -> s, v.soft) 
            |> Seq.filter pred 
            |> Seq.map fst 
            |> makeRegexCharacterClass

let frontVowelRegexCharacterClass = vowelFilter (fun (_, soft) -> soft)

/// Consonants can be single character or multi-character
let consonants = [
        "(b)",    "b";
        // /s/ before front vowels, otherwise /k/. ⟨e i y ä ö⟩. The letter ⟨c⟩ alone 
        // is used only in loanwords (usually in the /s/ value) and proper names, 
        // but ⟨ck⟩ is a normal representation for /k/ after a short vowel (as in 
        // English and German).
        "(c)" + frontVowelRegexCharacterClass, 
                "s"      
        "(c)",  "k";
        "(ck)", "k";
        "(ch)", "ɕ"; 
        // REVISIT: Can't distinguish this case
        // In loanwords. 
        // "ch", "ɧ";   
        // The conjunction 'och' (and) is pronounced /ɔk/ or /ɔ/.
        "(och)","ɔk"; 
        "(d)",  "d";
        "(dj)", "j";
        "(f)",  "f";
        // 'g' is /j/ before front vowels ⟨e i y ä ö⟩, otherwise /ɡ/
        "(g)" + frontVowelRegexCharacterClass, 
                "j";
        "(g)",  "ɡ";     
        "(gj)", "j";
        // 'gn' is /ɡn/ word-initially; /ŋn/ elsewhere
        "(^gn)","ɡn";  
        "(gn)", "ŋn";
        "(h)",  "h";
        "(hj)", "j";
        "(j)",  "j";
        // 'k' is /ɕ/ before front vowels ⟨e i y ä ö⟩, otherwise /k/
        "(k)" + frontVowelRegexCharacterClass, 
                "ɕ";  
        "(k)",  "k";
        "(kj)", "ɕ";
        "(l)",  "l";
        "(lj)", "j";
        "(m)",  "m";
        "(n)",  "n";
        "(ng)", "ŋ";
        // REVISIT: Can't distinguish this case
        // "ng", "ŋɡ";
        "(p)",  "p";
        "(r)",  "r";
        "(s)",  "s";
        "(sj)", "ɧ";
        // 'sk' is /ɧ/ before front vowels ⟨e i y ä ö⟩, otherwise /sk/
        "(sk)" + frontVowelRegexCharacterClass, 
                "ɧ"; 
        "(sk)", "sk";
        "(skj)","ɧ";
        "(stj)","ɧ";
        "(t)",  "t";
        "(tj)", "ɕ";
        // Before 1906, ⟨fv, hv⟩ and final ⟨f⟩ were also used for /v/. 
        // Now these spellings are used in some proper names.
        "(v)",  "v";     
        // 'w' is rarely used (loanwords, proper names). In loanwords from English 
        // may be pronounced /w/.
        "(w)",  "v";     
        "(x)",  "ks"; 
        // used in loanwords and proper names.
        "(z)",  "s";     
    ]

let singleConsonantRegexCharacterClass = 
    consonants  |> Seq.map fst 
                |> Seq.filter (fun s -> s.Length = 3) 
                |> Seq.map (fun s -> s.Chars 1)
                |> Seq.map string
                |> makeRegexCharacterClass

let followedByDoubleConsonant = 
    singleConsonantRegexCharacterClass 
                |> sprintf "(?<dc>%s)\\k<dc>" 

let joinRegexAlternatives (xs:string list) = System.String.Join("|", xs)

let longPattern x = "(" + x + ")"
let longLookup = 
    vowels  |> Seq.map (fun (c, v) -> (longPattern c, v.long)) 
            |> List.ofSeq

let shortPattern x = longPattern x + followedByDoubleConsonant
let shortLookup = 
    vowels  |> Seq.map (fun (c, v) -> (shortPattern c, v.short)) 
            |> List.ofSeq

let allLookups = List.concat([consonants; shortLookup; longLookup])
let makePattern (xs:(string * string) list) =
    xs  |> List.map fst 
        |> List.sortByDescending String.length
        |> joinRegexAlternatives

// Regex matching
let (|FirstLevelMatch|_|) pattern input =
    if input = null || input.Equals "" then None
    else
        let metaPattern = sprintf @"(%s){1}(.*)" pattern
        let m = Regex.Match(input, metaPattern, RegexOptions.Compiled)
        if m.Success then Some (m.Groups.[1].Value, m.Groups.[m.Groups.Count - 2].Value)
        else None

let ipaTranslate pattern word = 
    let rec transUtil word acc = 
        match word with 
        | FirstLevelMatch pattern (mtch, rest) -> transUtil rest (mtch::acc)
        | "" -> List.rev acc;    
    transUtil word [] // |> Seq.fold (+) ""

