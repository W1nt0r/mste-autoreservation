using AutoReservation.Presentation.ViewModels;
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

namespace AutoReservation.Presentation.Views
{
    /// <summary>
    /// Interaction logic for AutoAddWindow.xaml
    /// </summary>
    public partial class AutoAddWindow : Window
    {
        public AutoDetailViewModel Advm { get; set; }

        public AutoAddWindow()
        {
            InitializeComponent();

            Advm = new AutoDetailViewModel();
            Advm.DialogResult += Result;
            DataContext = Advm;
        }

        private void Result(bool result)
        {
            DialogResult = result;
        }
    }
}
