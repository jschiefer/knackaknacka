namespace System
open System.Reflection

[<assembly: AssemblyTitleAttribute("knackaknacka")>]
[<assembly: AssemblyProductAttribute("knackaknacka")>]
[<assembly: AssemblyDescriptionAttribute("Knock Knock! Who's there? KÖTBULLAR!")>]
[<assembly: AssemblyVersionAttribute("0.1.0")>]
[<assembly: AssemblyFileVersionAttribute("0.1.0")>]
do ()

module internal AssemblyVersionInformation =
    let [<Literal>] Version = "0.1.0"
    let [<Literal>] InformationalVersion = "0.1.0"
