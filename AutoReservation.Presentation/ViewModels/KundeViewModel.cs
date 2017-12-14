using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Presentation.Commands;
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
        public ICommand AddNewKundeCommand { get; set; }
        public ICommand RemoveKundeCommand { get; set; }

        public KundeViewModel()
        {
            AddNewKundeCommand = new RelayCommand<KundeDto>(param => SaveKunde(param));
            RemoveKundeCommand = new RelayCommand<KundeDto>(param => DeleteKunde(param));
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

        private void SaveKunde(KundeDto kunde)
        {
            KundeAddWindow kundeAddWindow = new KundeAddWindow();

            if (kundeAddWindow.ShowDialog() ?? false)
            {
                Kunden.Add(kundeAddWindow.Kdvm.Kunde);
            }
        }

        private void DeleteKunde(KundeDto kunde)
        {
            if (kunde != default(KundeDto) && MessageBox.Show("Wollen Sie diesen Eintrag wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Kunden.Remove(kunde);
            }
        }
    }
}
