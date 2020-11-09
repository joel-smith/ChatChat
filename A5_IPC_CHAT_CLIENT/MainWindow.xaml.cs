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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       // BackgroundWorker listenerWorker;

        TcpClient client = new TcpClient();
        NetworkStream sendStream = null;


        public MainWindow()
        {
            InitializeComponent();
            //this.Dispatcher.BeginInvoke((Action)(() =>
            //{
            //    GetMessages();
            //}));

            //will need a background worker thread started here
            //listenerWorker = new BackgroundWorker();
            //listenerWorker.DoWork += new DoWorkEventHandler(listenerWorker_DoWork);
            //listenerWorker.RunWorkerAsync();
            //listenerWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(listenerWorker_RunWorkerCompleted);
        }

        private void GetMessages()
        {
            while (true)
            {
               if (sendStream != null) 
                { 
                
                sendStream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytes = sendStream.Read(buffer, 0, buffer.Length);
                string message = Encoding.ASCII.GetString(buffer, 0, bytes);

                this.Dispatcher.BeginInvoke((Action)(() =>
             {
                 Output.AppendText(message + Environment.NewLine);
             }));
                
                }
            }
        }

        //private void ThreadProc()
        //{          
        //    if (sendStream != null)
        //    {
        //        NetworkStream sendStream = client.GetStream();
        //        int i;
        //    byte[] buffer = new byte[1024];
        //        int bytes = sendStream.Read(buffer, 0, buffer.Length);
        //        //string tmpString = Encoding.ASCII.GetString(buffer, 0, bytes);
        //        // Loop to receive all the data sent by the client.
        //        while ((i = sendStream.Read(buffer, 0, buffer.Length)) != 0)
        //        {
        //            // Translate data bytes to a ASCII string.
        //            string data = System.Text.Encoding.ASCII.GetString(buffer, 0, i);
        //            Console.WriteLine("Received: {0}", data);
        //            Output.AppendText(data + Environment.NewLine);
        //        }
        //        //this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (ThreadStart)delegate () { Output.AppendText(tmpString + Environment.NewLine); });
        //        //Thread.Sleep(TimeSpan.FromSeconds(1));
        //    }
        //}
        ////to pull messages
        //void listenerWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    //server listening stuff here
        //    //to be used for getting messages
        //    //Server receiveServer = new Server();
        //   // receiveServer.DoServer("127.0.0.1", 13000);
        //   // this.Dispatcher
        //   // Output.AppendText(receiveServer.inMessage); //put the message on new line
        //}
        //void listenerWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    //
        //}
        //sends contents of textbox somewhere
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string messageToSend = Input.Text;
            //Client msgClient = new Client();
            //string serverIP = IP_Field.Text;
            //string messageReceived;
            //add check for server, maybe an ACK/NACK?

            //messageReceived = msgClient.SendMessage(serverIP, 13000, messageToSend);
            Input.Text = String.Empty;
            //call to send here

            
            NetworkStream sendStream = client.GetStream();

            Byte[] data = System.Text.Encoding.ASCII.GetBytes(messageToSend);

            // Send the message to the connected TcpServer. 
            sendStream.Write(data, 0, data.Length);

        }

        //clears textbox
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Input.Text = String.Empty;
        }

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


        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            client.Close();
        }
    }
}
