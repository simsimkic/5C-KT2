using SIMS_Projekat.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat
{
    public static class AccommodationState
    {
        public static AccommodationService Accommodations = new();
        public static AccommodationReservationService AccommodationReservations = new();
        public static GuestRatingService GuestRatings = new();
        public static OwnerRatingService OwnerRatings = new();
        public static ReservationPostponementService ReservationPostponements = new();

        public static ReservationTimer ReservationTimer = new();
    }
}
