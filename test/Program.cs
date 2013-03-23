using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace test
{
    using System.Net.Security;

    class Program
    {
        static void Main(string[] args)
        {
            var server = new irc.protocol.IrcServer(new List<string>{ "irc.rizon.net"}, 9999, (sender, certificate, chain, errors) => true, (sender, host, certificates, certificate, issuers) => null, EncryptionPolicy.RequireEncryption, Encoding.GetEncoding(65001));
            server.MessageRecievedEvent += new Microsoft.FSharp.Control.FSharpHandler<irc.protocol.MessageRecievedEventArgs>(server_MessageRecievedEvent);
            server.WriteMessage("NICK fSharpIrcTest");
            Console.ReadKey();
        }

        static void server_MessageRecievedEvent(object sender, irc.protocol.MessageRecievedEventArgs args)
        {
            Console.WriteLine(args.Message);
        }
    }
}
