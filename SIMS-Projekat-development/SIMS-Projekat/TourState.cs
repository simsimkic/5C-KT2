using SIMS_Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat
{
    public static class TourState
    {
        public static TourRepository Tours = new();
        public static TourReservationRepository TourReservations = new();
        public static TourInvitationRepository TourInvitations = new();
    }
}
