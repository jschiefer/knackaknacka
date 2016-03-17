module knackaknacka.Tests

open NUnit.Framework
open knackaknacka.NaiveSwedish

[<Test>]
let ``c followed by hardvowel``() =
    let x = tokenize "CA"
    Assert.NotNull(x)
    let count = x |> Seq.length
    Assert.AreEqual(2, count)
