using AutoReservation.BusinessLayer;
using AutoReservation.Dal.Entities;
using AutoReservation.Presentation.Commands;
using AutoReservation.Presentation.Interfaces;
using AutoReservation.Presentation.Services;
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
        private ObservableCollection<Auto> _autos;
        public ObservableCollection<Auto> Autos { get { return _autos; } set { SetProperty(ref _autos, value); } }
        public ICommand AddNewAutoCommand { get; set; }
        public ICommand RemoveAutoCommand { get; set; }

        private AutoManager autoManager;
        private IMessageDisplayer displayer;

        public AutoViewModel() : this(new MessageBoxDisplayer())
        {
            
        }

        public AutoViewModel(IMessageDisplayer displayer)
        {
            Autos = new ObservableCollection<Auto>();
            AddNewAutoCommand = new RelayCommand<Auto>(param => SaveAuto(param));
            RemoveAutoCommand = new RelayCommand<Auto>(param => DeleteAuto(param));
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

        private void SaveAuto(Auto auto)
        {
            AutoAddWindow autoAddWindow = new AutoAddWindow();

            if (autoAddWindow.ShowDialog() ?? false)
            {
                Autos.Add(autoAddWindow.Advm.Auto);
            }
        }

        private void DeleteAuto(Auto auto)
        {
            //if (auto != default(Auto) && MessageBox.Show("Wollen Sie diesen Eintrag wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            if (auto != default(Auto) && displayer.DisplayDialog("Löschen", "Wollen Sie diesen Eintrag wirklich löschen?")) 
            {
                autoManager.Delete(auto);
                Autos.Remove(auto);
            }
        }

        public void LoadAutoData()
        {
            Task.Run(() => LoadAutoDataWorkingThread());
        }

        private void LoadAutoDataWorkingThread()
        {
            ObservableCollection<Auto> col = new ObservableCollection<Auto>();

            foreach (Auto auto in autoManager.List)
            {
                col.Add(auto);
            }

            Autos = col;
        }
    }
}
