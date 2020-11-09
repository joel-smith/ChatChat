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

            Server test = new Server();

            // string hostName = Dns.GetHostName(); // Retrive the Name of HOST  
            // Console.WriteLine(hostName);
            // // Get the IP  
            // string serverAd = Dns.GetHostEntry(hostName).AddressList[0].ToString();
            // Console.WriteLine("My IP Address is :" + serverAd);

            string hostname = System.Net.Dns.GetHostName();
            Console.WriteLine("Computer name :" + hostname);
            IPAddress[] addresses = Dns.GetHostAddresses(hostname);
            Console.WriteLine($"GetHostAddresses({hostname}) returns:");
            /*showing only the IPv4 address*/
            foreach (IPAddress address in addresses)
            {
                Console.WriteLine(address);

                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    Console.WriteLine(address);


                }
            }
            Console.WriteLine("input server IP Address");
            string inputIP = Console.ReadLine();


           //  System.Console.WriteLine(addresses[1].ToString());
            //test.DoServer(addresses[0].ToString(), 13000);
            test.DoServer(inputIP, 13000);
            //test.DoServer(addresses[1].ToString(), 13000);

            // test.DoServer("192.168.189.81", 13000);




            Console.ReadLine();
           
        }

    }
}

