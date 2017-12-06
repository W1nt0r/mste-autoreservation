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

namespace AutoReservation.Presentation
{
    /// <summary>
    /// Interaction logic for CustomDialog.xaml
    /// </summary>
    public partial class CustomDialog : Window
    {
        public CustomDialog(string title, string message)
        {
            Title = title;

            InitializeComponent();

            Message.Text = message;
        }

        private void ConfirmClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void AbortClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
