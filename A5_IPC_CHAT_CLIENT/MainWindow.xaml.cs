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
    public partial class MainWindow : Window
    {
        TcpClient client = new TcpClient();
       // TcpClient client = new TcpClient();
       
        NetworkStream sendStream = null;

        string sessionUser = "Yoel";

        public MainWindow()
        {
            InitializeComponent();
        }


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
                    try
                    { int bytes = sendStream.Read(buffer, 0, buffer.Length);
                    string message = Encoding.ASCII.GetString(buffer, 0, bytes);

                    DisplayMessage(message);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                    finally
                    {

                    }
                } 

            }
        }

        void DisplayMessage(string message) 
        { 
             this.Dispatcher.BeginInvoke((Action)(() =>
             {
                 Output.AppendText(message + Environment.NewLine);
             }));
        }

        /// <summary>
        /// sends a message through the button and textbox
        /// add username ahead maybe
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string messageToSend = sessionUser + "\n" + Input.Text;
            Input.Text = String.Empty;
            
            NetworkStream sendStream = client.GetStream();

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(messageToSend);

            // Send the message to the connected TcpServer. 
            sendStream.Write(data, 0, data.Length);

        }

        /// <summary>
        /// clears user input textbox textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Input.Text = String.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            string serverIP = IP_Field.Text;

            int port = 13000;

            //add reference to textbox here
            IPAddress clientIP = IPAddress.Parse(Client_IP.Text);

            IPEndPoint ipLocalEndPoint = new IPEndPoint(clientIP, 13000);

            client = new TcpClient();
 
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

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            client.Close();
        }

        private void Client_IP_OnGotFocus(object sender, RoutedEventArgs e)
        {
            Client_IP.Text = "";
        }
    }
}
