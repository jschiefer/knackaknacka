#if INTERACTIVE
#r "bin/Debug/knackaknacka.dll "
#r "../../packages/test/NUnit/lib/nunit.framework.dll"
#else
module knackaknacka.Tests
#endif

open NUnit.Framework
open knackaknacka.NaiveSwedish

[<TestFixtureSetUp>]
let init () =
    ()

[<TestFixtureTearDown>]
let cleanup () =
    ()

[<Test>]
let ``a is ɑː``() =
    let inputString = "a"
    let matches = translateToIPA inputString
    let count = matches |> Seq.length
    Assert.AreEqual(1, count)
    Assert.AreEqual(LongVowel 'a', matches.[0])


[<Test>]
let ``at is ɑːt``() =
    let inputString = "at"
    let matches = translateToIPA inputString
    let count = matches |> Seq.length
    Assert.AreEqual(2, count)
    Assert.AreEqual(LongVowel 'a', matches.[0])
    Assert.AreEqual(SingleConsonant 't', matches.[1])

[<Test>]
let ``atta is attɑː``() =
    let inputString = "atta"
    let matches = translateToIPA inputString
    let count = matches |> Seq.length
    Assert.AreEqual(4, count)
    Assert.AreEqual(ShortVowel 'a', matches.[0])
    Assert.AreEqual(SingleConsonant 't', matches.[1])
    Assert.AreEqual(SingleConsonant 't', matches.[2])
    Assert.AreEqual(LongVowel 'a', matches.[3])
