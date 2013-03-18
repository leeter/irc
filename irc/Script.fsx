// This file is a script that can be executed with the F# Interactive.  
// It can be used to explore and test the library project.
// Note that script files will not be part of the project build.

#load "C:\Users\Kantos\Documents\Visual Studio 2010\Projects\irc\irc\Module1.fs"
#load "C:\Users\Kantos\Documents\Visual Studio 2010\Projects\irc\irc\IrcServer.fs"
open Module1
open irc.protocol
open System.Net
let address = [| 127uy; 0uy; 0uy; 1uy |]
let addresso = new IPAddress(address)

let foo = new IrcServer([addresso], 25)

