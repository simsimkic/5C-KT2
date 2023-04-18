using SIMS_Projekat.Model;
using SIMS_Projekat.Repository;
using SIMS_Projekat.Serializer;
using System.Collections.Generic;
using System.Linq;

namespace SIMS_Projekat.Service
{
    public class UserService : Service<User>
    {
        public UserService() : base("../../../Resources/Data/users.csv")
        {

        }

        public User? GetByUsername(string username)
        {
            return _repository.GetAll().FirstOrDefault(u => u.Username == username);
        }

        public bool Login(string username, string password)
        {
            return GetByUsername(username)?.Password == password;
        }
    }
}
