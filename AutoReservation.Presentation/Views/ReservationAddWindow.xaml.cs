using AutoReservation.Presentation.ViewModels;
using System;
using System.Windows;

namespace AutoReservation.Presentation.Views
{
    /// <summary>
    /// Interaction logic for ReservationAddWindow.xaml
    /// </summary>
    public partial class ReservationAddWindow : Window
    {
        private bool initialized = false;
        public ReservationDetailViewModel rdvm;
        public ReservationAddWindow()
        {
            rdvm = new ReservationDetailViewModel();
            
            DataContext = rdvm;
            InitializeComponent();
            VonDatePicker.DisplayDateStart = DateTime.Now;
            initialized = true;
        }

        private void VonDatePicker_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (initialized)
            {
                DateTime newTime = (DateTime)VonDatePicker.SelectedDate;
                newTime = newTime.AddHours(24);
                BisDatePicker.DisplayDateStart = newTime;
                if (newTime.AddHours(24) > BisDatePicker.SelectedDate)
                {
                    BisDatePicker.SelectedDate = newTime;
                }

            }
        }
    }
}
