#if INTERACTIVE
#r "bin/Debug/knackaknacka.dll "
#r "../../packages/test/NUnit/lib/nunit.framework.dll"
#else
module knackaknacka.Tests
#endif

open NUnit.Framework
open knackaknacka.NaiveSwedish

[<Test>]
let ``short a``() =
    let inputString = "att"
    let pattern = "(a)" + followedByDoubleConsonant
    let blah = ipaTranslateWithPattern pattern inputString
    let count = blah |> Seq.length
    Assert.AreEqual(1, count)

[<Test>]
let ``short a - 2``() =
    let inputString = "att"
    let pattern = "(a)" + followedByDoubleConsonant
    let blah = ipaTranslateWithPattern pattern inputString
    let count = blah |> Seq.length
    Assert.AreEqual(1, count)


