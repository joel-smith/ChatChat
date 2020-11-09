using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

// Taken from TCPIPExample 3
// The following code is extracted from the MSDN site:
// https://msdn.microsoft.com/en-us/library/system.net.sockets.tcplistener(v=vs.110).aspx
//
//https://docs.microsoft.com/en-us/dotnet/api/system.net.sockets.tcpclient?view=netcore-3.1
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

