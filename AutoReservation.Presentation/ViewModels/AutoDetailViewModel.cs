using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Dal.Entities;
using AutoReservation.Presentation.Commands;
using AutoReservation.Presentation.Delegates;
using AutoReservation.Presentation.Interfaces;
using AutoReservation.Presentation.Services;
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
        public event Result DialogResult;
        private Auto _auto;
        public Auto Auto { get { return _auto; } set { SetProperty(ref _auto, value); } }
        private string _markeString;
        public string MarkeString { get { return _markeString; } set { SetProperty(ref _markeString, value); } }
        private string _basistarifString;
        public string BasistarifString { get { return _basistarifString; } set { SetProperty(ref _basistarifString, value); } }
        private string _tagestarifString;
        public string TagestarifString { get { return _tagestarifString; } set { SetProperty(ref _tagestarifString, value); } }
        private AutoKlasse _autoKlasse;
        public AutoKlasse AutoKlasse {
            get { return _autoKlasse; }
            set {
                ChangeAutoType(value);
                SetProperty(ref _autoKlasse, value);
            }
        }

        private AutoManager autoManager;
        private IMessageDisplayer displayer;

        public ICommand SaveAutoCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AutoDetailViewModel() : this(new MessageBoxDisplayer())
        {
            
        }

        public AutoDetailViewModel(IMessageDisplayer displayer)
        {
            ChangeAutoType(AutoKlasse);
            SaveAutoCommand = new RelayCommand<Window>(param => SaveAuto(param));
            CancelCommand = new RelayCommand<Window>(param => Cancel(param));
            autoManager = new AutoManager();
            this.displayer = displayer;
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

                if(Auto is LuxusklasseAuto)
                {
                    ((LuxusklasseAuto)Auto).Basistarif = int.Parse(BasistarifString);
                }

                try
                {
                    Auto = autoManager.Insert(Auto);

                    DialogResult?.Invoke(true);
                }
                catch (DatabaseChangeException)
                {
                    displayer.DisplayError("Fehler beim Speichern", "Der Eintrag konnte nicht in die Datenbank gespeichert werden!");
                }
            }
            else
            {
                displayer.DisplayWarning("Fehler beim Erstellen eines Auto-Eintrags", String.Join("\n", errMsgs));
            }
        }

        private void Cancel(Window addAutoWindow)
        {
            DialogResult?.Invoke(false);
        }

        private void ChangeAutoType(AutoKlasse autoType)
        {
            switch(autoType)
            {
                case AutoKlasse.Luxusklasse:
                    Auto = new LuxusklasseAuto();
                    break;
                case AutoKlasse.Mittelklasse:
                    Auto = new MittelklasseAuto();
                    break;
                case AutoKlasse.Standard:
                    Auto = new StandardAuto();
                    break;
            }
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
            if (AutoKlasse != AutoKlasse.Luxusklasse)
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
