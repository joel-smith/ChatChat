/*
*	NAME	      : MainChatWindow.xaml.cs
*	PROJECT		  : Assginemnt 5  PROG2121 
*	PROGRAMMER	  : Joel Smith
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
using A5_CLIENT_LIB;
using System.ComponentModel;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Threading;

namespace A5_IPC_CHAT_CLIENT
{
    /*
    *	NAME	      : ChatMainWindow
    *	PROPOSE		  : ChatMainWindow class has been created for the interaction logic of ChatMainWindow.xaml.
    *	                It has function that handles the event.    
    */
    public partial class ChatMainWindow : Window
    {
        BackgroundWorker listenerWorker;
        string newMessage;


        TcpClient client = new TcpClient();
       
        NetworkStream sendStream = null;

        /* -------------------------------------------------------------------------------------
        *	Name	: ChatMainWindow
        *	Purpose : This function initializes the about window component
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
        *	Inputs	: None
        *	Returns	: None
        *------------------------------------------------------------------------------------ */

        //to be put onto a thread
        private void GetMessages()
        {
            sendStream = client.GetStream();
            byte[] buffer = new byte[1024];


            //add try and catch
            while (true)
            {
               if (sendStream != null) 
                { 
                int bytes = sendStream.Read(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytes);

                    DisplayMessage(message);
                }
            }
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
                      and clears the input textbox when clicked.
        *	Inputs	: object sender : Object of which rasied the event
        *	        : RoutedEventArgs e : Execution of the event
        *	Returns	: None
        *------------------------------------------------------------------------------------ */
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string messageToSend = Input.Text;
            Input.Text = String.Empty;
            
            //call to send here
            NetworkStream sendStream = client.GetStream();

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(messageToSend);

            // Send the message to the connected TcpServer. 
            sendStream.Write(data, 0, data.Length);

        }

        /* -------------------------------------------------------------------------------------
        *	Name	: Send_Click
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

            //Client userClient = new Client();
            client.Connect(serverIP, port);
            
            // Translate the passed message into ASCII and store it as a Byte array.
            Byte[] data = System.Text.Encoding.ASCII.GetBytes("New User");

            // Get a client stream for reading and writing.
            //  Stream stream = client.GetStream();

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
    }
}
