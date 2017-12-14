using AutoReservation.BusinessLayer;
using AutoReservation.Dal.Entities;
using AutoReservation.Presentation.Commands;
using AutoReservation.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;

namespace AutoReservation.Presentation.ViewModels
{
    public class ReservationViewModel : INotifyPropertyChanged
    {
        private ReservationManager resManager;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool? _hidden;
        public bool? Hidden { get { return _hidden; } set { SetProperty(ref _hidden, value); StartUpdate(null, null); } }
        public ICommand AddReservationCommand { get; set; }
        public ICommand RemoveReservationCommand { get; set; }
        public ICommand FilterReservationsCommand { get; set; }
        private Timer timer;

        private ObservableCollection<Reservation> _reservationen;
        public ObservableCollection<Reservation> Reservationen
        {
            get
            {
                if ((bool)Hidden && _reservationen != null)
                {
                    ObservableCollection<Reservation> res = new ObservableCollection<Reservation>();
                    foreach (Reservation r in _reservationen)
                    {
                        if (r.Bis > DateTime.Now)
                        {
                            res.Add(r);
                        }
                    }
                    return res;
                }
                else
                {
                    return _reservationen;
                }
            }
            set { SetProperty(ref _reservationen, value); }
        }

        public ReservationViewModel()
        {
            resManager = new ReservationManager();
            Hidden = true;
            RemoveReservationCommand = new RelayCommand<Reservation>(param => DeleteReservation(param));
            AddReservationCommand = new RelayCommand<Reservation>(param => SaveReservation(param));
            timer = new Timer(1000);
            timer.Elapsed += StartUpdate;
            timer.Start();
        }
        private void StartUpdate(Object source, ElapsedEventArgs e)
        {
            Task.Run(() =>
            {
                List<Reservation> updatedData = resManager.List;
                updatedData.Sort((x, y) => x.Von.CompareTo(y.Von));

                ObservableCollection<Reservation> res = new ObservableCollection<Reservation>();
                foreach (Reservation r in updatedData)
                {
                    res.Add(r);
                }
                Reservationen = res;
            });
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

        public void LoadReservationData()
        {
            StartUpdate(null, null);
        }

        private void SaveReservation(Reservation reservation)
        {
            ReservationAddWindow reservationAddWindow = new ReservationAddWindow();
            if (reservationAddWindow.ShowDialog() ?? false)
            {
                StartUpdate(null, null);
            }
        }

        private void DeleteReservation(Reservation reservation)
        {
            if (reservation != default(Reservation) && MessageBox.Show("Wollen Sie diese Reservation wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                resManager.Delete(reservation);
                Reservationen.Remove(reservation);
            }
        }
    }
}
