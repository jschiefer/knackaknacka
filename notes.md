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

Need to figure out the correct backreferences in all cases, particularly in cases of 
"x before y" rules. Named backreferences may be the answer.

Also will need proper backreferences to correctly detect double consonants.


Change before front vowels: c, j, k, sk, 