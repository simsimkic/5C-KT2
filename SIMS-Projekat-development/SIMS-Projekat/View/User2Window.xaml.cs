using Accessibility;

using SIMS_Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SIMS_Projekat.View
{
    /// <summary>
    /// Interaction logic for GuestWindow.xaml
    /// </summary>
    public partial class User2Window : Window, INotifyPropertyChanged
    {
        public ObservableCollection<Tour> Tours { get; set; }



        private uint tourId;
        public uint TourId
        {
            get => tourId;
            set
            {
                if (value != tourId)
                {
                    tourId = value;
                    OnPropertyChanged();
                }
            }
        }

        private uint reservationGuestId;

        public uint ReservationGuestId
        {
            get => reservationGuestId;
            set
            {
                if (value != reservationGuestId)
                {
                    reservationGuestId = value;
                    OnPropertyChanged();
                }
            }
        }

        private uint reservationGuestNumber;

        public uint ReservationGuestNumber
        {
            get => reservationGuestNumber;
            set
            {
                if (value != reservationGuestNumber)
                {
                    reservationGuestNumber = value;
                    OnPropertyChanged();
                }
            }
        }

        private string city;
        public string City
        {
            get => city;
            set
            {
                if (value != city)
                {
                    city = value;
                    OnPropertyChanged();
                }
            }
        }

        private string country;
        public string Country
        {
            get => country;
            set
            {
                if (value != country)
                {
                    country = value;
                    OnPropertyChanged();
                }
            }
        }


        private string lang;
        public string Lang
        {
            get => lang;
            set
            {
                if (value != lang)
                {
                    lang = value;
                    OnPropertyChanged();
                }
            }
        }

        private uint guestNumber;
        public uint GuestNumber
        {
            get => guestNumber;
            set
            {
                if (value != guestNumber)
                {
                    guestNumber = value;
                    OnPropertyChanged();
                }
            }
        }
        private uint duration;
        public uint Duration
        {
            get => duration;
            set
            {
                if(value != duration)
                {
                    duration = value;
                    OnPropertyChanged();
                }
            }
        }
        public User2Window()
        {

            InitializeComponent();
            DataContext = this;
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public void ScheduleTour()
        {

            int result = TourState.TourReservations.ScheduleReservation(TourId, ReservationGuestId, ReservationGuestNumber);
            if (result == 1)
            {
                MessageBox.Show("Uspesno kreirana rezervacija ture.");
            }
            else if (result == -1)
            {
                uint availableSlots = TourState.Tours.GetById(TourId).CurrentFreeSlots;
                string messageBoxText = "Nije moguce napraviti rezervaciju, smanjiti broj gostiju na " + availableSlots.ToString();
                var messageBoxResult = MessageBox.Show(messageBoxText, "Rezervacija ture", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (messageBoxResult == MessageBoxResult.Yes && availableSlots > 0)
                {
                    Trace.WriteLine("Usao u if!");
                    TourState.TourReservations.ScheduleReservation(TourId, ReservationGuestId, availableSlots);
                }
                else
                {
                    List<Tour> tourReccommendations = TourState.TourReservations.GetAlternateTours(TourId, reservationGuestNumber);
                    // MessageBox.Show(tourReccommendations.Count.ToString());
                    foreach (var item in tourReccommendations)
                    {
                        MessageBox.Show("Broj slobodnih mesta: " + item.CurrentFreeSlots.ToString(), "Id alternativne ture: " + item.Id.ToString());
                    }
                }
            }
            else if (result == -2)
            {
                MessageBox.Show("Zadata tura nema validni datum.");
            }
            else if (result == -3)
            {
                MessageBox.Show("Zadata tura ne postoji");
            }
            else
            {
                MessageBox.Show("Neispravan broj gostiju.");
            }
        }

        private void Search(object sender, RoutedEventArgs e)
        {
        
            var result = TourState.Tours.SearchTours(City, Country, Lang, GuestNumber, Duration);
            string message = "\n";

            if (result != null)
            {
                foreach (Tour tour in result)
                {
                    message += "Tura:" + tour.Name + " " + tour.Country + " " + tour.City + " " + tour.Description + " " + tour.Language + " " + tour.GuestMax + " " + tour.Duration+ " "+ tour.CurrentFreeSlots+ "\n";
                }
                NotifyPropertyChanged(message);
                MessageBox.Show(message);

            }
            

        }

        private void Reserve(object sender, RoutedEventArgs e)
        {
            ScheduleTour();
        }
    }
}
