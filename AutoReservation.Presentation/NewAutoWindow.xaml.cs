using AutoReservation.Common.DataTransferObjects;
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
    /// Interaction logic for NewAutoWindow.xaml
    /// </summary>
    public partial class NewAutoWindow : Window
    {
        public AutoDto Auto { get; set; }

        public NewAutoWindow()
        {
            Auto = new AutoDto();

            InitializeComponent();

            DataContext = Auto;
        }

        private void AddAutoButton_Click(object sender, RoutedEventArgs e)
        {
            List<string> errMsgs = checkFields();

            Console.WriteLine($"Marke: {Auto.Marke}; Basistarif: {Auto.Basistarif}; Tagestarif: {Auto.Tagestarif}; Autoklasse: {Auto.AutoKlasse}");

            if(errMsgs.Count == 0)
            {
                DialogResult = true;
            } else
            {
                CustomDialog errorDialog = new CustomDialog("Fehler beim Erstellen eines Auto-Eintrags", String.Join("\n", errMsgs));
                errorDialog.ShowDialog();
            }
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BasistarifTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(!IsValidInputNumber(e.Text))
            {
                e.Handled = true;
            }
        }

        private void TagestarifTextbox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!IsValidInputNumber(e.Text))
            {
                e.Handled = true;
            }
        }

        private bool IsValidInputNumber(string input)
        {
            return int.TryParse(input, out var num);
        }

        private bool IsInputEmpty(string input)
        {
            return input.Length == 0;
        }

        private bool IsValidTagestarif()
        {
            if(Auto.AutoKlasse != AutoKlasse.Luxusklasse)
            {
                TagestarifTextbox.Text = "0";
                Auto.Tagestarif = 0;
            }

            return IsValidInputNumber(TagestarifTextbox.Text);
        }

        private List<string> checkFields()
        {
            List<string> errorMsgs = new List<string>();

            if(IsInputEmpty(MarkeTextbox.Text))
            {
                errorMsgs.Add("Das Textfeld \"Marke\" muss ausgefüllt werden!");
            }

            if(!IsValidInputNumber(BasistarifTextbox.Text))
            {
                errorMsgs.Add($"Ungültiger Wert \"{BasistarifTextbox.Text}\" für das Feld \"Basistarif\"");
            }

            if (!IsValidTagestarif())
            {
                errorMsgs.Add($"Ungültiger Wert \"{TagestarifTextbox.Text}\" für das Feld \"Tagestarif\"");
            }

            return errorMsgs;
        }
    }
}
