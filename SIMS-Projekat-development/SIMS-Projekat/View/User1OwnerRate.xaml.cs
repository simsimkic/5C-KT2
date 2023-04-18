using SIMS_Projekat.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
using System.Windows.Shapes;

namespace SIMS_Projekat.View
{
    /// <summary>
    /// Interaction logic for User1OwnerRate.xaml
    /// </summary>
    public partial class User1OwnerRate : Window, INotifyPropertyChanged
    {
        public ObservableCollection<OwnerRating> OwnerRatings { get; set; }
        public OwnerRating SelectedOwnerRating { get; set; }

        private uint cleanlinessRating;
        public uint CleanlinessRating
        {
            get { return cleanlinessRating; }
            set
            {
                if (cleanlinessRating != value)
                {
                    cleanlinessRating = value;
                    OnPropertyChanged("CleanlinessRating");
                }
            }
        }

        private uint correctnessRating;
        public uint CorrectnessRating
        {
            get { return correctnessRating; }
            set
            {
                if (correctnessRating != value)
                {
                    correctnessRating = value;
                    OnPropertyChanged("CorrectnessRating");
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
                    OnPropertyChanged("Comment");
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
        public User1OwnerRate()
        {
            InitializeComponent();
            DataContext = this;
            OwnerRatings = new ObservableCollection<OwnerRating>(AccommodationState.OwnerRatings.GetAll().Where(x => x.GuestId == GlobalState.CurrentUser.Id && !x.Rated));
        }

        private void RateOwner(object sender, RoutedEventArgs e)
        {
            Result<OwnerRating> result = AccommodationState.OwnerRatings.RateOwner(SelectedOwnerRating,CleanlinessRating,CorrectnessRating,Comment, ImageURLs);
            MessageBox.Show(result.Message);
            OwnerRatings = new ObservableCollection<OwnerRating>(AccommodationState.OwnerRatings.GetAll().Where(x => x.GuestId == GlobalState.CurrentUser.Id && !x.Rated));
            NotifyPropertyChanged(nameof(OwnerRatings));
        }
    }
}
