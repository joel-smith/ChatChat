/*
*	NAME	      : MainChatWindow.xaml.cs
*	PROJECT		  : Assginment 5  PROG2121 
*	PROGRAMMER	  : Joel Smith
*	                Luka Horiuchi
*	LAST VERSION  : 2020-11-09
*	PURPOSE       : This file includes the class and functions for the events occuring in the chat window
*/

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
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;
using System.Net;

namespace A5_IPC_CHAT_CLIENT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class ChatMainWindow : Window
    {
        TcpClient client = new TcpClient();
       
       
        NetworkStream sendStream = null;

        string sessionUser;

        /* -------------------------------------------------------------------------------------
        *	Name	: ChatMainWindow
        *	Purpose : This function initializes the chatMainWindow window component
        *	Inputs	: None
        *	Returns	: None
        *------------------------------------------------------------------------------------ */
        public ChatMainWindow()
        {
            InitializeComponent();
        }


        /* -------------------------------------------------------------------------------------
        *	Name	: GetMessages
        *	Purpose : This function will be used to get messages from the server.
        *	          This will be put onto a thread.
        *	Inputs	: None
        *	Returns	: None
        *------------------------------------------------------------------------------------ */
        private void GetMessages()
        {
            sendStream = client.GetStream();
            byte[] buffer = new byte[1024];


            //add try and catch
            while (client.Connected)
            {
                if (sendStream != null)
                {
                    try
                    { int bytes = sendStream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.ASCII.GetString(buffer, 0, bytes);

                    DisplayMessage(message);
                    } 
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                } 

            }
            DisplayMessage("Server has been disconnected");
        }

        /* -------------------------------------------------------------------------------------
        *	Name	: DisplayMessage
        *	Purpose : This function will be used to print the messages out to the output textbox
        *	Inputs	: None
        *	Returns	: None
        *------------------------------------------------------------------------------------ */
        void DisplayMessage(string message) 
        { 
             this.Dispatcher.BeginInvoke((Action)(() =>
             {
                 Output.AppendText(message + Environment.NewLine);
             }));
        }

        /* -------------------------------------------------------------------------------------
       *	Name	: Send_Click
       *	Purpose : This function will send the textbox inputted strings out to the server 
       *             and clears the input textbox when clicked.
       *	Inputs	: object sender : Object of which rasied the event
       *	        : RoutedEventArgs e : Execution of the event
       *	Returns	: None
       *------------------------------------------------------------------------------------ */
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            //putting the username in front of the input text
            string messageToSend = sessionUser + ":  " + Input.Text;
            Input.Text = String.Empty;

            if (client.Connected)
            {
                NetworkStream sendStream = client.GetStream();
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(messageToSend);

                // Send the message to the connected TcpServer. 
                sendStream.Write(data, 0, data.Length);
            }
            else
            {
                if (Output.Text != "")
                {
                    Output.AppendText("\n");
                }
                Output.AppendText("Server is not connected");
            }

        }

        /* -------------------------------------------------------------------------------------
        *	Name	: Clear_Click
        *	Purpose : This function will clear the input text box when clicked.
        *	Inputs	: object sender : Object of which rasied the event
        *	        : RoutedEventArgs e : Execution of the event
        *	Returns	: None
        *------------------------------------------------------------------------------------ */
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Input.Text = String.Empty;
        }

        /* -------------------------------------------------------------------------------------
        *	Name	: ConnectButton_Click
        *	Purpose : This function will connect to the server to start the chat when clicked.
        *	        : RoutedEventArgs e : Execution of the event
        *	Returns	: None
        *------------------------------------------------------------------------------------ */
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string serverIP = IP_Field.Text;

            int port = 13000;

            try
            {
                //add reference to textbox here
                IPAddress clientIP = IPAddress.Parse(Client_IP.Text);

                IPEndPoint ipLocalEndPoint = new IPEndPoint(clientIP, 13000);

                client = new TcpClient();

                client.Connect(serverIP, port);

                //set the username here
                Set_userName();

                if (Output.Text != "")
                {
                    Output.AppendText("\n");
                }

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes("New User has been entered: " + sessionUser);

                // Get a client stream for reading and writing.
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                byte[] buffer = new byte[1024];
                int bytes = stream.Read(buffer, 0, buffer.Length);

                string tmpString = Encoding.ASCII.GetString(buffer, 0, bytes);

                Output.AppendText(tmpString + Environment.NewLine);

                Thread MessageGetter = new Thread(GetMessages);
                MessageGetter.IsBackground = true;
                MessageGetter.Start();
            }
            //exception 
            catch (FormatException)
            {
                if (Output.Text != "")
                {
                    Output.AppendText("\n");
                }
                Output.AppendText("Please put in the correct IP Address");
            }
            catch (SocketException)
            {
                if (Output.Text != "")
                {
                    Output.AppendText("\n");
                }
                Output.AppendText("Cannot connect to the client!");

            }
        }

        /* -------------------------------------------------------------------------------------
        *	Name	: Window_Closing
        *	Purpose : This function occurs when the windows is closed, disconnect from the client.
        *	Inputs	: object sender : Object of which rasied the event
        *	        : CancelEventArgs e : Execution of the event
        *	Returns	: None
        *------------------------------------------------------------------------------------ */
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            client.Close();
        }

        /* -------------------------------------------------------------------------------------
        *	Name	: Set_userName
        *	Purpose : This function sets the username to the sessionUser variable.
        *	Inputs	: None
        *	Returns	: None
        *------------------------------------------------------------------------------------ */
        private void Set_userName()
        {

            string def = "Enter User Name";

            if (Client_UserName.Text != "")
            {
                if (Client_UserName.Text == def)
                {
                    //if Enter User Name is in the input, it will automatically set as anonymous
                    sessionUser = "Anonymous";
                    Client_UserName.Clear();
                    Client_UserName.AppendText("Anonymous");
                }
                else
                {
                    sessionUser = Client_UserName.Text;
                }
            }
            else
            {
                //if blank input, it will automatically set as anonymous
                sessionUser = "Anonymous";
                Client_UserName.Clear();
                Client_UserName.AppendText("Anonymous");
            }

        }

        /* -------------------------------------------------------------------------------------
        *	Name	: UserName_OnGotFocus
        *	Purpose : Clears the UserName textbox when clicked
        *	Inputs	: None
        *	Returns	: None
        *------------------------------------------------------------------------------------ */
        private void UserName_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Client_UserName.Text = "";
        }

        /* -------------------------------------------------------------------------------------
        *	Name	: Client_IP_OnGotFocus
        *	Purpose : Clears the Client_IP textbox when clicked
        *	Inputs	: None
        *	Returns	: None
        *------------------------------------------------------------------------------------ */
        private void Client_IP_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Client_IP.Text = "";
        }
    }
}
