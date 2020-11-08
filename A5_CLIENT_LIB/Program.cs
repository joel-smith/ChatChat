using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace A5_CLIENT_LIB
{
    class Program
    {
        static void Main(string[] args)
        {
            Client testClient = new Client();
            testClient.SendMessage("127.0.1.1", 13000, "blahblajh");
            /*testing to get the hostname and host address*/
            //string hostname = System.Net.Dns.GetHostName();
            //Console.WriteLine("Computer name :" + hostname);
            //IPAddress[] addresses = Dns.GetHostAddresses(hostname);
            //Console.WriteLine($"GetHostAddresses({hostname}) returns:");
            ///*showing only the IPv4 address*/
            //foreach (IPAddress address in addresses)
            //{
            //    if (address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //    {
            //        Console.WriteLine(address);

            //    }
            //}
            Console.ReadLine();
        }
    }
}
