using SIMS_Projekat.Serializer;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace SIMS_Projekat.Model
{
    public enum Role { OWNER, GUIDE, USER }
    public class User : ISerializable, INotifyPropertyChanged
    {
        public uint Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Username { get; set; }
        public string Password { get; set; }
        public Role Role { get; set; }
        private bool superUser { get; set; } = false;
        public bool SuperUser
        {
            get { return superUser; }
            set
            {
                if (value != superUser)
                {
                    superUser = value;
                    OnPropertyChanged(nameof(SuperUser));
                }
            }
        }
        public User() { }
        public User(string username, string password, Role role)
        {
            Username = username;
            Password = password;
            Role = role;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(), IsDeleted.ToString(), Username, Password, Role.ToString(), SuperUser.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToUInt32(values[0]);
            IsDeleted = Convert.ToBoolean(values[1]);
            Username = values[2];
            Password = values[3];
            Role = (Role)Enum.Parse(typeof(Role), values[4]);   // string to Role enum conversion
            SuperUser = Convert.ToBoolean(values[5]);
        }
    }
}
