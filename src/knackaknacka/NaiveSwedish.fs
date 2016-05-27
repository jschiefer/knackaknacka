module knackaknacka.NaiveSwedish

open System.Collections
open System.Text.RegularExpressions

// Inspired by http://www2.hhs.se/isa/swedish/chap9.htm
// and https://en.wikipedia.org/wiki/Swedish_alphabet
// and https://en.wikipedia.org/wiki/Swedish_phonology
// and of course https://twitter.com/DUNSONnDRAGGAN


// We are trying to break any word into a list of graphemes, whose pronounciations 
// (phonemes) we will then look up.
type Grapheme =
    | LongVowel of char 
    | ShortVowel of char
    | SoftableConsonant of char
    | SingleConsonant of char
    | Conjunction of string

// Vowels can be hard or soft. A soft vowel (also known as a front vowel) changes
// the pronounciation of preciding consonants in some cases. See below in the 
// consonants list for examples.
type Vowel = { longPronounciation : string; shortPronounciation: string; soft : bool }

// Vowels are assumed to be short before double consonants, long in all other cases. 
// This is probably an oversimplification
let vowels = [
        "a", {longPronounciation = "ɑː"; shortPronounciation = "a"; soft = false}; 
        // Some words exceptionally have ⟨e⟩ for /ɛ/, among them words with ⟨ej⟩, 
        // numerals, proper names and their derivations, and loanwords. Before 1889, 
        // ⟨e⟩ for /ɛ/ and /ɛː/ was also used for many other words, in particular 
        // words with ⟨je⟩ now spelled ⟨jä⟩. Many Swedes merge /ɛ/ and /e/.
        // The sound /eː/ at the end of loanwords and in the last syllable of Swedish 
        // surnames is represented by ⟨é⟩.
        "e", {longPronounciation = "eː"; shortPronounciation = "ɛ"; soft = true};    
        "i", {longPronounciation = "iː"; shortPronounciation = "ɪ"; soft = true};
        //	The phoneme /ʊ/ is relatively infrequent; short ⟨o⟩ more often represents /ɔ/. 
        // In a few words, long ⟨o⟩ represents /oː/.
        "o", {longPronounciation = "uː"; shortPronounciation = "ɔ"; soft = false};    
        "u", {longPronounciation = "ʉː"; shortPronounciation = "ɵ"; soft = false};
        "y", {longPronounciation = "yː"; shortPronounciation = "ʏ"; soft = true};
        //	Most words with /ɔ/ and some words with /oː/ are spelled with ⟨o⟩.
        "å", {longPronounciation = "oː"; shortPronounciation = "ɔ"; soft = false};    
        //	Some words with /ɛ/ are spelled with ⟨e⟩.
        "ä", {longPronounciation = "ɛː"; shortPronounciation = "ɛ"; soft = true};    
        //	The short ö is, in some dialects, pronounced as /ɵ/.
        "ö", {longPronounciation = "øː"; shortPronounciation = "œ"; soft = true};    
        ]

let toCharacterClass strings =
    "[" + (strings |> Seq.distinct |> Seq.fold (+) "") + "]"

let surroundWithGroup x = "(" + x + ")"

let vowelCharacterClassFilteredBy predicate = 
    vowels  
    |> Seq.map (fun (s, v) -> s, v.soft) 
    |> Seq.filter predicate 
    |> Seq.map fst 
    |> toCharacterClass

let softVowelCharacterClass = vowelCharacterClassFilteredBy (fun (_, soft) -> soft)

/// Consonants can be single character or multi-character
let softableConsonants = [
    // /s/ before soft vowels, ⟨e i y ä ö⟩ otherwise /k/. 
    "c", "s"     
    // 'g' is /j/ before soft vowels ⟨e i y ä ö⟩, otherwise /ɡ/
    "g", "j";
    // 'k' is /ɕ/ before soft vowels ⟨e i y ä ö⟩, otherwise /k/
    "k", "ɕ";  
    // 'sk' is /ɧ/ before soft vowels ⟨e i y ä ö⟩, otherwise /sk/
    "sk", "ɧ"; 
]

let consonants = [
    "b",    "b";
    // /s/ before soft vowels, ⟨e i y ä ö⟩ otherwise /k/. The letter ⟨c⟩ alone 
    // is used only in loanwords (usually in the /s/ value) and proper names, 
    // but ⟨ck⟩ is a normal representation for /k/ after a short vowel (as in 
    // English and German).
    "c",  "k";
    "ck", "k";
    "ch", "ɕ"; 
    // REVISIT: Can't distinguish this case
    // In loanwords. 
    // "ch", "ɧ";   
    // The conjunction 'och' (and) is pronounced /ɔk/ or /ɔ/.
    "och","ɔk"; 
    "d",  "d";
    "dj", "j";
    "f",  "f";
    // 'g' is /j/ before soft vowels ⟨e i y ä ö⟩, otherwise /ɡ/
    "g",  "ɡ";     
    "gj", "j";
    // 'gn' is /ɡn/ word-initially; /ŋn/ elsewhere
    "^gn","ɡn";  
    "gn", "ŋn";
    "h",  "h";
    "hj", "j";
    "j",  "j";
    // 'k' is /ɕ/ before soft vowels ⟨e i y ä ö⟩, otherwise /k/
    "k",  "k";
    "kj", "ɕ";
    "l",  "l";
    "lj", "j";
    "m",  "m";
    "n",  "n";
    "ng", "ŋ";
    // REVISIT: Can't distinguish this case
    // "ng", "ŋɡ";
    "p",  "p";
    "r",  "r";
    "s",  "s";
    "sj", "ɧ";
    // 'sk' is /ɧ/ before soft vowels ⟨e i y ä ö⟩, otherwise /sk/
    "sk", "sk";
    "skj","ɧ";
    "stj","ɧ";
    "t",  "t";
    "tj", "ɕ";
    // Before 1906, ⟨fv, hv⟩ and final ⟨f⟩ were also used for /v/. 
    // Now these spellings are used in some proper names.
    "v",  "v";     
    // 'w' is rarely used (loanwords, proper names). In loanwords from English 
    // may be pronounced /w/.
    "w",  "v";     
    "x",  "ks"; 
    // used in loanwords and proper names.
    "z",  "s";     
]

let singleConsonantCharacterClass = 
    consonants  
    |> Seq.map fst 
    |> Seq.filter (fun s -> s.Length = 1) 
    |> Seq.map string 
    |> toCharacterClass

let doubleConsonant = 
    singleConsonantCharacterClass 
    |> sprintf "(?<dc>%s)\\k<dc>"       // note named backreference

let joinRegexAlternatives (xs:string list) = 
    System.String.Join("|", xs)

let longVowelList = 
    vowels  
    |> Seq.map (fun (s, _) -> s |> surroundWithGroup) 
    |> List.ofSeq

let shortPattern s = (s |> surroundWithGroup) + doubleConsonant

let shortVowelList = 
    vowels  
    |> Seq.map (fun (s, _) -> shortPattern s) 
    |> List.ofSeq

let prepConsonant t =
    t |> fst |> surroundWithGroup 

let consonantList = 
    consonants |> List.map prepConsonant |> List.ofSeq

let followedBySoftVowel cs =
    cs + softVowelCharacterClass

let softableConsonantList = 
    softableConsonants |> List.map prepConsonant |> List.map followedBySoftVowel |> List.ofSeq

let allLookups = List.concat([consonantList; softableConsonantList; shortVowelList; longVowelList])

let makePattern xs =
    xs  
    |> List.sortByDescending String.length
    |> joinRegexAlternatives

let completePattern = makePattern allLookups

let (|RegexMatch|_|) pattern input =
    if input = null || input.Equals "" then None
    else
        let wrappedPattern = sprintf @"(%s){1}(.*)" pattern
        let m = Regex.Match(input, wrappedPattern)
        if m.Success then 
            Some (m.Groups.[1].Value, m.Groups.[m.Groups.Count - 2].Value)
        else None

let vowelCharacterClass = vowelCharacterClassFilteredBy (fun (_, _) -> true)

let vowelFollowedByDoubleConsonantPattern = 
    (vowelCharacterClass |> surroundWithGroup) + doubleConsonant

let consonantFollowedBySoftVowelPattern = 
    (singleConsonantCharacterClass + softVowelCharacterClass) |> surroundWithGroup

let firstChar (s:string) =
    s.ToCharArray().[0]

let translateToIPA word = 
    let rec transUtil word acc = 
        match word with
        | RegexMatch completePattern (chunk, rest) -> 
            let acc = match chunk with            // secondary match
            | RegexMatch vowelFollowedByDoubleConsonantPattern (secondaryMatch, _) -> 
                let chars = secondaryMatch.ToCharArray()
                let shortVowel = chars.[0] |> ShortVowel
                let consonant = chars.[1] |> SingleConsonant
                (consonant::consonant::shortVowel::acc)
            | RegexMatch consonantFollowedBySoftVowelPattern (secondaryMatch, _) ->
                let chars = secondaryMatch.ToCharArray()
                let consonant = chars.[0] |> SoftableConsonant
                let vowel = chars.[1] |> LongVowel
                (vowel::consonant::acc)
            | RegexMatch vowelCharacterClass (secondaryMatch, _) ->
                let vowel = secondaryMatch |> firstChar |> LongVowel
                (vowel::acc)
            | RegexMatch singleConsonantCharacterClass (secondaryMatch, _) ->
                let consonant = secondaryMatch |> firstChar |> SingleConsonant
                (consonant::acc)
            transUtil rest acc
        | "" -> List.rev acc;    
    [] |> transUtil word // |> Seq.fold (+) ""
