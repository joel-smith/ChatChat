using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using A5_SERVER_PROG;
//using A5_CLIENT_LIB;
using System.Net.Sockets;
using System.Threading;
using System.Net;
using System.Runtime.InteropServices;

namespace A5_IPC_CHAT_CLIENT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        byte[] bytes = new byte[1024];
        Socket senders;
        public MainWindow()
        {
            InitializeComponent();

        }

        //sends contents of textbox somewhere
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string messageToSend = Input.Text;
            
            string serverIP = IP_Field.Text;
            string messageReceived;
            //add check for server, maybe an ACK/NACK?

            messageReceived = msgClient.SendMessage(serverIP, 13000, messageToSend);
            Input.Text = String.Empty;
            //call to send here

            if (Output.Text == "")
            {
                Output.AppendText(messageReceived); //put the message on new line
            }
            else
            {
                Output.AppendText("\n" + messageReceived); //put the message on new line
            }


        }

        public void Server_Connect(object sender, RoutedEventArgs e)
        {
            try
            {
                // Establish the remote endpoint for the socket.  
                // This example uses port 11000 on the local computer.  
                IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];
                IPEndPoint remoteEP = new IPEndPoint(ipAddress, 13000);

                // Create a TCP/IP  socket.  
                senders = new Socket(ipAddress.AddressFamily,
                    SocketType.Stream, ProtocolType.Tcp);
                try 
                { 
                    // Connect the socket to the remote endpoint. Catch any errors.
                    senders.Connect(remoteEP);
                }
                catch (ArgumentNullException ane)
                {
                    Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
                }
                catch (SocketException se)
                {
                    Console.WriteLine("SocketException : {0}", se.ToString());
                }
                catch (Exception es)
                {
                    Console.WriteLine("Unexpected exception : {0}", es.ToString());
                }
            }
            catch (Exception er)
            {
                Console.WriteLine(er.ToString());
            }


        }
        //clears textbox
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Input.Text = String.Empty;
        }

        ////client connection a wait thread...
        ////http://nonsoft.la.coocan.jp/SoftSample/CS.NET/SampleTcpIpSvr.html
        //private void ServerThread() 
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            //connecting to the client
        //            TcpClient myTcpClient = listener.AcceptTcpClient();
        //            Console.WriteLine(myTcpClient);

        //            if (aClient == null)
        //            {
        //                break;
        //            }
        //            else if (aClient.Connected == false)
        //            {
        //                break;
        //            }
        //            else if (aClient.Connected == true)
        //            {
        //                NetworkStream stream = aClient.GetStream();
        //            }

        //        }
        //    }
        //    catch
        //    { 
        //    }

        //}

        //public void recieve_message()
        //{
        //    NetworkStream stream = aClient.GetStream();

        //}
    }
}

//private void Send_Click(object sender, RoutedEventArgs e)
//{
//    string messageToSend = Input.Text;
//    Client msgClient = new Client();
//    string serverIP = IP_Field.Text;
//    string messageReceived;
//    //add check for server, maybe an ACK/NACK?

//    messageReceived = msgClient.SendMessage(serverIP, 13000, messageToSend);
//    Input.Text = String.Empty;
//    //call to send here

//    if (Output.Text == "")
//    {
//        Output.AppendText(messageReceived); //put the message on new line
//    }
//    else
//    {
//        Output.AppendText("\n" + messageReceived); //put the message on new line
//    }


//}




//try
//{
//    // Establish the remote endpoint for the socket.  
//    // This example uses port 11000 on the local computer.  
//    IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
//    IPAddress ipAddress = ipHostInfo.AddressList[0];
//    IPEndPoint remoteEP = new IPEndPoint(ipAddress, 13000);

//    // Create a TCP/IP  socket.  
//    senders = new Socket(ipAddress.AddressFamily,
//        SocketType.Stream, ProtocolType.Tcp);
//    try
//    {
//        // Connect the socket to the remote endpoint. Catch any errors.
//        senders.Connect(remoteEP);
//    }
//    catch (ArgumentNullException ane)
//    {
//        Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
//    }
//    catch (SocketException se)
//    {
//        Console.WriteLine("SocketException : {0}", se.ToString());
//    }
//    catch (Exception es)
//    {
//        Console.WriteLine("Unexpected exception : {0}", es.ToString());
//    }
//}
//catch (Exception er)
//{
//    Console.WriteLine(er.ToString());
//}








//string messageToSend = Input.Text;
////string serverIP = IP_Field.Text;
////add check for server, maybe an ACK/NACK?

//// Translate the passed message into ASCII and store it as a Byte array.
//Byte[] data = System.Text.Encoding.ASCII.GetBytes(messageToSend);

////Send the data throught the socket
//int bytesSent = senders.Send(data);

//// Receive the response from the remote device
//int bytesRec = senders.Receive(bytes);

//if (Output.Text == "")
//{
//    Output.AppendText(Encoding.ASCII.GetString(bytes, 0, bytesRec)); //put the message on new line
//}
//else
//{
//    Output.AppendText("\n" + Encoding.ASCII.GetString(bytes, 0, bytesRec)); //put the message on new line
//}

//Input.Text = String.Empty;
////call to send here