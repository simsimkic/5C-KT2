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

    public partial class GuideWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void NotifyPropertyChanged(string name)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }


        private string tourName;
        public string TourName
        {
            get { return tourName; }
            set
            {
                if (tourName != value)
                {
                    tourName = value;
                    OnPropertyChanged("Name");
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
                    OnPropertyChanged("City");
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
                    OnPropertyChanged("Country");
                }
            }
        }

        private string description;

        public string Description
        {
            get { return description; }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged("Description");
                }
            }
        }

        private string tourLanguage;

        public string TourLanguage
        {
            get { return tourLanguage; }
            set
            {
                if (tourLanguage != value)
                {
                    tourLanguage = value;
                    OnPropertyChanged("Language");
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
                    OnPropertyChanged("GuestMax");
                }
            }
        }

        private string keyPoints;
        public string KeyPoints
        {
            get { return keyPoints; }
            set
            {
                if (keyPoints != value)
                {
                    keyPoints = value;
                    OnPropertyChanged("KeyPoints");
                }
            }
        }


        private string startDate;
        public string StartDate
        {
            get { return startDate; }
            set
            {
                if (startDate != value)
                {
                    startDate = value;
                    OnPropertyChanged("StartDate");
                }

            }
        }

        private uint duration;
        public uint Duration
        {
            get { return duration; }
            set
            {
                if (duration != value)
                {
                    duration = value;
                    OnPropertyChanged("Duration");
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
                    OnPropertyChanged("ImageURLs");
                }
            }
        }


        private uint guideId;
        public uint GuideId
        {
            get { return guideId; }
            set
            {
                if (guideId != value)
                {
                    guideId = value;
                    OnPropertyChanged("GuideId");
                }
            }
        }
        private bool tourStart;
        public bool TourStart
        {
            get { return tourStart; }
            set
            {
                if (tourStart != value)
                {
                    tourStart = value;
                    OnPropertyChanged("TourStart");
                }
            }
        }

        private bool tourEnd;
        public bool TourEnd
        {
            get { return tourEnd; }
            set
            {
                if (tourEnd != value)
                {
                    tourEnd = value;
                    OnPropertyChanged("TourEnd");
                }
            }
        }

        private uint activeKeyPoint;
        public uint ActiveKeyPoint
        {
            get { return activeKeyPoint; }
            set
            {
                if (activeKeyPoint != value)
                {
                    activeKeyPoint = value;
                    OnPropertyChanged("ActiveKeyPoint");
                }
            }
        }

        private uint currentFreeSlots;
        public uint CurrentFreeSlots
        {
            get { return currentFreeSlots; }
            set
            {
                if (currentFreeSlots != value)
                {
                    currentFreeSlots = value;
                    OnPropertyChanged("CurrentFreeSlots");
                }
            }
        }

        private Tour startTour;
        public Tour StartTour
        {
            get { return startTour; }
            set
            {
                if (startTour != value)
                {
                    startTour = value;
                    OnPropertyChanged("StartTour");
                }
            }
        }

        private User selectedGuest;
        public User SelectedGuest
        {
            get { return selectedGuest; }
            set
            {
                if(selectedGuest != value)
                {
                    selectedGuest = value;
                    OnPropertyChanged("SelectedGuest");
                }
            }
        }



        public GuideWindow()
        {
            InitializeComponent();
            DataContext = this;

            if (TourState.Tours.CheckGuideTours(GlobalState.CurrentUser.Id))
            {

                TourGuests = new ObservableCollection<User>(TourState.TourReservations.GetAllUsersOnTour(TourState.Tours.GetGuidesActiveTour(GlobalState.CurrentUser.Id).Id));
                NotifyPropertyChanged("TourGuests");
            }

        }

        private void CreateTourClick(object sender, RoutedEventArgs e)
        {
            if (GuideId < 0 || GuestMax < 0 || ActiveKeyPoint < 0 || CurrentFreeSlots < 0)
            {
                MessageBox.Show("Incorrect integer input :)");
                return;
            }
            if (ImageURLs == null)
            {
                MessageBox.Show("Incorrect integer input :)");
            }

            List<string> images = ImageURLs.Split("^").ToList() ?? new List<string>();
            List<DateTime> startdate = StartDate.Split(",").Select(date => DateTime.Parse(date)).ToList() ?? new List<DateTime>();
            List<string> keypoints = KeyPoints.Split(",").ToList() ?? new List<string>();

            if (TourState.Tours.Save(new Tour(GlobalState.CurrentUser.Id, TourName, City, Country, Description, TourLanguage, GuestMax, keypoints, startdate, Duration, images)) != default(Tour))
            {
                MessageBox.Show("Tour saved.");
                return;
            }
            MessageBox.Show("Input validation failed.");
        }

        private void StartTourClick(object sender, RoutedEventArgs e)
        {

            if (TourState.Tours.StartTour(StartTour.Id, GlobalState.CurrentUser.Id) != null) {
                MessageBox.Show("Tour started.");
                TourGuests = new ObservableCollection<User>(TourState.TourReservations.GetAllUsersOnTour(TourState.Tours.GetGuidesActiveTour(GlobalState.CurrentUser.Id).Id));
                NotifyPropertyChanged("TourGuests");
                return;
            }
            MessageBox.Show("Unable to start toure");

        }

        

        private void EndTourClick(object sender, RoutedEventArgs e)
        {

            if (TourState.Tours.EndTour(StartTour.Id, GlobalState.CurrentUser.Id))
            {
                MessageBox.Show("Tour ended");
                TourGuests = new ObservableCollection<User>();
                NotifyPropertyChanged("TourGuests");

                return;
            }
            MessageBox.Show("Incorrect integer input :)");
            return;
        }


        public ObservableCollection<User> TourGuests { get; set; }
        


       
        private void InviteGuestClick(object sender, RoutedEventArgs e)
        {
            if (TourState.TourReservations.InviteUser(TourState.Tours.GetGuidesActiveTour(GlobalState.CurrentUser.Id).Id, SelectedGuest.Id))
            {
                MessageBox.Show("Guest invited");
                return;
            }
            MessageBox.Show("Cant invite guest");
            return;
        }


        


        public List<Tour> todayTours= TourState.Tours.GetTodayTours(GlobalState.CurrentUser.Id).ToList();
        public List<Tour> TodayTours
        {
            get { return todayTours; }
        }

        public List<User> usersOnActiveTour = TourState.TourReservations.GetAllUsersOnTour(TourState.Tours.GetGuidesActiveTour(GlobalState.CurrentUser.Id).Id);

        public List<User> UsersOnActiveTour
        {
            get { return usersOnActiveTour; }
        }
 
    }
}
