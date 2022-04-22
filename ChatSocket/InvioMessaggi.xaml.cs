using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Net;
using System.Xml;


namespace ChatSocket
{
    /// <summary>
    /// Logica di interazione per InvioMessaggi.xaml
    /// </summary>
    public partial class InvioMessaggi : Window
    {
        List<Contatto> contatti;
        Action<List<Contatto>> _callback;
        public InvioMessaggi(List<Contatto> c, Action<List<Contatto>> callback)
        {
            InitializeComponent();
            contatti = c;
            lbAgenda.ItemsSource = contatti;
            _callback = callback;
        }


        private void btnInvia_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPAddress remote_address = IPAddress.Parse(txtIndirizzoIP.Text);
                IPEndPoint remote_endpoint = new IPEndPoint(remote_address, int.Parse(txtPorta.Text));

                // converto il messaggio in un array di byte mediante la codifica UTF-8
                byte[] messaggio = Encoding.UTF8.GetBytes(txtMessaggio.Text);

                //invio in maniera asincrona il messaggio al destinatario
                socket.SendTo(messaggio, remote_endpoint);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Attenzione: qualcosa è andato storto durante l'invio del messaggio");
            }
        }

        

        private void lbAgenda_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbAgenda.SelectedItem != null)
            {
                Contatto selected = (Contatto)lbAgenda.SelectedItem;
                txtIndirizzoIP.Text = selected.IP;
                txtPorta.Text = selected.Port;
            }
        }

        private void btnNuovoContatto_Click(object sender, RoutedEventArgs e)
        {
            AggiungiContatto ac = new AggiungiContatto(contatti, update, txtIndirizzoIP.Text, txtPorta.Text);
            ac.Show();
        }

        private void update(List<Contatto> c)
        {
            contatti = c;
            _callback(contatti);
            lbAgenda.ItemsSource = new List<int>();
            lbAgenda.ItemsSource = contatti;
        }

        private void btnCancella_Click(object sender, RoutedEventArgs e)
        {
            Contatto c = (Contatto)lbAgenda.SelectedItem;
            contatti.Remove(c);
            _callback(contatti);
            lbAgenda.ItemsSource = new List<int>();
            lbAgenda.ItemsSource = contatti;
        }

        private void btnBroadcast_Click(object sender, RoutedEventArgs e)
        {
            foreach(Contatto c in contatti)
            {
                try
                {
                    Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                    IPAddress remote_address = IPAddress.Parse(c.IP);
                    IPEndPoint remote_endpoint = new IPEndPoint(remote_address, int.Parse(c.Port));

                    // converto il messaggio in un array di byte mediante la codifica UTF-8
                    byte[] messaggio = Encoding.UTF8.GetBytes(txtMessaggio.Text);

                    //invio in maniera asincrona il messaggio al destinatario
                    socket.SendTo(messaggio, remote_endpoint);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Attenzione: qualcosa è andato storto durante l'invio del messaggio");
                }
            }
        }

        private void btnDeseleziona_Click(object sender, RoutedEventArgs e)
        {
            lbAgenda.SelectedIndex = -1;
            txtIndirizzoIP.Text = "";
            txtPorta.Text = "";
        }
    }
}
