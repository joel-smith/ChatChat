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

namespace A5_IPC_CHAT_CLIENT
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //to be used for getting messages
            Server receiveServer = new Server();
            //will need a background worker thread started here
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
