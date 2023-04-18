using SIMS_Projekat.Model;
using SIMS_Projekat.Repository;
using SIMS_Projekat.View;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace SIMS_Projekat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _username;
        public string Username
        {
            get => _username;
            set
            {
                if (value != _username)
                {
                    _username = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void SignIn(object sender, RoutedEventArgs e)
        {
            if (GlobalState.Users.Login(Username, txtPassword.Password))
            {
                User user = GlobalState.Users.GetByUsername(Username);
                GlobalState.CurrentUser = user;

                switch (user.Id)
                {
                    case 1:
                        OwnerWindow ownerWindow = new OwnerWindow();
                        ownerWindow.Show();
                        Close();
                        break;
                    case 2:
                        User1Window user1Window = new User1Window(user.Id);
                        user1Window.Show();
                        Close();
                        break;
                    case 3:
                        GuideWindow guideWindow = new GuideWindow();
                        guideWindow.Show();
                        Close();
                        break;
                    case 4:
                        User2Window user2Window = new User2Window();
                        user2Window.Show();
                        Close();
                        break;
                }
                return;
            }
            MessageBox.Show("Incorrect information.");
        }

    }
}