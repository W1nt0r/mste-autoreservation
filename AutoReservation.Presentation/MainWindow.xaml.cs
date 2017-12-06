using AutoReservation.Common.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoReservation.Presentation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<AutoDto> AutoList { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            InitDefaultVals();

            DataContext = this;
        }

        private void InitDefaultVals()
        {
            AutoList = new ObservableCollection<AutoDto>() {
                new AutoDto { AutoKlasse = AutoKlasse.Luxusklasse, Marke = "Mercedes", Tagestarif = 212, Basistarif = 2323 },
                new AutoDto { AutoKlasse = AutoKlasse.Luxusklasse, Marke = "BMW", Tagestarif = 266, Basistarif = 2454 },
                new AutoDto { AutoKlasse = AutoKlasse.Luxusklasse, Marke = "Tesla", Tagestarif = 100, Basistarif = 9233 },
                new AutoDto { AutoKlasse = AutoKlasse.Standard, Marke = "Opel", Basistarif = 560 },
                new AutoDto { AutoKlasse = AutoKlasse.Mittelklasse, Marke = "Renault", Basistarif = 700 },
                new AutoDto { AutoKlasse = AutoKlasse.Standard, Marke = "Dacia", Basistarif = 450 }
            };
        }

        private void OnRowDelete(object sender, RoutedEventArgs e)
        {
            if(sender is Button)
            {
                var DataContext = (sender as Button).DataContext;

                if(DataContext is AutoDto)
                {
                    CustomDialog confirmDialog = new CustomDialog("Bestätigung", "Wollen Sie den Eintrag wirklich löschen?");

                    if(confirmDialog.ShowDialog() ?? false)
                    {
                        var correspondingCar = DataContext as AutoDto;
                        AutoList.Remove(correspondingCar);
                    }
                }
            }
        }

        private void addElement(object sender, RoutedEventArgs e)
        {
            var currentTabItem = MainControl.SelectedItem as TabItem;

            switch(currentTabItem.Header)
            {
                case "Autos":
                    break;
            }
        }
    }
}
