using System;
using System.Windows;
using System.Windows.Input;
using AutoReservation.Common.DataTransferObjects;
using System.Collections.ObjectModel;
using AutoMapper;
using AutoReservation.Presentation.ViewModels;
using AutoReservation.Dal.Entities;

namespace AutoReservation.Presentation.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public KundeViewModel Kvm { get; set; }
        public ReservationViewModel Rvm { get; set; }
        public AutoViewModel Avm { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Kvm = new KundeViewModel();
            Rvm = new ReservationViewModel();
            Avm = new AutoViewModel();
            DataContext = this;

            Kvm.Kunden = new ObservableCollection<KundeDto>
             {
                new KundeDto {Id = 1, Nachname = "Nass", Vorname = "Anna", Geburtsdatum = new DateTime(1981, 05, 05)},
                new KundeDto {Id = 2, Nachname = "Beil", Vorname = "Timo", Geburtsdatum = new DateTime(1980, 09, 09)},
                new KundeDto {Id = 3, Nachname = "Pfahl", Vorname = "Martha", Geburtsdatum = new DateTime(1990, 07, 03)},
                new KundeDto {Id = 4, Nachname = "Zufall", Vorname = "Rainer", Geburtsdatum = new DateTime(1954, 11, 11)},
                new KundeDto {Id = 5, Nachname = "Fährlich", Vorname = "Sergej", Geburtsdatum = new DateTime(1963, 05, 31)},
                new KundeDto {Id = 6, Nachname = "Döpfel", Vorname = "Herr", Geburtsdatum = new DateTime(1982, 03, 10)},
                new KundeDto {Id = 7, Nachname = "Committed", Vorname = "Hans", Geburtsdatum = new DateTime(1975, 02, 26)},
                new KundeDto {Id = 8, Nachname = "Hof", Vorname = "Fred", Geburtsdatum = new DateTime(1937, 09, 03)},
                new KundeDto {Id = 9, Nachname = "La Faire", Vorname = "René", Geburtsdatum = new DateTime(1958, 04, 01)},
                new KundeDto {Id = 10, Nachname = "Miamarsch", Vorname = "Alex", Geburtsdatum = new DateTime(1995, 10, 17)},
                new KundeDto {Id = 10, Nachname = "Mann", Vorname = "Herr Herrmann", Geburtsdatum = new DateTime(1953, 12, 24)}
            };

            AutoDto Car1 = new AutoDto() { Marke = "Kia", Tagestarif = 10, Basistarif = 100 };
            AutoDto Car2 = new AutoDto() { Marke = "Peugeot", Tagestarif = 10, Basistarif = 100 };

            Rvm.Reservationen = new ObservableCollection<ReservationDto>
            {
            new ReservationDto { Auto = Car1, Kunde = Kvm.Kunden[0], Von = DateTime.Now, Bis = DateTime.Now.AddDays(1) },
            new ReservationDto { Auto = Car2, Kunde = Kvm.Kunden[1], Von = DateTime.Now, Bis = DateTime.Now.AddDays(2) },
            new ReservationDto { Auto = Car1, Kunde = Kvm.Kunden[2], Von = DateTime.Now.AddDays(-3), Bis = DateTime.Now.AddDays(-2) },
            new ReservationDto { Auto = Car1, Kunde = Kvm.Kunden[3], Von = DateTime.Now, Bis = DateTime.Now.AddDays(1) },
            new ReservationDto { Auto = Car2, Kunde = Kvm.Kunden[4], Von = DateTime.Now, Bis = DateTime.Now.AddDays(1) }
            };

            /*Avm.Autos = new ObservableCollection<AutoDto>() {
                new AutoDto { AutoKlasse = AutoKlasse.Luxusklasse, Marke = "Mercedes", Tagestarif = 212, Basistarif = 2323 },
                new AutoDto { AutoKlasse = AutoKlasse.Luxusklasse, Marke = "BMW", Tagestarif = 266, Basistarif = 2454 },
                new AutoDto { AutoKlasse = AutoKlasse.Luxusklasse, Marke = "Tesla", Tagestarif = 100, Basistarif = 9233 },
                new AutoDto { AutoKlasse = AutoKlasse.Standard, Marke = "Opel", Tagestarif = 560 },
                new AutoDto { AutoKlasse = AutoKlasse.Mittelklasse, Marke = "Renault", Tagestarif = 700 },
                new AutoDto { AutoKlasse = AutoKlasse.Standard, Marke = "Dacia", Tagestarif = 450 }
            };*/
            /*Avm.Autos = new ObservableCollection<Auto>()
            {
                new LuxusklasseAuto {Marke = "Mercedes", Tagestarif = 212, Basistarif = 2323},
                new LuxusklasseAuto {Marke = "BMW", Tagestarif = 266, Basistarif = 2454 },
                new StandardAuto {Marke = "Opel", Tagestarif = 560},
                new MittelklasseAuto {Marke = "Renault", Tagestarif = 700}
            };*/
            
        }

        private void AutoReservationTabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(e.OriginalSource == AutoReservationTabControl)
            {
                if(AutoReservationTabControl.SelectedIndex == 0)
                {
                    Avm.LoadAutoData();
                }
            }
        }
    }
}
