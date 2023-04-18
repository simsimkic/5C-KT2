using SIMS_Projekat.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SIMS_Projekat.Repository
{
    public class TourReservationRepository : Repository<TourReservation>
    {

        public TourReservationRepository() : base("../../../Resources/Data/toursreservations.csv") { }

        public List<User> GetAllUsersOnTour(uint tourId)
        {
            return GetAll().Where(tourReservation => tourReservation.TourId == tourId && DateOnly.FromDateTime(tourReservation.StartDate) == DateOnly.FromDateTime(DateTime.Now)).Select(tourReservation => tourReservation.User).ToList();

        }

        public bool InviteUser(uint tourReservationId, uint userId)
        {
           

            TourReservation tourReservation = GetById(tourReservationId);

            //Trace.WriteLine(GetAllUsersOnTour(tourReservation.TourId).Exists(x => x.Id == userId));
            //Trace.WriteLine(IsUserInvited(userId, tourReservationId) == false);
            
            if (GetAllUsersOnTour(tourReservation.TourId).Exists(x=> x.Id==userId )&& IsUserInvited(userId, tourReservationId) == false)
            {

                TourInvitation tourInvitation = new TourInvitation(tourReservationId, userId, TourState.Tours.GetById(tourReservation.TourId).ActiveKeyPoint);
                TourState.TourInvitations.Save(tourInvitation);
                return true;
            }
            return false;

        }

        public bool IsUserInvited(uint userId, uint tourReservationId)
        {
           return TourState.TourInvitations.GetAll().Where(tourInvitation => tourInvitation.UserId == userId && tourInvitation.TourReservationId == tourReservationId).Any();
        }

        // Returns 0 if invalid param, -1 if not enough slots, -2 if tour has no valid date, 1 if successful
        public int ScheduleReservation(uint tourId, uint guestId, uint numberOfGuests)
        {
            if (numberOfGuests > 0)
            {
                Tour tour = TourState.Tours.GetById(tourId);
                Trace.WriteLine(tourId);
                if (tour == null)
                    return -3;
                if (tour.CurrentFreeSlots >= numberOfGuests)
                {
                    if (tour.StartDate.Find(date => date > DateTime.Now) == default(DateTime))
                    {
                        return -2;
                    }
                    TourReservation tourReservation = new TourReservation(guestId, numberOfGuests, tourId, tour.StartDate.Find(date => date > DateTime.Now));
                    Save(tourReservation);
                    tour.CurrentFreeSlots -= numberOfGuests;
                    TourState.Tours.Update(tour);
                    return 1;
                }
                else
                {
                    return -1;
                }
            }

            return 0;

        }

        public List<Tour> GetAlternateTours(uint tourId, uint numberOfGuests)
        {
            string selectedCity = TourState.Tours.GetById(tourId).City;
            List<Tour> AlternativeTours = TourState.Tours.GetAll().Where(x => x.City == selectedCity && x.Id != tourId && x.StartDate.Where(x => x > DateTime.Now).Any() && x.CurrentFreeSlots >= numberOfGuests).ToList();
            return AlternativeTours;
        }
    }
}
