using AutoReservation.Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutoReservation.Presentation.ViewModels
{
    public class KundeViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<KundeDto> _kunden;
        public ObservableCollection<KundeDto> Kunden { get { return _kunden; } set { SetProperty(ref _kunden, value); } }
        public AddCommand AddNewKundeCommand { get; set; }
        public RemoveCommand RemoveKundeCommand { get; set; }

        public KundeViewModel()
        {
            AddNewKundeCommand = new AddCommand(this);
            RemoveKundeCommand = new RemoveCommand(this);
        }

        private bool SetProperty<T>(ref T field, T value, [CallerMemberName] string name = null)
        {
            if(Equals(field, value))
            {
                return false;
            }
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            return true;
        }

        public class AddCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;
            private KundeViewModel ViewModel{get;set;}
            public AddCommand(KundeViewModel ViewModel)
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
                KundeAddWindow kundeAddWindow = new KundeAddWindow(new KundeDto() { Geburtsdatum = DateTime.Now });
                if (kundeAddWindow.ShowDialog() ?? false)
                {
                    ViewModel.Kunden.Add(kundeAddWindow.Kunde);
                }
            }
        }

        public class RemoveCommand : ICommand
        {
            public event EventHandler CanExecuteChanged;
            private KundeViewModel ViewModel { get; set; }
            public RemoveCommand(KundeViewModel ViewModel)
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
                KundeDto kunde = (KundeDto)parameter;
                if (kunde != default(KundeDto) && MessageBox.Show("Wollen Sie diesen Eintrag wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    ViewModel.Kunden.Remove(kunde);
                }
            }
        }

    }
}
