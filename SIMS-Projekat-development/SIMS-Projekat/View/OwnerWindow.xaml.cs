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
    /// Interaction logic for OwnerWindow.xaml
    /// </summary>
    public partial class OwnerWindow : Window, INotifyPropertyChanged
    {
        private string accommodationName;
        public string AccommodationName
        {
            get { return accommodationName; }
            set
            {
                if (accommodationName != value)
                {
                    accommodationName = value;
                    OnPropertyChanged();
                }
            }
        }

        private string city;
        public string City
        {
            get { return city; }
            set
            {
                if (city != value)
                {
                    city = value;
                    OnPropertyChanged();
                }
            }
        }
        private string country;
        public string Country
        {
            get { return country; }
            set
            {
                if (country != value)
                {
                    country = value;
                    OnPropertyChanged();
                }
            }
        }

        private AccommodationType type;
        public AccommodationType Type
        {
            get { return type; }
            set
            {
                if (type != value)
                {
                    type = value;
                    OnPropertyChanged();
                }
            }
        }

        private uint guestMax;
        public uint GuestMax
        {
            get { return guestMax; }
            set
            {
                if (guestMax != value)
                {
                    guestMax = value;
                    OnPropertyChanged();
                }
            }
        }

        private uint minReservationDays;
        public uint MinReservationDays
        {
            get { return minReservationDays; }
            set
            {
                if (minReservationDays != value)
                {
                    minReservationDays = value;
                    OnPropertyChanged();
                }
            }
        }

        private uint reservationCancellationCutoffDays;
        public uint ReservationCancellationCutoffDays
        {
            get { return reservationCancellationCutoffDays; }
            set
            {
                if (reservationCancellationCutoffDays != value)
                {
                    reservationCancellationCutoffDays = value;
                    OnPropertyChanged();
                }
            }
        }

        private string imageURLs;
        public string ImageURLs
        {
            get { return imageURLs; }
            set
            {
                if (imageURLs != value)
                {
                    imageURLs = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool notifications;
        public bool Notifications
        {
            get { return notifications; }
            set
            {
                if (notifications != value)
                {
                    notifications = value;
                    OnPropertyChanged();
                }
            }
        }

        private GuestRating guestRating;
        public GuestRating GuestRating
        {
            get { return guestRating; }
            set
            {
                if (guestRating != value)
                {
                    guestRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private uint cleanlinessRating;
        public uint CleanlinessRating
        {
            get { return cleanlinessRating; }
            set
            {
                if (cleanlinessRating != value)
                {
                    cleanlinessRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private uint complianceRating;
        public uint ComplianceRating
        {
            get { return complianceRating; }
            set
            {
                if (complianceRating != value)
                {
                    complianceRating = value;
                    OnPropertyChanged();
                }
            }
        }
        private uint communicationRating;
        public uint CommunicationRating
        {
            get { return communicationRating; }
            set
            {
                if (communicationRating != value)
                {
                    communicationRating = value;
                    OnPropertyChanged();
                }
            }
        }
        private uint respectForPropertyRating;
        public uint RespectForPropertyRating
        {
            get { return respectForPropertyRating; }
            set
            {
                if (respectForPropertyRating != value)
                {
                    respectForPropertyRating = value;
                    OnPropertyChanged();
                }
            }
        }
        private uint demeanorRating;
        public uint DemeanorRating
        {
            get { return demeanorRating; }
            set
            {
                if (demeanorRating != value)
                {
                    demeanorRating = value;
                    OnPropertyChanged();
                }
            }
        }
        private uint complaintsRating;
        public uint ComplaintsRating
        {
            get { return complaintsRating; }
            set
            {
                if (complaintsRating != value)
                {
                    complaintsRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                if (comment != value)
                {
                    comment = value;
                    OnPropertyChanged();
                }
            }
        }

        private ReservationPostponement selectedPostponement;
        public ReservationPostponement SelectedPostponement
        {
            get { return selectedPostponement; }
            set
            {
                if (selectedPostponement != value)
                {
                    selectedPostponement = value;
                    OnPropertyChanged();
                }
            }
        }

        private double averageRating;
        public double AverageRating
        {
            get { return averageRating; }
            set
            {
                if (averageRating != value)
                {
                    averageRating = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool isSuperUser => GlobalState.CurrentUser.SuperUser;
        public bool IsSuperUser
        {
            get { return isSuperUser; }
            set
            {
            }
        }
        private string reasoning;
        public string Reasoning
        {
            get { return reasoning; }
            set
            {
                if (reasoning != value)
                {
                    reasoning = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<GuestRating> UsersToRate { get; set; }
        public ObservableCollection<ReservationPostponement> Postponements { get; set; }
        public ObservableCollection<OwnerRating> VisibleRatings { get; set; }

        public List<AccommodationType> accommodationTypes = Enum.GetValues(typeof(AccommodationType)).Cast<AccommodationType>().ToList();
        public List<AccommodationType> AccommodationTypes
        {
            get { return accommodationTypes; }
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

        public List<string> Countries { get; set; }
        public List<string> Cities { get; set; } = new List<string>();
        public OwnerWindow()
        {
            InitializeComponent();
            DataContext = this;
            Countries = GlobalState.Locations.Countries;
            UsersToRate = new ObservableCollection<GuestRating>(AccommodationState.GuestRatings.GetUsersToRate());
            Postponements = new ObservableCollection<ReservationPostponement>(AccommodationState.ReservationPostponements.GetAllRequestsForOwner());
            VisibleRatings = new ObservableCollection<OwnerRating>(AccommodationState.OwnerRatings.GetAllVisibleRatings());
            AverageRating = AccommodationState.OwnerRatings.GetAverageOwnerRating();
            NotifyPropertyChanged("UsersToRate");
            NotifyPropertyChanged("Postponements");
            NotifyPropertyChanged(nameof(VisibleRatings));
            UsersComboBox.IsEnabled = UsersToRate.Count > 0 ? true : false;
            AccommodationState.OwnerRatings.SetSuperOwnerStatus();
            HandleUserRatingNotifications();
        }

        private void HandleUserRatingNotifications()
        {
            if (Notifications = AccommodationState.GuestRatings.HasPendingRatings(GlobalState.CurrentUser)) DisplayUserRatingNotification();
        }
        private void DisplayUserRatingNotification()
        {
            // wpf magija ovde
        }

        private void RateGuest(object sender, RoutedEventArgs e)
        {
            var result = AccommodationState.GuestRatings.RateGuest(GuestRating, CleanlinessRating, ComplianceRating, CommunicationRating, RespectForPropertyRating, DemeanorRating, ComplaintsRating, Comment);
            MessageBox.Show(result.Message);
            if (result.Success)
            {
                VisibleRatings = new ObservableCollection<OwnerRating>(AccommodationState.OwnerRatings.GetAllVisibleRatings());
                AverageRating = AccommodationState.OwnerRatings.GetAverageOwnerRating();
                NotifyPropertyChanged(nameof(VisibleRatings));
            }
        }

        private void CreateAccommodation(object sender, RoutedEventArgs e)
        {
            if (GuestMax < 0 || MinReservationDays < 0 || ReservationCancellationCutoffDays < 0)
            {
                MessageBox.Show("Incorrect integer input :)");
                return;
            }
            List<string> images = ImageURLs?.Split("^").ToList() ?? new List<string>();
            if (AccommodationState.Accommodations.Save(new Accommodation(AccommodationName, City, Country, Type, GuestMax, MinReservationDays, ReservationCancellationCutoffDays, images, GlobalState.CurrentUser.Id)) != default(Accommodation))
            {
                MessageBox.Show("Accommodation saved.");
                return;
            }
            MessageBox.Show("Input validation failed.");
        }

        private void ApprovePostponementRequest(object sender, RoutedEventArgs e)
        {
            var result = AccommodationState.ReservationPostponements.HandlePostponement(SelectedPostponement, true, "Approved");
            MessageBox.Show(result.Message);
            Postponements = new ObservableCollection<ReservationPostponement>(AccommodationState.ReservationPostponements.GetAllRequestsForOwner());
            NotifyPropertyChanged(nameof(Postponements));
        }

        private void DenyPostponementRequest(object sender, RoutedEventArgs e)
        {
            var result = AccommodationState.ReservationPostponements.HandlePostponement(SelectedPostponement, false, Reasoning);
            MessageBox.Show(result.Message);
            Postponements = new ObservableCollection<ReservationPostponement>(AccommodationState.ReservationPostponements.GetAllRequestsForOwner());
            NotifyPropertyChanged(nameof(Postponements));
        }

        private void CountryChanged(object sender, SelectionChangedEventArgs e)
        {
            Cities = GlobalState.Locations.GetCountryCities(Country);
            NotifyPropertyChanged(nameof(Cities));
        }
    }
}
