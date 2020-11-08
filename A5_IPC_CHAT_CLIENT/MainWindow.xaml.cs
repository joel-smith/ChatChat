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

namespace A5_IPC_CHAT_CLIENT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BackgroundWorker listenerWorker;
        public MainWindow()
        {
            InitializeComponent();

            
            //will need a background worker thread started here
            listenerWorker = new BackgroundWorker();
            listenerWorker.DoWork += new DoWorkEventHandler(listenerWorker_DoWork);
            listenerWorker.RunWorkerAsync();
            listenerWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(listenerWorker_RunWorkerCompleted);
        }

        //to pull messages
        void listenerWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //server listening stuff here
            //to be used for getting messages
            Server receiveServer = new Server();

            receiveServer.DoServer("127.0.0.1", 13000);
            Output.AppendText(receiveServer.inMessage); //put the message on new line
        }

        void listenerWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //
        }

        //sends contents of textbox somewhere
        private void Send_Click(object sender, RoutedEventArgs e)
        {
            string messageToSend = Input.Text;
            Client msgClient = new Client();
            string serverIP = IP_Field.Text;
            string messageReceived;
            //add check for server, maybe an ACK/NACK?

            messageReceived = msgClient.SendMessage(serverIP, 13000, messageToSend);
            Input.Text = String.Empty;
            //call to send here

            if (Output.Text == "")
            {
                Output.AppendText(messageReceived); //put the message on new line
            } else
            {
                Output.AppendText("\n" + messageReceived); //put the message on new line
            }

            
        }

        //clears textbox
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            Input.Text = String.Empty;
        }
    }
}
