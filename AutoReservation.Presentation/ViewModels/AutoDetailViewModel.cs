using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Presentation.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoReservation.Presentation.ViewModels
{
    public class AutoDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private AutoDto _auto;
        public AutoDto Auto { get { return _auto; } set { SetProperty(ref _auto, value); } }
        private string _markeString;
        public string MarkeString { get { return _markeString; } set { SetProperty(ref _markeString, value); } }
        private string _basistarifString;
        public string BasistarifString { get { return _basistarifString; } set { SetProperty(ref _basistarifString, value); } }
        private string _tagestarifString;
        public string TagestarifString { get { return _tagestarifString; } set { SetProperty(ref _tagestarifString, value); } }
        public ICommand SaveAutoCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AutoDetailViewModel()
        {
            Auto = new AutoDto();
            SaveAutoCommand = new RelayCommand<Window>(param => SaveAuto(param));
            CancelCommand = new RelayCommand<Window>(param => Cancel(param));
        }

        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if (Equals(field, value))
            {
                return false;
            }
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            return true;
        }

        private void SaveAuto(Window addAutoWindow)
        {
            List<string> errMsgs = CheckFields();

            if(errMsgs.Count == 0)
            {
                Auto.Marke = MarkeString;
                Auto.Tagestarif = int.Parse(TagestarifString);
                Auto.Basistarif = int.Parse(BasistarifString);

                addAutoWindow.DialogResult = true;
            } else
            {
                MessageBox.Show(String.Join("\n", errMsgs), "Fehler beim Erstellen eines Auto-Eintrags", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void Cancel(Window addAutoWindow)
        {
            addAutoWindow.DialogResult = false;
        }

        private bool IsValidInputNumber(string input)
        {
            return int.TryParse(input, out var num);
        }

        private bool IsInputEmpty(string input)
        {
            return input == null || input.Length == 0;
        }

        private bool IsValidBasistarif()
        {
            if (Auto.AutoKlasse != AutoKlasse.Luxusklasse)
            {
                BasistarifString = "0";
            }

            return IsValidInputNumber(BasistarifString);
        }

        private List<string> CheckFields()
        {
            List<string> errorMsgs = new List<string>();

            if (IsInputEmpty(MarkeString))
            {
                errorMsgs.Add("Das Textfeld \"Marke\" muss ausgefüllt werden!");
            }

            if (!IsValidInputNumber(TagestarifString))
            {
                errorMsgs.Add($"Ungültiger Wert \"{TagestarifString}\" für das Feld \"Tagestarif\"");
            }

            if (!IsValidBasistarif())
            {
                errorMsgs.Add($"Ungültiger Wert \"{BasistarifString}\" für das Feld \"Basistarif\"");
            }

            return errorMsgs;
        }
    }
}
