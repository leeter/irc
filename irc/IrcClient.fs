﻿namespace irc
open System
open System.Collections
open System.Collections.Generic
open System.Text
open System.Net.Security
open System.Security.Cryptography.X509Certificates
open irc.protocol
open irc.framework
open irc.commands
open irc.messages

type public IrcClient() = class
    let servers = new Dictionary<string, IIrcServer>()
    member this.CreateServer(hostNames:IEnumerable<string>) (port:int) (ignoreInvalidCertificate:bool) (forceEncryption:bool) = 
        let rcvc = match ignoreInvalidCertificate with
                   | true -> (fun (s:obj) (c:X509Certificate) (ch:X509Chain) (sslPolicyErrors:SslPolicyErrors) -> true)
                   | _ -> IrcServer.DefaultRemoteCertificateValidationCallback

        let server = new IrcServer(hostNames, port, new RemoteCertificateValidationCallback(rcvc), new LocalCertificateSelectionCallback(IrcServer.DefaultLocalCertificateSelectionCallback), EncryptionPolicy.RequireEncryption, Encoding.GetEncoding(65001)) :> IIrcServer
        servers.Add(server.HostName, server)
        let result = new CommandResult<string>(true, enums.CommandResult.Succeeded, server.HostName)
        result
    member this.ConnectToServerAsClient (command:ConnectToServerAsClientCommand) =
        match servers.ContainsKey(command.ServerId) with
        | true -> servers.[command.ServerId]
                  |> SendPassMessage command.Password
                  |> SendNickMessage command.NickName
                  |> ignore            
        | _ -> ()
end

