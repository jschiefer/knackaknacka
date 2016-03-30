The parts not covered yet:
* g before a consonant that isn't n or a hard vowel
* g before a soft vowel, dj, gj, hj, j, lj
* k before a hard vowel
* k before a soft vowel, ck, kj, tj
* n, ng, g before n, 
* ch, -ge, rs, sch, sh, sj, sk, skj, stj
* handling vowels before single/double consonants
* all the spellings for the Spellings for the sje-phoneme /ɧ/

matchPhoneme should extract the longest possible string that matches to a phoneme, and 
return the phoneme, and the rest of the string.

So for each phoneme, there is one branch

start with a string and an empty list, and end up with a list of strings containing
the individual characters.

Every step: 
- remove first element of list (theString)
- Split the first character off the string
- Add the new string to the beginning of the list
- Add the new character to the end of the list
