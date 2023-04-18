using SIMS_Projekat.Model;
using SIMS_Projekat.Repository;
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
    /// Interaction logic for User1Window.xaml
    /// </summary>
    public partial class User1Window : Window, INotifyPropertyChanged
    {
        private uint userId;
        public ObservableCollection<Accommodation> Accommodations { get; set; }
        public ObservableCollection<AccommodationReservation> Reservations { get; set; }
        public ObservableCollection<ReservationPostponement> Postponements { get; set; }
        public Accommodation SelectedAccommodation { get; set; }
        public AccommodationReservation SelectedReservation { get; set; }
        public ReservationPostponement SelectedPostponement { get; set; }
        public DateTime SelectedStartDate { get; set; }
        public DateTime SelectedEndDate { get; set; }
        public DateTime NewStartDate { get; set; }
        public DateTime NewEndDate { get; set; }

        private string accommodationName = "";
        public string AccommodationName
        {
            get { return accommodationName; }
            set
            {
                if (value != accommodationName)
                {
                    accommodationName = value;
                    OnPropertyChanged("AccommodationName");
                }
            }
        }

        private string city = "";
        public string City
        {
            get { return city; }
            set
            {
                if (value != city)
                {
                    city = value;
                    OnPropertyChanged("City");
                }
            }
        }

        private string country = "";
        public string Country
        {
            get { return country; }
            set
            {
                if (value != country)
                {
                    country = value;
                    OnPropertyChanged("Country");
                }
            }
        }

        private AccommodationType type;
        public AccommodationType Type
        {
            get { return type; }
            set
            {
                if (value != type)
                {
                    type = value;
                    OnPropertyChanged("Type");
                }
            }
        }

        private uint numberOfGuests;
        public uint NumberOfGuests
        {
            get { return numberOfGuests; }
            set
            {
                if (value != numberOfGuests)
                {
                    numberOfGuests = value;
                    OnPropertyChanged("NumberOfGuests");
                }
            }
        }

        private uint numberOfStayDays;
        public uint NumberOfStayDays
        {
            get { return numberOfStayDays; }
            set
            {
                if (value != numberOfStayDays)
                {
                    numberOfStayDays = value;
                    OnPropertyChanged("NumberOfStayDays");
                }
            }
        }

        public IEnumerable<AccommodationType> AccommodationTypes
        {
            get
            {
                return Enum.GetValues(typeof(AccommodationType))
                    .Cast<AccommodationType>();
            }
        }

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public User1Window(uint userId)
        {
            InitializeComponent();
            DataContext = this;
            Accommodations = new ObservableCollection<Accommodation>(AccommodationState.Accommodations.GetAll());
            Reservations = new ObservableCollection<AccommodationReservation>(AccommodationState.AccommodationReservations.GetAll().Where(x => x.GuestId == GlobalState.CurrentUser.Id));
            Postponements = new ObservableCollection<ReservationPostponement>(AccommodationState.ReservationPostponements.GetAll().Where(x => x.AccommodationReservation.GuestId == GlobalState.CurrentUser.Id));
            this.userId = userId;
        }

        private void SearchAccommodations(object sender, RoutedEventArgs e)
        {
            Accommodations = new ObservableCollection<Accommodation>(AccommodationState.Accommodations.SearchAccommodations(AccommodationName, Country, City, Type, NumberOfGuests, NumberOfStayDays));
            NotifyPropertyChanged("Accommodations");
        }

        private void ReserveAccommodation(object sender, RoutedEventArgs e)
        {
            if (SelectedAccommodation != null)
            {
                AccommodationState.AccommodationReservations.ReserveAccommodation(SelectedAccommodation.Id, SelectedStartDate, SelectedEndDate, NumberOfGuests, NumberOfStayDays, userId);
                Reservations = new ObservableCollection<AccommodationReservation>(AccommodationState.AccommodationReservations.GetAll().Where(x => x.GuestId == GlobalState.CurrentUser.Id));
                NotifyPropertyChanged("Reservations");
            }
            else
                MessageBox.Show("Accommodation is not selected!");
        }

        private void PostponeReservation(object sender, RoutedEventArgs e)
        {
            if (SelectedReservation != null)
            {
                AccommodationState.ReservationPostponements.CreatePostponement(SelectedReservation, NewStartDate, NewEndDate);
                Postponements = new ObservableCollection<ReservationPostponement>(AccommodationState.ReservationPostponements.GetAll().Where(x => x.AccommodationReservation.GuestId == GlobalState.CurrentUser.Id));
                NotifyPropertyChanged("Postponements");
            }
            else
                MessageBox.Show("Reservation not selected!");
        }

        private void CancelReservation(object sender, RoutedEventArgs e)
        {
            if (SelectedReservation != null)
            {
                AccommodationState.AccommodationReservations.CancelReservation(SelectedReservation);
                Reservations = new ObservableCollection<AccommodationReservation>(AccommodationState.AccommodationReservations.GetAll().Where(x => x.GuestId == GlobalState.CurrentUser.Id));
                NotifyPropertyChanged("Reservations");
                Postponements = new ObservableCollection<ReservationPostponement>(AccommodationState.ReservationPostponements.GetAll().Where(x => x.AccommodationReservation.GuestId == GlobalState.CurrentUser.Id));
                NotifyPropertyChanged("Postponements");
            }
            else
                MessageBox.Show("Reservation not selected!");
        }

        private void RateOwner(object sender, RoutedEventArgs e)
        {
            User1OwnerRate window = new User1OwnerRate();
            window.Show();
        }
    }
}