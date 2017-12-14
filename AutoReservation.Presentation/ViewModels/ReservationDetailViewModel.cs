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
using AutoReservation.Service.Wcf;

namespace AutoReservation.Presentation.ViewModels
{
    public class ReservationDetailViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ReservationDto _reservation;
        public ReservationDto Reservation { get { return _reservation; } set { SetProperty(ref _reservation, value); } }
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

        public ReservationDetailViewModel()
        {
            Von = DateTime.Now;
            Bis = DateTime.Now.AddDays(1);
            
            Autos = new ObservableCollection<Auto>();
            foreach (Auto a in new AutoManager().List){
                Autos.Add(a);
            }
            AutoId = 1;
            Kunden = new ObservableCollection<Kunde>();
            foreach(Kunde k in new KundeManager().List)
            {
                Kunden.Add(k);
            }
            KundeId = 1;
            SaveReservationCommand = new RelayCommand<Window>(param => SaveReservation(param));
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

        private void SaveReservation(Window addReservationWindow)
        {
           if(CheckFields().Count == 0)
            {
                Reservation = new ReservationDto();
                Reservation.Auto = Autos[AutoId-1].ConvertToDto();
                Reservation.Kunde = Kunden[KundeId-1].ConvertToDto();
                Reservation.Von = Von;
                Reservation.Bis = Bis;
                if (new ReservationManager().IsAutoAvailable(Reservation.ConvertToEntity()))
                {
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
            if (Von < DateTime.Now || Bis < DateTime.Now)
            {
                return false;
            }
            if(Von.AddHours(24) > Bis)
            {
                return false;
            }
            
            return true;
        }


        private List<string> CheckFields()
        {
            List<String> errMsgs = new List<string>();
            if (!DatesAreValid())
            {
                errMsgs.Add("Die Datumsrange ist nicht gültig");
            }
            return errMsgs;
        }
    }
}
