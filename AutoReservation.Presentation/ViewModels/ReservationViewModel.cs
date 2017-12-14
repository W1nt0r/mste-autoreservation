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
using System.Linq;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Presentation.Interfaces;
using AutoReservation.Presentation.Services;

namespace AutoReservation.Presentation.ViewModels
{
    public class ReservationViewModel : INotifyPropertyChanged
    {
        private IMessageDisplayer displayer;
        private ReservationManager resManager;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool? _hidden;
        public bool? Hidden { get { return _hidden; } set { SetProperty(ref _hidden, value); StartUpdate(null, null); } }
        private bool _loading;
        public bool Loading { get { return _loading; } set { SetProperty(ref _loading, value); } }
        private bool _empty;
        public bool Empty { get { return _empty; } set { SetProperty(ref _empty, value); } }
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
                    var res = _reservationen.Where(x => x.Bis > DateTime.Now);
                    ObservableCollection<Reservation> re = new ObservableCollection<Reservation>();
                    foreach (Reservation r in res){
                        re.Add(r);
                    }
                    return re;
                }
                else
                {
                    return _reservationen;
                }
            }
            set { SetProperty(ref _reservationen, value); }
        }
        public ReservationViewModel(IMessageDisplayer messageDisplayer)
        {
            this.displayer = messageDisplayer;
            resManager = new ReservationManager();
            Hidden = true;
            RemoveReservationCommand = new RelayCommand<Reservation>(param => DeleteReservation(param));
            AddReservationCommand = new RelayCommand<Reservation>(param => SaveReservation(param));
            timer = new Timer(1000);
            timer.Elapsed += StartUpdate;
            timer.Start();
        }

        public ReservationViewModel() : this(new MessageBoxDisplayer())
        {
            
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
                Empty = Reservationen.Count == 0;
                Loading = false;
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
                Empty = Reservationen.Count == 0;
            }
        }

        private void DeleteReservation(Reservation reservation)
        {
            if (reservation != default(Reservation) && MessageBox.Show("Wollen Sie diese Reservation wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    resManager.Delete(reservation);
                    Reservationen.Remove(reservation);
                }
                catch (DatabaseChangeException)
                {
                    displayer.DisplayError("Fehler beim Löschen", "Der Eintrag konnte nicht aus der Datenbank gelöscht werden!");
                }
                catch (OptimisticConcurrencyException<Auto>)
                {
                    displayer.DisplayError("Fehler beim Löschen", "Es ist ein Nebenläufigkeitsproblem aufgetreten. Bitte versuchen Sie es erneut.");
                }
                catch (EntityNotFoundException)
                {
                    Reservationen.Remove(reservation);
                    displayer.DisplayError("Fehler beim Löschen", "Der zu löschende Eintrag existiert nicht in der Datenbank.");
                }
                Empty = Reservationen.Count == 0;
            }
        }
    }
}
