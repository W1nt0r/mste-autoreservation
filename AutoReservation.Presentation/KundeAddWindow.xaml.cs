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
using System.Windows.Shapes;
using AutoReservation.Common.DataTransferObjects;

namespace AutoReservation.Presentation
{
    /// <summary>
    /// Interaction logic for KundeAddWindow.xaml
    /// </summary>
    public partial class KundeAddWindow : Window
    {

        public KundeDto Kunde { get; set; }

        public KundeAddWindow(KundeDto kunde)
        {
            InitializeComponent();
            Kunde = kunde;
            DataContext = this;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
