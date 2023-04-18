using SIMS_Projekat.Model;
using SIMS_Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;

namespace SIMS_Projekat
{
    public class ReservationTimer
    {
        private Timer Timer;

        public ReservationTimer()
        {
            Timer = new Timer(600000);
            Timer.AutoReset = true;
            Timer.Enabled = true;
            Timer.Elapsed += OnTimedEvent;
            HandleReservations();
        }
        private static void OnTimedEvent(Object? source, ElapsedEventArgs e)
        {
            HandleReservations();
        }

        private static void HandleReservations()
        {
            List<AccommodationReservation> reservations = AccommodationState.AccommodationReservations.GetAll().Where(x => !x.GuestRated).ToList();
            foreach (var reservation in reservations)
            {
                if (DateTime.Compare(reservation.EndDate, DateTime.Now) < 0)
                {
                    reservation.GuestRated = true;
                    AccommodationState.GuestRatings.Save(new GuestRating(reservation.Id));
                    AccommodationState.AccommodationReservations.Update(reservation);
                }
            }

            List<GuestRating> unratedGuestRatings = AccommodationState.GuestRatings.GetAll().Where(x => !x.Rated).ToList();
            foreach (var guestRating in unratedGuestRatings)
            {
                if ((DateTime.Now - guestRating.Reservation.EndDate).TotalDays > 5)
                {
                    AccommodationState.GuestRatings.Delete(guestRating);
                }
            }

            List<AccommodationReservation> unratedOwnerReservations = AccommodationState.AccommodationReservations.GetAll().Where(x => !x.OwnerRated).ToList();
            foreach (var reservation in unratedOwnerReservations)
            {
                if (DateTime.Compare(reservation.EndDate, DateTime.Now) < 0)
                {
                    reservation.OwnerRated = true;
                    AccommodationState.OwnerRatings.Save(new OwnerRating(reservation.Id));
                    AccommodationState.AccommodationReservations.Update(reservation);
                }
            }

            List<OwnerRating> unratedOwnerRatings = AccommodationState.OwnerRatings.GetAll().Where(x => !x.Rated).ToList();
            foreach (var ownerRating in unratedOwnerRatings)
            {
                if ((DateTime.Now - ownerRating.Reservation.EndDate).TotalDays > 5)
                {
                    AccommodationState.OwnerRatings.Delete(ownerRating);
                }
            }
        }
    }
}
