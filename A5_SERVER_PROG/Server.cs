//    FILE                   : Server.cs
//    PROJECT                : ChatChat v1.1 
//    PROGRAMMER             : Joel Smith
//                             Luka Horiuchi
//    LAST VERSION           : 2020 - 11 - 09
//    DESCRIPTION: contains logic for chat server

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

    /*
    *	NAME	      : Server
    *	PROPOSE		  : ChatMainWindow class has been created for the interaction logic of ChatMainWindow.xaml.
    *	                It has function that handles the event.    
    */
    public class Server
    {
        //****************************************************************
        
        public string inMessage;

        //keys for all our clients, to be referenced in A5_IPC_CLIENT
        List<TcpClient> clientIDList = new List<TcpClient>();



        /* -------------------------------------------------------------------------------------
        *	Name	: DoServer
        *	Purpose : This function initializes the connection, listen to the client connection
        *	Inputs	: String serverAddress : IP address of the server
        *	          Int 32               : server port number
        *	Returns	: None
        *------------------------------------------------------------------------------------ */
        public void DoServer(string serverAddress, Int32 serverPort)
        {
            Console.WriteLine("ChatChat - server 1.0");
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 13000.
                Int32 port = serverPort;
                IPAddress localAddr = IPAddress.Parse(serverAddress);

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

                    //setup and start the thread for recieving the message from the client
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

        /* -------------------------------------------------------------------------------------
        *	Name	: Worker
        *	Purpose : This function wil recieve all data sent by the client and response every
        *	          message back to the client. It will be used through thread.
        *	Inputs	: Obejct o : client
        *	Returns	: None
        *------------------------------------------------------------------------------------ */
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
            while (client.Connected)
            {
                try
                {
                    if ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);

                        Broadcast(data);
                    }
                }
                catch (IOException)
                {
                    Console.WriteLine("Cannot read the message from the client....");
                }
            }

            // Shutdown and end connection
            client.Close();
        }

        /* -------------------------------------------------------------------------------------
        *	Name	: Broadcast
        *	Purpose : This function will send back the message recieved from clients to each clients.
        *	          If the client is gone, it will prompt that to all other users too.
        *	          Used inside the Worker() function.
        *	Inputs	: string message : message
        *	Returns	: None
        *------------------------------------------------------------------------------------ */
        public void Broadcast(string message)
        {
            for (int i = 0; i < clientIDList.Count; i++)
            {
                try { 
                    NetworkStream clientStream = clientIDList[i].GetStream();

                    byte[] msg = null;

                    msg = System.Text.Encoding.ASCII.GetBytes(message);

                    clientStream.Write(msg, 0, msg.Length);

                    Console.WriteLine("Sent: {0}", message);

                    clientStream.Flush();
                    }
                    catch (ObjectDisposedException)
                    {
                        Console.WriteLine("Oops, that client is gone, deleting entry");
                        clientIDList.RemoveRange(i, 1); //delete that entry
                        Broadcast("SYSTEM: Offline User Removed from Broadcast"); //notify current users, that user has left
                    }  
            }
        }
    }
}


