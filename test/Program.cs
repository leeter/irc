using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace test
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = new irc.protocol.IrcServer("irc.rizon.net", 9999);
            //var result = server.ConnectAsync("localhost");
            //result.Wait();
            Console.ReadKey();
        }
    }
}
