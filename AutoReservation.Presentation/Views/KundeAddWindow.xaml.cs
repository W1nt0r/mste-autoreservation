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
using AutoReservation.Presentation.ViewModels;

namespace AutoReservation.Presentation
{
    /// <summary>
    /// Interaction logic for KundeAddWindow.xaml
    /// </summary>
    public partial class KundeAddWindow : Window
    {
        public KundeDetailViewModel Kdvm { get; set; }

        public KundeAddWindow()
        {
            InitializeComponent();
            Kdvm = new KundeDetailViewModel();
            DataContext = Kdvm;
            Kdvm.DialogResult += Result;
        }

        private void Result(bool dialogResult)
        {
            DialogResult = dialogResult;
        }
    }
}
