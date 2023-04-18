using SIMS_Projekat.Model;
using SIMS_Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat.Service
{
    public class AccommodationService : Service<Accommodation>
    {
        public AccommodationService() : base("../../../Resources/Data/accommodations.csv")
        {

        }
        public IEnumerable<Accommodation> SearchAccommodations(string name, string county, string city, AccommodationType selectedType, uint numberOfGuests, uint numberOfStayDays)
        {
            return AccommodationState.Accommodations.GetAll().Where(x => x.Name.ToLower().Contains(name.ToLower()) && x.Country.ToLower().Contains(county.ToLower()) && x.City.ToLower().Contains(city.ToLower()) && x.Type == selectedType && x.GuestMax >= numberOfGuests && x.MinReservationDays >= numberOfStayDays);

        }
    }
}
