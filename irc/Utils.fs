// Learn more about F# at http://fsharp.net
module irc.Utils
open System
open System.Text
open System.Globalization
open System.Reflection
open System.Text.RegularExpressions
open irc.messages

[<assembly:AssemblyVersion("1.0.0.0")>]
[<assembly:AssemblyCompany("LeetsoftWerx")>]
[<assembly:AssemblyCopyright("Copyright © LeetSoftwerx 2013")>]
[<assembly:AssemblyDescription("IRC client library")>]
do()

let MakeMessage (text:string) (encoding:Encoding) (maxLength:int) =
    seq{
        match text.Length with
        | x when x <= maxLength -> yield text
        | _ -> yield System.String.Empty;
    }

// ParseRegex parses a regular expression and returns a list of the strings that match each group in 
// the regular expression. 
// List.tail is called to eliminate the first element in the list, which is the full matched expression, 
// since only the matches for each group are wanted. 
let internal (|ParseRegex|_|) regex str =
   let m = Regex(regex).Match(str)
   if m.Success
   then Some (List.tail [ for x in m.Groups -> x.Value ])
   else None

let internal (|Integer|_|) (str: string) =
   let mutable intvalue = 0
   if System.Int32.TryParse(str, &intvalue) then Some(intvalue)
   else None

let internal (|ParseNumericReply|) (reply:string) = 
    match reply with
    | ParseRegex "^:(?<server>[a-zA-Z0-9][a-zA-Z0-9\-]*[a-zA-Z0-9]) (?<reply>[0-9]{3}) (?<resultText>.*)" [a; Integer b; c] -> Some({ prefix = a; reply = b; remainder = c })
    | _ -> None

