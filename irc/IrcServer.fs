namespace irc.protocol
open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Net
open System.Net.Sockets
open System.Net.Security
open System.Text
open System.Security.Cryptography.X509Certificates

type Client = { client:TcpClient; hostName:string }
type public IrcServer(serverNames:IEnumerable<string>, port:int, remoteCertificateValidationCallback:RemoteCertificateValidationCallback, userCertificateSelectionCallback:LocalCertificateSelectionCallback, encryptionPolicy:EncryptionPolicy, encoding:Encoding) as this = class
    let TcpConnect (serverName:string):Async<Client option> =
        async{
            try
                let! host = Async.FromBeginEnd(serverName, (fun (name:string, callback:AsyncCallback, obj:Object) -> Dns.BeginGetHostEntry(name, callback, obj)), Dns.EndGetHostEntry)
                let client = new TcpClient()
                do! Async.FromBeginEnd(host.AddressList, port, (fun (x:IPAddress[], y:int, z:AsyncCallback, obj:Object) -> client.BeginConnect(x, y, z, obj)), client.EndConnect)
                return option.Some({client = client; hostName = serverName})
            with
                :? System.Net.Sockets.SocketException -> return None
        }
    let DoConnect(serverNames:seq<string>) = 
        async{                  
            return Seq.pick (fun x ->
                            TcpConnect x
                            |> Async.RunSynchronously) serverNames
        }
    let SslConnect(client:Client) = 
        async{
            let sslStream = new SslStream(client.client.GetStream(), false, remoteCertificateValidationCallback, userCertificateSelectionCallback, encryptionPolicy)
            do! Async.FromBeginEnd(client.hostName, (fun (host:string, z:AsyncCallback, obj:Object) -> sslStream.BeginAuthenticateAsClient(host, z, obj)), sslStream.EndAuthenticateAsClient)
            return sslStream;
        }
    let server =        
        DoConnect serverNames
        |> Async.RunSynchronously
    let stream =         
        SslConnect server
        |> Async.RunSynchronously
    let messageRecievedEvent = new Event<MessageRecievedEventArgs>()
    let writer = new StreamWriter(stream, encoding)
    let reader = new StreamReader(stream, encoding, false, 512)
    let rec StartRecieve() =
        async{ 
            messageRecievedEvent.Trigger(new MessageRecievedEventArgs(reader.ReadLine()))
            do! StartRecieve()
        }
    do Async.Start <| StartRecieve()
    [<CLIEvent>]
    member this.MessageRecievedEvent = messageRecievedEvent.Publish
    member this.WriteMessage(message:string) = 
        writer.WriteLine(message)
    interface IDisposable with
        member this.Dispose() =
            stream.Dispose()
            match server.client with
            | null -> ()
            | _ -> match server.client.Connected with
                    | true -> server.client.Close()
                    | false -> ()
    end
    //member this.SendMessage (message:string) = 
    new (serverName:string, port:int) = 
        new IrcServer(
            [| serverName |],
            port,
            new RemoteCertificateValidationCallback((fun (s:obj) (c:X509Certificate) (ch:X509Chain) (sslPolicyErrors:SslPolicyErrors) -> 
                match sslPolicyErrors with
                | SslPolicyErrors.None -> true
                | _ -> false)),
            new LocalCertificateSelectionCallback((fun (s:obj) (th:string) (lc:X509CertificateCollection) (rc:X509Certificate) (ac:string[]) ->
                match lc with
                | null -> null
                | _ -> match ac with
                       | null -> null
                       | _ -> lc.Cast<X509Certificate>().FirstOrDefault(fun (i:X509Certificate) ->                                                                        
                                                                            Array.exists (fun (a:string) -> a.Equals(i.Issuer, StringComparison.Ordinal)) ac))),
            EncryptionPolicy.RequireEncryption,
            Encoding.GetEncoding(65001))
end
    