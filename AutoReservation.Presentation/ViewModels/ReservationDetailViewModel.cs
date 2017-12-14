using AutoReservation.BusinessLayer;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Dal.Entities;
using AutoReservation.Presentation.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Linq;
using AutoReservation.Presentation.Views;
using AutoReservation.Presentation.Interfaces;
using AutoReservation.Presentation.Services;

namespace AutoReservation.Presentation.ViewModels
{
    public class ReservationDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private Reservation _reservation;
        public Reservation Reservation { get { return _reservation; } set { SetProperty(ref _reservation, value); } }
        private DateTime _von;
        public DateTime Von { get { return _von; } set { SetProperty(ref _von, value); } }
        private DateTime _bis;
        public DateTime Bis { get { return _bis; } set { SetProperty(ref _bis, value); } }
        private int _kundeId;
        public int KundeId { get { return _kundeId; } set { SetProperty(ref _kundeId, value); } }
        private int _autoId;
        public int AutoId { get { return _autoId; } set { SetProperty(ref _autoId, value); } }
        public ICommand SaveReservationCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        private ObservableCollection<Auto> _autos;
        public ObservableCollection<Auto> Autos { get { return _autos; } set { SetProperty(ref _autos, value); } }
        private ObservableCollection<Kunde> _kunden;
        public ObservableCollection<Kunde> Kunden { get { return _kunden; } set { SetProperty(ref _kunden, value); } }

        private IMessageDisplayer displayer;

        public ReservationDetailViewModel(IMessageDisplayer displayer)
        {
            this.displayer = displayer;

            Von = DateTime.Now;
            Bis = DateTime.Now.AddDays(1);

            Autos = new ObservableCollection<Auto>();
            foreach (Auto a in new AutoManager().List)
            {
                Autos.Add(a);
            }
            Kunden = new ObservableCollection<Kunde>();
            foreach (Kunde k in new KundeManager().List)
            {
                Kunden.Add(k);
            }

            if (Autos.Count == 0 || Kunden.Count == 0)
            {
                //MessageBox.Show("Achtung: Sie müssen zuerst Autos und Kunden registrieren, bevor Sie Reservationen anlegen können");
                displayer.DisplayWarning("Achtung", "Sie müssen zuerst Autos und Kunden registrieren, bevor Sie Reservationen anlegen können");
                Application.Current.Windows.OfType<ReservationAddWindow>().First().Loaded += (s, e) => Application.Current.Windows.OfType<ReservationAddWindow>().First().DialogResult = false;
            }
            KundeId = 1;
            SaveReservationCommand = new RelayCommand<Window>(param => SaveReservation(param));
            CancelCommand = new RelayCommand<Window>(param => Cancel(param));
        }

        public ReservationDetailViewModel() : this(new MessageBoxDisplayer())
        {
           
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

        private void SaveReservation(Window addReservationWindow)
        {
            AutoId = 10999;
           if(CheckFields().Count == 0)
            {
                Reservation = new Reservation();
                Reservation.AutoId = AutoId;
                Reservation.KundeId = KundeId;
                Reservation.Von = Von;
                Reservation.Bis = Bis;
                if (new ReservationManager().IsAutoAvailable(Reservation))
                {
                    new ReservationManager().Insert(Reservation);
                    addReservationWindow.DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Fehler: Dieses Auto ist in diesem Zeitraum leider nicht verfügbar.");
                }
            }
            else
            {
                MessageBox.Show("Sorry. Fählerhafti igab");
            }
        }

        private void Cancel(Window addReservationWindow)
        {
            addReservationWindow.DialogResult = false;
        }

        private bool DatesAreValid()
        {
           
            if(Von.AddHours(24) > Bis)
            {
                return false;
            }
            
            return true;
        }

        private bool KundeIsValid()
        {
            return (Kunden.Any(x=>x.Id ==KundeId));
        }

        private bool AutoIsValid()
        {
            return (Autos.Any(x=>x.Id==AutoId));
        }

        private List<string> CheckFields()
        {
            List<String> errMsgs = new List<string>();
            if (!DatesAreValid())
            {
                errMsgs.Add("Der Datumsrange ist nicht gültig");
            }
            if (!KundeIsValid())
            {
                errMsgs.Add("Bitte wähle einen Kunden aus");
            }
            if (!AutoIsValid())
            {
                errMsgs.Add("Bitte wähle ein Auto");
            }
            
            return errMsgs;
        }
    }
}
