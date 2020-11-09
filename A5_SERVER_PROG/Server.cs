using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Remoting.Messaging;
using System.Runtime.CompilerServices;

namespace A5_SERVER_PROG
{
    public class Server
    {
        //****************************************************************
        //string outMessage; //to be NULL when nothing happening, and if has something, send it
        public string inMessage;

        //keys for all our clients, to be referenced in A5_IPC_CLIENT
        List<TcpClient> clientIDList = new List<TcpClient>();



        //method: doServer
        //description: test connection, returns string back capitalized to uppercase, and thats it
        //public void DoServer(string serverAddress, Int32 serverPort)
        public void DoServer(string serverAddress, Int32 serverPort)
        {
            Console.WriteLine("server 0.1");
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = serverPort;
                IPAddress localAddr = IPAddress.Parse(serverAddress);

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();


                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also user server.AcceptSocket() here.
                    //new client information, accepted here
                    TcpClient client = server.AcceptTcpClient();
                    
                    //add client to list
                    clientIDList.Add(client);


                    Console.WriteLine("Connected!");
                    ParameterizedThreadStart ts = new ParameterizedThreadStart(Worker);
                    Thread clientThread = new Thread(ts);
                    clientThread.Start(client);


                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
                Console.ReadLine();
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
                Console.ReadLine();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        //remove static to store message??
        public void Worker(Object o)
        {
            TcpClient client = (TcpClient)o;
            // Buffer for reading data
            Byte[] bytes = new Byte[256];
            String data = null;

            data = null;

            // Get a stream object for reading and writing
            NetworkStream stream = client.GetStream();
            
            int i;

            

            // Loop to receive all the data sent by the client.
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                // Process the data sent by the client.
                // replace this to make new data from our protocols
                // 
                //data = data.ToUpper();

                //Broadcast(data);

               // byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                // Send back a response.
               // stream.Write(msg, 0, msg.Length);

                
            }

            // Shutdown and end connection
            client.Close();
        }

        public void Broadcast(string message)
        {
            foreach (TcpClient clients in clientIDList)
            {
                NetworkStream clientStream = clients.GetStream();

                string test = "Test Message";

                byte[] msg = null;

                msg = System.Text.Encoding.ASCII.GetBytes(test);

                clientStream.Write(msg, 0, msg.Length);

                Console.WriteLine("Sent: {0}", msg);

                clientStream.Flush();

            }
        }
    }
}


