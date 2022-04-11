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
using System.Net.Sockets;
using System.Net;
using System.Windows.Threading;

namespace ChatSocket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Socket socket = null;
        DispatcherTimer dispatch = null;
        public MainWindow()
        {
            InitializeComponent();

            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            /*
             Addressfamily: dice il tipo di indirizzo di rete -> privato
            SocketType: Dgram per UDP, Stream per TCP
            ProtocolType: il tipo di protocollo da usare -> UDP
             */
            IPAddress local_address = IPAddress.Any;
            IPEndPoint local_endpoint = new IPEndPoint(local_address.MapToIPv4(), 65300);
            /*
             chiedo al S.O. l'indirizzo IP del sistema, poi creo un endpoint sulla porta 65000
            questo è l'endpoint del mittente
             */

            socket.Bind(local_endpoint); //unisce la socket all'endpoint

            lblIP.Content = local_endpoint;

            //// to get user private's ip
            //string IPAddress = "";
            //IPHostEntry Host = default(IPHostEntry);
            //string Hostname = null;
            //Hostname = System.Environment.MachineName;
            //Host = Dns.GetHostEntry(Hostname);
            //foreach (IPAddress IP in Host.AddressList)
            //{
            //    if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            //    {
            //        IPAddress += Convert.ToString(IP) + "\n";
            //    }
            //}

            //lblIP.Content = IPAddress;

        }

        private void btnInvia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                IPAddress remote_address = IPAddress.Parse(txtIndirizzoIP.Text);
                IPEndPoint remote_endpoint = new IPEndPoint(remote_address, int.Parse(txtPorta.Text));

                // converto il messaggio in un array di byte mediante la codifica UTF-8
                byte[] messaggio = Encoding.UTF8.GetBytes(txtMessaggio.Text);

                //invio in maniera asincrona il messaggio al destinatario
                socket.SendTo(messaggio, remote_endpoint);
            }catch(Exception ex)
            {
                MessageBox.Show("Attenzione: qualcosa è andato storto durante l'invio del messaggio");
            }
        }
    }
}
