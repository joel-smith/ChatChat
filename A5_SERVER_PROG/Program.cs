//    FILE                   : Program.cs
//    PROJECT                : ChatChat v1.1 
//    PROGRAMMER             : Joel Smith
//                             Luka Horiuchi
//    LAST VERSION           : 2020 - 11 - 09
//    DESCRIPTION: starts server from IP address inputted thru command line.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;


namespace A5_SERVER_PROG { 
    class Program
    {
        static void Main(string[] args)
        {
            Server runServer = new Server();

            Console.WriteLine("************** CHATCHAT SERVER **************");
            Console.WriteLine("Input server IP Address");
            string inputIP = Console.ReadLine();
            runServer.DoServer(inputIP, 13000);

            Console.ReadLine();
           
        }

    }
}

