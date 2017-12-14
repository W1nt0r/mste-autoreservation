using AutoReservation.BusinessLayer;
using AutoReservation.BusinessLayer.Exceptions;
using AutoReservation.Common.DataTransferObjects;
using AutoReservation.Dal.Entities;
using AutoReservation.Presentation.Commands;
using AutoReservation.Presentation.Interfaces;
using AutoReservation.Presentation.Services;
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
        private ObservableCollection<Kunde> _kunden;
        public ObservableCollection<Kunde> Kunden { get { return _kunden; } set { SetProperty(ref _kunden, value); } }
        private bool _loading;
        public bool Loading { get { return _loading; } set { SetProperty(ref _loading, value); } }
        private bool _empty;
        public bool Empty { get { return _empty; } set { SetProperty(ref _empty, value); } }
        public ICommand AddNewKundeCommand { get; set; }
        public ICommand RemoveKundeCommand { get; set; }

        private KundeManager kundeManager;
        private IMessageDisplayer displayer;

        public KundeViewModel() : this(new MessageBoxDisplayer()) { }

        public KundeViewModel(IMessageDisplayer displayer)
        {
            AddNewKundeCommand = new RelayCommand<Kunde>(param => SaveKunde(param));
            RemoveKundeCommand = new RelayCommand<Kunde>(param => DeleteKunde(param));
            kundeManager = new KundeManager();
            this.displayer = displayer;
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

        private void SaveKunde(Kunde kunde)
        {
            KundeAddWindow kundeAddWindow = new KundeAddWindow();

            if (kundeAddWindow.ShowDialog() ?? false)
            {
                Kunden.Add(kundeAddWindow.Kdvm.Kunde);
                Empty = Kunden.Count == 0;
            }
        }

        private void DeleteKunde(Kunde kunde)
        {
            if (kunde != default(Kunde) && displayer.DisplayDialog("Löschen", "Wollen Sie diesen Eintrag wirklich löschen?"))
            {
                try
                {
                    kundeManager.Delete(kunde);
                    Kunden.Remove(kunde);
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
                    Kunden.Remove(kunde);
                    displayer.DisplayError("Fehler beim Löschen", "Der zu löschende Eintrag existiert nicht in der Datenbank.");
                }
                Empty = Kunden.Count == 0;

            }
        }

        public void LoadKundeData()
        {
            Task.Run(() => LoadKundeDataWorkingThread());
        }

        private void LoadKundeDataWorkingThread()
        {
            ObservableCollection<Kunde> col = new ObservableCollection<Kunde>();

            foreach (Kunde kunde in kundeManager.List)
            {
                col.Add(kunde);
            }

            Kunden = col;
            Empty = Kunden.Count == 0;
            Loading = false;
        }
    }
}
