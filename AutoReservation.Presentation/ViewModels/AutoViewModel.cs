using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Presentation.Commands;
using AutoReservation.Presentation.Views;
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
    public class AutoViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<AutoDto> Autos { get; set; }
        public ICommand AddNewAutoCommand { get; set; }
        public ICommand RemoveAutoCommand { get; set; }

        public AutoViewModel()
        {
            AddNewAutoCommand = new RelayCommand<AutoDto>(param => SaveAuto(param));
            RemoveAutoCommand = new RelayCommand<AutoDto>(param => DeleteAuto(param));
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

        private void SaveAuto(AutoDto auto)
        {
            AutoAddWindow autoAddWindow = new AutoAddWindow();

            if (autoAddWindow.ShowDialog() ?? false)
            {
                Autos.Add(autoAddWindow.Advm.Auto);
            }
        }

        private void DeleteAuto(AutoDto auto)
        {
            if (auto != default(AutoDto) && MessageBox.Show("Wollen Sie diesen Eintrag wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Autos.Remove(auto);
            }
        }
    }
}
