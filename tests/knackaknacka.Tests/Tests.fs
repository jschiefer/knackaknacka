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
    let pattern = makePattern allLookups
    let blah = ipaTranslate pattern inputString
    ()
//    let count = blah |> Seq.length
//    Assert.AreEqual(2, count)

[<Test>]
let ``at is ɑːt``() =
    let inputString = "at"
    let pattern = makePattern allLookups
    let blah = ipaTranslate pattern inputString
    ()
//    let count = blah |> Seq.length
//    Assert.AreEqual(2, count)

[<Test>]
let ``att is att``() =
    let inputString = "att"
    let pattern = makePattern allLookups
    let blah = ipaTranslate pattern inputString
    ()
//    let count = blah |> Seq.length
//    Assert.AreEqual(2, count)
