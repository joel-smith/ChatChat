using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

//whether its client or server initiating it, the exact same code
//is used to send a message, receive, close
//request/response/close

namespace A5_CLIENT_LIB
{
    public class Client
    {

        string clientID;


        //Method: DoClient
        //Description: basic test functionality for client
        public string SendMessage(string serverAddress, Int32 serverPort, string message)
        {
            //Boolean RunIt = true;
            //while (RunIt)
            //{
            //     Console.WriteLine("Please Enter a message to send, or a blank line to end:");
            //    String message = Console.ReadLine();
            string messageReceived;
            if (message != "")
            {
                messageReceived = ConnectClient(serverAddress, 13000, message);
                return messageReceived;

            }
            return "";
            //else
            //{
            //    RunIt = false;
            //}
            // }
        }


        //
        // The following code was extracted from the MSDN site:
        // https://msdn.microsoft.com/en-us/library/system.net.sockets.tcpclient(v=vs.110).aspx
        //
        static string ConnectClient(String server, Int32 serverPort, String message)
        {
            string res;
            try
            {
                // Create a TcpClient.
                // Note, for this client to work you need to have a TcpServer 
                // connected to the same address as specified by the server, port
                // combination.
                Int32 port = serverPort;
                TcpClient client = new TcpClient(server, port);

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();

                NetworkStream stream = client.GetStream();

                

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                Console.WriteLine("Sent: {0}", message);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                data = new Byte[256];

                // String to store the response ASCII representation.
                String responseData = String.Empty;

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
                return res = responseData;
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            return res = "";
        }
    }

    //need to add capabilities for username
    //public static string Connect(String server, String message)
    //{
    //    try
    //    {
    //        //Create a TcpClient
    //        Int32 port = 13000;
    //        TcpClient Client = new TcpClient(server, port);
    //        //get ASCII data into Byte array
    //        Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
    //        //client stream for read/write
    //        NetworkStream stream = Client.GetStream();
    //        //send message to TcpServer
    //        stream.Write(data, 0, data.Length);
    //        Console.WriteLine("Sent: {0}", message);
    //        //Tcp.Server response
    //        data = new byte[256];
    //        //blank string to store the ASCII message
    //        String responseData = String.Empty;
    //        //get first batch of TCP server response bytes
    //        Int32 bytes = stream.Read(data, 0, data.Length);
    //        responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
    //        Console.WriteLine("Recieved {0}", responseData);
    //        //close down
    //        stream.Close();
    //        Client.Close();
    //        return responseData;
    //    }
    //    catch (ArgumentNullException e)
    //    {
    //        Console.WriteLine("ArgumentNullException: {0}", e);
    //    }
    //    catch (SocketException e)
    //    {
    //        Console.WriteLine("SocketException: {0}", e);
    //    }
    //    Console.WriteLine("\n Press Enter to continue...");
    //    Console.Read();
    //}


}
