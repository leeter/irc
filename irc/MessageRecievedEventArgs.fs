namespace irc.protocol
open System

type public MessageRecievedEventArgs(message:string) = class
    inherit EventArgs()
    member this.Message = message    
end
