using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace A5_SERVER_PROG
{
    public class Server
    {
        //****************************************************************
        //string outMessage; //to be NULL when nothing happening, and if has something, send it
        //string inMessage;


        byte[] bytes = new Byte[1024];
        //keys for all our clients, to be referenced in A5_IPC_CLIENT
        List<TcpClient> clientIDList = new List<TcpClient>();

        public void Init_Server()
        {
            // Establish the local endpoint for the socket.  
            // Dns.GetHostName returns the name of the
            // host running the application.  
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 13000); //?????????

            // TcpListener server = new TcpListener(port);??????????
            TcpListener server = new TcpListener(ipAddress, 13000);
            //for server init
            server.Start();
            Thread connection = new Thread(new ThreadStart(Connect_Await_Thread));


        }

       //trying to make the thread stuff for waiting
        private void Connect_Await_Thread()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 13000);
            listener.Start();
                while (true)
                {
                if (listener.Pending() == true)
                {

                    TcpClient tcp = listener.AcceptTcpClient();
                    Console.WriteLine("{0}: Connected!", tcp.Client.RemoteEndPoint);
                    clientIDList.Add(tcp);
                    //clientIDList.Add(IPAddress.Parse((tcp.Client.RemoteEndPoint).ToString()));
                }
                else
                {
                    Thread.Sleep(0);
                }

                }
            //listener.Stop();

        }
        //how to get the threads working for each client????
        //trying to read/write from/to the client using thread...
        private void Read_Write_Thread()
        {
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

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                Console.WriteLine("Sent: {0}", data);
            }


        }



        //method: doServer
        //description: test connection, returns string back capitalized to uppercase, and thats it
        //public void DoServer(string serverAddress, Int32 serverPort)
        public void DoServer()
        {
            Console.WriteLine("server 0.1");
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = 13000;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

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
                    Console.WriteLine(client + "Connected!");
                    ParameterizedThreadStart ts = new ParameterizedThreadStart(Worker);
                    Thread clientThread = new Thread(ts);
                    clientThread.Start(client);


                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");
            Console.Read();
        }

        public static void Worker(Object o)
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

                byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                // Send back a response.
                stream.Write(msg, 0, msg.Length);

                Console.WriteLine("Sent: {0}", data);
            }

            // Shutdown and end connection
            client.Close();
        }
    }










}
//        //****************************************************************
//        //string outMessage; //to be NULL when nothing happening, and if has something, send it
//        //string inMessage;

//        //keys for all our clients, to be referenced in A5_IPC_CLIENT
//        //List<string> clientIDList = new List<string>();

//        public static string data = null;


//        //method: doServer
//        //description: test connection, returns string back capitalized to uppercase, and thats it
//        //public void DoServer(string serverAddress, Int32 serverPort)
//        public void DoServer()
//        {
//            // Data buffer for incoming data.  
//            byte[] bytes = new Byte[1024];

//            // Establish the local endpoint for the socket.  
//            // Dns.GetHostName returns the name of the
//            // host running the application.  
//            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
//            IPAddress ipAddress = ipHostInfo.AddressList[0];
//            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 13000);

//            // Create a TCP/IP socket.  
//            Socket listener = new Socket(ipAddress.AddressFamily,
//                SocketType.Stream, ProtocolType.Tcp);

//            try
//            {
//                listener.Bind(localEndPoint);
//                listener.Listen(10);

//                // Start listening for connections.  
//                while (true)
//                {
//                    Console.WriteLine("Waiting for a connection...");
//                    // Program is suspended while waiting for an incoming connection.  
//                    Socket handler = listener.Accept();
//                    //client list 
//                    Console.WriteLine("Connected");
//                    data = null;

//                    // An incoming connection needs to be processed.  
//                    while (true)
//                    {
                        
//                        int bytesRec = handler.Receive(bytes);
//                        data += Encoding.ASCII.GetString(bytes, 0, bytesRec);
//                            break;
//                    }

//                    // Show the data on the console.  
//                    Console.WriteLine("Text received : {0}", data);

//                    // Echo the data back to the client.  
//                    byte[] msg = Encoding.ASCII.GetBytes(data);

//                    handler.Send(msg);
//                    handler.Shutdown(SocketShutdown.Both);
//                    handler.Close();
//                }

//            }
//            catch (Exception e)
//            {
//                Console.WriteLine(e.ToString());
//            }

//            Console.WriteLine("\nPress ENTER to continue...");
//            Console.Read();
//        }

//        }
//    }

    
////            try
////            {
////                // Set the TcpListener on port 13000.
////                Int32 port = serverPort;
////    IPAddress localAddr = IPAddress.Parse(serverAddress);

////    // TcpListener server = new TcpListener(port);
////    server = new TcpListener(localAddr, port);

////    // Start listening for client requests.
////    server.Start();


////                // Enter the listening loop.
////                while (true)
////                {
////                    Console.Write("Waiting for a connection... ");

////                    // Perform a blocking call to accept requests.
////                    // You could also user server.AcceptSocket() here.
////                    //new client information, accepted here
////                    TcpClient client = server.AcceptTcpClient();
////    Console.WriteLine("Connected!");
////                    ParameterizedThreadStart ts = new ParameterizedThreadStart(Worker);
////    Thread clientThread = new Thread(ts);
////    clientThread.Start(client);


////                }
////            }
////            catch (SocketException e)
////{
////    Console.WriteLine("SocketException: {0}", e);
////}
////finally
////{
////    // Stop listening for new clients.
////    server.Stop();
////}


////Console.WriteLine("\nHit enter to continue...");
////Console.Read();
////        }

////        public static void Worker(Object o)
////{
////    TcpClient client = (TcpClient)o;
////    // Buffer for reading data
////    Byte[] bytes = new Byte[256];
////    String data = null;

////    data = null;

////    // Get a stream object for reading and writing
////    NetworkStream stream = client.GetStream();

////    int i;

////    // Loop to receive all the data sent by the client.
////    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
////    {
////        // Translate data bytes to a ASCII string.
////        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
////        Console.WriteLine("Received: {0}", data);

////        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

////        // Send back a response.
////        stream.Write(msg, 0, msg.Length);

////        Console.WriteLine("Sent: {0}", data);
////    }

////    // Shutdown and end connection
////    client.Close();
////}
