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
                if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    Console.WriteLine(address);


                }
            }
            //Console.ReadLine();


            // System.Console.WriteLine(serverAd.ToString());
            //test.DoServer(addresses[0].ToString(), 13000);
            test.DoServer("127.0.1.1", 13000);

            //test.DoServer("192.168.0.17", 13000);





            ////make this into a multithreaded program
            //TcpListener server = null;
            ////always use try
            //try
            //{
            //    //assign each port to a chat room?
            //    Int32 port = 13000;
            //    //eventually add way to change/update this
            //    IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            //    //turn this into a dictionary later
            //    server = new TcpListener(localAddr, port);
            //    server.Start();
            //    //data buffer
            //    Byte[] bytes = new Byte[256];
            //    String data = null;
            //    //listener loop
            //    while (true)
            //    {
            //        Console.WriteLine("awaiting connection");
            //        // Perform a blocking call to accept requests.
            //        // You could also use server.AcceptSocket() here.
            //        TcpClient client = server.AcceptTcpClient();
            //        Console.WriteLine("Connected!");
            //        data = null;
            //        NetworkStream stream = client.GetStream();
            //        int i;
            //        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            //        {
            //            // Translate data bytes to a ASCII string.
            //            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            //            Console.WriteLine("Received: {0}", data);
            //            // Process the data sent by the client.
            //            data = data.ToUpper();
            //            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
            //            // Send back a response.
            //            stream.Write(msg, 0, msg.Length);
            //            Console.WriteLine("Sent: {0}", data);
            //        }
            //        client.Close();
            //    }
            //}
            //catch (SocketException ex)
            //{
            //    Console.WriteLine(ex);
            //}
            //finally
            //{
            //    server.Stop();
            //}   
            //}

        }

    }
}

