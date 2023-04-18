using SIMS_Projekat.Model;
using SIMS_Projekat.Repository;
using SIMS_Projekat.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat
{
    public static class GlobalState
    {
        public static LocationRepository Locations = new();

        public static User CurrentUser;
        public static UserService Users = new();

    }
}
