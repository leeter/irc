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
            var client = new irc.IrcClient();
            var server = client.CreateServer(new List<string> { "irc.rizon.net" }, 25, true, true);
            Console.ReadKey();
        }

        static void server_MessageRecievedEvent(object sender, irc.protocol.MessageRecievedEventArgs args)
        {
            Console.WriteLine(args.Message);
        }
    }
}
