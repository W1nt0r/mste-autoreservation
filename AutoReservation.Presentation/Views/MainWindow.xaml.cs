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

        }

        private void AutoReservationTabControl_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(e.OriginalSource == AutoReservationTabControl)
            {
                switch (AutoReservationTabControl.SelectedIndex)
                {
                    case 0:
                        {
                            Avm.Loading = true;
                            Avm.LoadAutoData();
                            break;
                        }
                    case 1:
                        {
                            Kvm.Loading = true;
                            Kvm.LoadKundeData();
                            break;
                        }
                    case 2:
                        {
                            Rvm.Loading = true;
                            Rvm.LoadReservationData();
                            break;
                        }
                }

            }
        }
    }
}
