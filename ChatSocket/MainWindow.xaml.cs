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
using System.Threading;
using System.Xml.Serialization;
using System.IO;

namespace ChatSocket
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Socket socket = null;
        DispatcherTimer dTimer = null;
        int WELL_KNOWN_PORT = 65000;
        List<Contatto> contatti;
        public MainWindow()
        {
            InitializeComponent();
            Deserialize();


            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            /*
             Addressfamily: dice il tipo di indirizzo di rete -> privato
            SocketType: Dgram per UDP, Stream per TCP
            ProtocolType: il tipo di protocollo da usare -> UDP
             */

            lblPorta.Content = lblPorta.Content += " " + WELL_KNOWN_PORT;

            //// to get user private's ip
            string ipa = "";
            IPHostEntry Host = default(IPHostEntry);
            string Hostname = null;
            Hostname = System.Environment.MachineName;
            Host = Dns.GetHostEntry(Hostname);
            foreach (IPAddress IP in Host.AddressList)
            {
                if (IP.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    ipa += Convert.ToString(IP) + "\n";
                }
            }

            lblIP.Content = ipa;
            Thread listen = new Thread(new ThreadStart(socketInListen));
            listen.Start();
        }

        private void update(List<Contatto> c)
        {
            contatti = c;
            Serialize();
        }

        private void Serialize()
        {
            XmlSerializer xmls = new XmlSerializer(typeof(List<Contatto>));
            TextWriter tw = new StreamWriter("agenda.xml");
            xmls.Serialize(tw, contatti);
            tw.Close();
        }

        private void Deserialize()
        {
            XmlSerializer xmls = new XmlSerializer(typeof(List<Contatto>));
            TextReader tr = new StreamReader("agenda.xml");
            contatti = (List<Contatto>)xmls.Deserialize(tr);
            tr.Close();
        }

        private void socketInListen()
        {
            IPAddress local_address = IPAddress.Any;
            IPEndPoint local_endpoint = new IPEndPoint(local_address.MapToIPv4(), WELL_KNOWN_PORT);
            /*
             chiedo al S.O. l'indirizzo IP del sistema, poi creo un endpoint sulla porta 65000
            questo è l'endpoint del mittente
             */

            

            socket.Bind(local_endpoint); //unisce la socket all'endpoint

            while (true)
            {
                aggiornamento_dTimer();
            }


            
        }

        private void aggiornamento_dTimer()
        {
            int nBytes = 0;
            if((nBytes = socket.Available) > 0)
            {
                //ricezione dei caratteri in attesa
                byte[] buffer = new byte[nBytes];

                // ancora non so chi mi manda i dati
                EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);

                // riceve i dati della socket, inoltre scrive i dati del mittente nell'endpoint
                nBytes = socket.ReceiveFrom(buffer, ref remoteEndPoint);

                string from = ((IPEndPoint)remoteEndPoint).Address.ToString();
                string messaggio = Encoding.UTF8.GetString(buffer, 0, nBytes);
                string name = from;
                for (int i = 0; i < contatti.Count(); i++)
                {
                    if (contatti[i].IP == from)
                    {
                        name = contatti[i].Nome;
                    }
                }

                Dispatcher.BeginInvoke(new Action(() =>
                {
                    lstChat.Items.Add(name + ": " + messaggio);
                }));
            }
        }

        
        private void btnSendMessage_Click(object sender, RoutedEventArgs e)
        {
            InvioMessaggi im = new InvioMessaggi(contatti, update);
            im.Show();
        }
    }
}
