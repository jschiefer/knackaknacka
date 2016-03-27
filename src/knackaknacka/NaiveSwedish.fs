module knackaknacka.NaiveSwedish

open System.Collections
open System.Text.RegularExpressions

// Inspired by http://www2.hhs.se/isa/swedish/chap9.htm
// and https://en.wikipedia.org/wiki/Swedish_alphabet
// and https://en.wikipedia.org/wiki/Swedish_phonology


type SoftVowel =        // also: front vowel
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

type Vowels = { 
        long : string;      // Before single consonants
        short: string }     // Before double consonants

let vowels = 
    new Map<string, Vowels>(
        [
        "a", {long = "ɑː"; short = "a"}; 
        "e", {long = "eː"; short = "ɛ"};    // Some words exceptionally have ⟨e⟩ for /ɛ/, among them words with ⟨ej⟩, 
                                            // numerals, proper names and their derivations, and loanwords. Before 1889, 
                                            // ⟨e⟩ for /ɛ/ and /ɛː/ was also used for many other words, in particular 
                                            // words with ⟨je⟩ now spelled ⟨jä⟩. Many Swedes merge /ɛ/ and /e/.
                                            // The sound /eː/ at the end of loanwords and in the last syllable of Swedish 
                                            // surnames is represented by ⟨é⟩.
        "i", {long = "iː"; short = "ɪ"};
        "o", {long = "uː"; short = "ɔ"};    //	The phoneme /ʊ/ is relatively infrequent; short ⟨o⟩ more often represents /ɔ/. 
                                            // In a few words, long ⟨o⟩ represents /oː/.
        "u", {long = "ʉː"; short = "ɵ"};
        "y", {long = "yː"; short = "ʏ"};
        "å", {long = "oː"; short = "ɔ"};    //	Most words with /ɔ/ and some words with /oː/ are spelled with ⟨o⟩.
        "ä", {long = "ɛː"; short = "ɛ"};    //	Some words with /ɛ/ are spelled with ⟨e⟩.
        "ö", {long = "øː"; short = "œ"};    //	The short ö is, in some dialects, pronounced as /ɵ/.
        ])

let consonants =
    new Map<string, string>(
        [
        "b", "b";
        "c", "s"    // before front vowels, otherwise /k/. ⟨e i y ä ö⟩. The letter ⟨c⟩ alone 
                    // is used only in loanwords (usually in the /s/ value) and proper names, 
                    // but ⟨ck⟩ is a normal representation for /k/ after a short vowel (as in 
                    // English and German).
        "c", "k";
        "ch", "ɧ";  // In loanwords. The conjunction 'och' (and) is pronounced /ɔk/ or /ɔ/.
        "ch", "ɕ"; 
        "d", "d";
        "dj", "j";
        "f", "f";
        "g", "ɡ";   // /j/ before front vowels ⟨e i y ä ö⟩, otherwise /ɡ/
        "g", "j";
        "gj", "j";
        "gn", "ɡn"; // /ɡn/ word-initially; /ŋn/ elsewhere
        "gn", "ŋn";
        "h", "h";
        "hj", "j";
        "j", "j";
        "k", "ɕ";   // /ɕ/ before front vowels ⟨e i y ä ö⟩, otherwise /k/
        "k", "k";
        "kj", "ɕ";
        "l", "l";
        "lj", "j";
        "m", "m";
        "n", "n";
        "ng", "ŋ";
        "ng", "ŋɡ";
        "p", "p";
        "r", "r";   // Is pronounced as /ɾ/ in some words.
        "s", "s";
        "sj", "ɧ";
        "sk", "ɧ";  // /ɧ/ before front vowels ⟨e i y ä ö⟩, otherwise /sk/
        "sk", "sk";
        "skj", "ɧ";
        "stj", "ɧ";
        "t", "t";
        "tj", "ɕ";
        "v", "v";   // Before 1906, ⟨fv, hv⟩ and final ⟨f⟩ were also used for /v/. 
                    // Now these spellings are used in some proper names.
        "w", "v";   // Rarely used (loanwords, proper names). In loanwords from English 
                    // may be pronounced /w/.
        "x", "ks"; 
        "z", "s";   // Used in loanwords and proper names.
    ])


let translate word =
    word

let makeMatchString() =
    let concat = Phonelist |> Seq.map (fun x -> fst x) |> String.concat "|" 
    "(" + concat + ")"

let tokenize str =
    let m = Regex.Match(str, makeMatchString() + "+", RegexOptions.IgnoreCase)
    m.Groups.[1].Captures |> Seq.cast<Capture> |> Seq.map (fun x -> x.Value) 

