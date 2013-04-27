module irc.messages

type internal NumericMessage = { prefix: string; reply:int; remainder:string }

let internal SendPassMessage (password:string) (server:irc.protocol.IIrcServer) =
    match System.String.IsNullOrEmpty password with
    | false -> server.WriteMessage ("PASS " + password)
    | _ -> ()
    server

let internal SendNickMessage (nick:string) (server:irc.protocol.IIrcServer) =
    match System.String.IsNullOrEmpty nick with
    | false -> server.WriteMessage ("NICK " + nick)
    | _ -> ()
    server