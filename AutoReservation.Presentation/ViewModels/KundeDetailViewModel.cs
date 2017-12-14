using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Presentation.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoReservation.Presentation.ViewModels
{
    public class KundeDetailViewModel : BindableBase
    {
        private KundeDto _kunde;
        public KundeDto Kunde { get { return _kunde; } set { SetProperty(ref _kunde, value); } }
        private string _vorname;
        public string Vorname { get { return _vorname; } set { SetProperty(ref _vorname, value); } }
        private string _vornameError;
        public string VornameError { get { return _vornameError; } set { SetProperty(ref _vornameError, value); } }
        private string _nachname;
        public string Nachname { get { return _nachname; } set { SetProperty(ref _nachname, value); } }
        private string _nachnameError;
        public string NachnameError { get { return _nachnameError; } set { SetProperty(ref _nachnameError, value); } }
        private DateTime _geburtsdatum;
        public DateTime Geburtsdatum { get { return _geburtsdatum; } set { SetProperty(ref _geburtsdatum, value); } }
        private string _geburtsdatumError;
        public string GeburtsdatumError { get { return _geburtsdatumError; } set { SetProperty(ref _geburtsdatumError, value); } }
        public ICommand SaveKundeCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public KundeDetailViewModel()
        {
            Kunde = new KundeDto();
            Geburtsdatum = DateTime.Now.Date;
            SaveKundeCommand = new RelayCommand<Window>(param => SaveKunde(param));
            CancelCommand = new RelayCommand<Window>(param => Cancel(param));
        }

        public void SaveKunde(Window window)
        {
            if (!CheckFields())
            {
                Kunde.Geburtsdatum = Geburtsdatum;
                Kunde.Vorname = Vorname;
                Kunde.Nachname = Nachname;

                window.DialogResult = true;
            }
        }

        public void Cancel(Window window)
        {
            window.DialogResult = false;
        }

        private string CheckText(string text, string errorMessage, ref bool errorState)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                errorState = true;
                return errorMessage;
            }
            return default(string);
        }

        private bool CheckFields()
        {
            bool errorState = false;
            VornameError = CheckText(Vorname, "Bitte geben Sie einen Vornamen ein", ref errorState);
            NachnameError = CheckText(Nachname, "Bitte geben Sie einen Nachnamen ein", ref errorState);
            if (Geburtsdatum == default(DateTime) || Geburtsdatum >= DateTime.Now.AddYears(-18))
            {
                GeburtsdatumError = "Bitte geben Sie ein gültiges Geburtsdatum ein";
                errorState = true;
            }
            else
            {
                GeburtsdatumError = null;
            }
            return errorState;
        }

        private bool CheckText(string text)
        {
            return false;
        }
    }
}
