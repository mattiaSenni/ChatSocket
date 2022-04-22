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

namespace ChatSocket
{
    /// <summary>
    /// Logica di interazione per AggiungiContatto.xaml
    /// </summary>
    public partial class AggiungiContatto : Window
    {
        List<Contatto> _contatti;
        Action<List<Contatto>> _callback;
        public AggiungiContatto(List<Contatto> contatti, Action<List<Contatto>> callback, string IP, string porta)
        {
            InitializeComponent();
            _contatti = contatti;
            _callback = callback;
            txtIndirizzoIP.Text = IP;
            txtPorta.Text = porta;
        }

        private void btnNuovoContatto_Click(object sender, RoutedEventArgs e)
        {
            Contatto c = new Contatto()
            {
                Nome = txtNome.Text,
                Port = txtPorta.Text,
                IP = txtIndirizzoIP.Text
            };

            if (_contatti.Contains(c))
            {
                MessageBox.Show("contatto già esistete");
            }
            else
            {
                _contatti.Add(c);
                _callback(_contatti);
                this.Close();
            }
            
        }
    }
}
