using AutoReservation.BusinessLayer;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Dal.Entities;
using AutoReservation.Presentation.Views;
using AutoReservation.Service.Wcf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace AutoReservation.Presentation.ViewModels
{
    public class ReservationViewModel : INotifyPropertyChanged
    {
        private ReservationManager resManager;
        public event PropertyChangedEventHandler PropertyChanged;
        private bool _hidden;
        public bool Hidden { get { return _hidden; } set { SetProperty(ref _hidden, value); } }
        public AddCommand AddReservationCommand { get; set; }
        public RemoveCommand RemoveReservationCommand { get; set; }
        public FilterCommand FilterReservationsCommand { get; set; }

        private System.Timers.Timer timer;

        private ObservableCollection<ReservationDto> _reservationen;
        public ObservableCollection<ReservationDto> Reservationen
        {
            get
            {
                return _reservationen;
            }
            set { SetProperty(ref _reservationen, value); }
        }

        public ReservationViewModel()
        {
            resManager = new ReservationManager();
            Hidden = true;
            FilterReservationsCommand = new FilterCommand(this);
            RemoveReservationCommand = new RemoveCommand(this);
            AddReservationCommand = new AddCommand(this);
            timer = new System.Timers.Timer(5000);
            timer.Elapsed += startUpdate;
            timer.Start();
        }
        private void startUpdate(Object source, ElapsedEventArgs e)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                var updatedData = resManager.List;
                ObservableCollection<ReservationDto> res = new ObservableCollection<ReservationDto>();
                foreach (Reservation r in updatedData)
                {
                    res.Add(r.ConvertToDto());
                }
                Reservationen = res;
            }));
        }

        private void UpdateReservations()
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

    }

    public class AddCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private ReservationViewModel ViewModel { get; set; }
        public AddCommand(ReservationViewModel ViewModel)
        {
            this.ViewModel = ViewModel;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("Entered Add Command");
            ReservationAddWindow reservationAddWindow = new ReservationAddWindow();
            if (reservationAddWindow.ShowDialog() ?? false)
            {
                ViewModel.Reservationen.Add(reservationAddWindow.rdvm.Reservation);
            }
        }
    }

    public class RemoveCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public ReservationViewModel ViewModel { get; set; }
        public RemoveCommand(ReservationViewModel ViewModel)
        {
            this.ViewModel = ViewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            Console.WriteLine("Entered RemoveCommand");
            ReservationDto reservation = (ReservationDto)parameter;
            if (reservation != default(ReservationDto) && MessageBox.Show("Wollen Sie diese Reservation wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                ViewModel.Reservationen.Remove(reservation);
            }
        }
    }

    public class FilterCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        public ReservationViewModel ViewModel { get; set; }
        public FilterCommand(ReservationViewModel ViewModel)
        {
            this.ViewModel = ViewModel;
        }
        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            ViewModel.Hidden = !ViewModel.Hidden;
        }
    }
}
