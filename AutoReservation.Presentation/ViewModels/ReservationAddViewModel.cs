using AutoReservation.Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoReservation.Presentation.ViewModels
{
    public class ReservationAddViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        private int _reservationsNr;
        public int ReservationsNr { get { return _reservationsNr; } set { SetProperty(ref _reservationsNr, value); } }
        private DateTime _bis;
        public DateTime Bis { get { return _bis; } set { SetProperty(ref _bis, value); } }
        private DateTime _von;
        public DateTime Von { get { return _von; } set { SetProperty(ref _von, value); } }
        private byte[] _rowVersion;
        public byte[] RowVersion { get { return _rowVersion; } set { SetProperty(ref _rowVersion, value); } }

        private ObservableCollection<KundeDto> _kunden;
        public ObservableCollection<KundeDto> Kunden { get { return _kunden; } set { SetProperty(ref _kunden, value); } }

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
}
