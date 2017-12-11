using System;
using System.Windows;
using System.Windows.Input;
using AutoReservation.Common.DataTransferObjects;
using System.Collections.ObjectModel;

namespace AutoReservation.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public ObservableCollection<KundeDto> Kunden { get; set; }
        private bool CloseCommand_CanExecute { get; set; }
        private bool MaximizeCommand_CanExecute { get; set; }
        private bool MinimizeCommand_CanExecute { get; set; }
        private bool RestoreWindowCommand_CanExecute { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            CloseCommand_CanExecute = true;
            MaximizeCommand_CanExecute = true;
            MinimizeCommand_CanExecute = true;
            RestoreWindowCommand_CanExecute = false;
            DataContext = this;

            Kunden = new ObservableCollection<KundeDto>
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
        }

        private void KundeAddButton_Click(object sender, RoutedEventArgs e)
        {
            KundeAddWindow kundeAddWindow = new KundeAddWindow(new KundeDto() { Geburtsdatum = DateTime.Now });
            if (kundeAddWindow.ShowDialog() ?? false)
            {
                Kunden.Add(kundeAddWindow.Kunde);
            }
        }

        private void DeleteRow_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            KundeDto kunde = (KundeDto)e.Parameter;
            if (kunde != default(KundeDto) && MessageBox.Show("Wollen Sie diesen Eintrag wirklich löschen?", "Löschen", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                Kunden.Remove(kunde);
            }
        }
    }
}
