using SIMS_Projekat.Model;
using SIMS_Projekat.Repository;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;

namespace SIMS_Projekat.Service
{
    public class AccommodationReservationService : Service<AccommodationReservation>
    {
        public AccommodationReservationService() : base("../../../Resources/Data/reservations.csv")
        {

        }
        public List<IEnumerable<DateTime>> GetRecommendedDates(List<IEnumerable<DateTime>> freeDates, uint numberOfStayDays)
        {
            List<IEnumerable<DateTime>> recommendedDates = new List<IEnumerable<DateTime>>();
            foreach (IEnumerable<DateTime> dates in freeDates)
            {
                bool isRecommendable = true;
                foreach (DateTime date in dates)
                {
                    if (DateTime.Now > date && date.AddDays(numberOfStayDays) > dates.Last())
                    {
                        isRecommendable = false;
                        break;
                    }
                }
                if (isRecommendable)
                {
                    recommendedDates.Add(dates);
                    if (recommendedDates.Count() == 3)
                        break;
                }
            }
            return recommendedDates;
        }
        public List<IEnumerable<DateTime>> GetAllFreeDates(List<DateTime> startDays, List<DateTime> endDays, uint numberOfStayDays)
        {
            int n = startDays.Count();
            startDays.Sort();
            endDays.Sort();
            List<IEnumerable<DateTime>> freeDates = new List<IEnumerable<DateTime>>();
            freeDates.Add(GetEachDay(startDays[0].AddDays(-numberOfStayDays), startDays[0].AddDays(-1)));
            for (int i = 0; i < n - 1; i++)
            {
                if ((int)(startDays[i + 1].AddDays(-1) - endDays[i].AddDays(1)).TotalDays + 1 >= numberOfStayDays)
                    freeDates.Add(GetEachDay(endDays[i].AddDays(1), startDays[i + 1].AddDays(-1)));
            }
            freeDates.Add(GetEachDay(endDays[n - 1].AddDays(1), endDays[n - 1].AddDays(numberOfStayDays)));
            return freeDates;
        }
        public bool IsDateFree(List<IEnumerable<DateTime>> reservedDates, IEnumerable<DateTime> pickedDates)
        {
            foreach (IEnumerable<DateTime> reserved in reservedDates)
            {
                foreach (DateTime picked in pickedDates)
                {
                    if (reserved.Contains(picked))
                        return false;
                }
            }
            return true;
        }
        public IEnumerable<DateTime> GetEachDay(DateTime from, DateTime thru)
        {
            for (var day = from.Date; day.Date <= thru.Date; day = day.AddDays(1))
            {
                yield return day;
            }
        }

        public bool IsReservationValid(uint selectedAccommodationId, DateTime selectedStartDay, DateTime selectedEndDay, uint numberOfGuests, uint numberOfStayDays)
        {
            if (selectedStartDay < DateTime.Now || selectedEndDay < DateTime.Now)
            {
                MessageBox.Show("Start and end day cannot be in past!");
                return false;
            }
            if (selectedEndDay < selectedStartDay)
            {
                MessageBox.Show("Start day must be before end day!");
                return false;
            }
            if (numberOfStayDays < 1)
            {
                MessageBox.Show("Number of stay days must be greater than 0!");
                return false;
            }
            if (numberOfGuests < 1)
            {
                MessageBox.Show("Number of guests must be greater than 0!");
                return false;
            }
            if (numberOfGuests > AccommodationState.Accommodations.GetById(selectedAccommodationId).GuestMax)
            {
                MessageBox.Show("Number of guests must be lower or equal to maximum number of guests!");
                return false;
            }
            if (numberOfStayDays < AccommodationState.Accommodations.GetById(selectedAccommodationId).MinReservationDays)
            {
                MessageBox.Show("Number of stay days must be greater than minimum reservation days!");
                return false;
            }
            if (numberOfStayDays > (selectedEndDay - selectedStartDay).TotalDays + 1)
            {
                MessageBox.Show("Number of stay days must be greater than difference between end and start day!");
                return false;
            }
            return true;
        }

        public void ReserveAccommodation(uint selectedAccommodationId, DateTime selectedStartDay, DateTime selectedEndDay, uint numberOfGuests, uint numberOfStayDays, uint guestId)
        {
            IEnumerable<DateTime> pickedDates = GetEachDay(selectedStartDay, selectedEndDay);
            List<IEnumerable<DateTime>> reservedDates = new List<IEnumerable<DateTime>>();
            List<DateTime> startDays = new List<DateTime>();
            List<DateTime> endDays = new List<DateTime>();
            bool reservationForSelectedAccommodationAlreadyExists = false;

            if (!IsReservationValid(selectedAccommodationId, selectedStartDay, selectedEndDay, numberOfGuests, numberOfStayDays))
            {
                return;
            }

            if (AccommodationState.AccommodationReservations.GetAll() == null)
            {
                AccommodationState.AccommodationReservations.Save(new AccommodationReservation(guestId, selectedAccommodationId, selectedStartDay, selectedStartDay.AddDays(numberOfStayDays - 1), numberOfStayDays, numberOfGuests));
                MessageBox.Show("First reservation ever approved!");
                return;
            }

            foreach (AccommodationReservation reservation in AccommodationState.AccommodationReservations.GetAll())
            {
                if (reservation.AccommodationId == selectedAccommodationId)
                {
                    startDays.Add(reservation.StartDate);
                    endDays.Add(reservation.EndDate);
                    reservedDates.Add(GetEachDay(reservation.StartDate, reservation.EndDate));
                    reservationForSelectedAccommodationAlreadyExists = true;
                }
            }

            if (!reservationForSelectedAccommodationAlreadyExists)
            {
                AccommodationState.AccommodationReservations.Save(new AccommodationReservation(guestId, selectedAccommodationId, selectedStartDay, selectedStartDay.AddDays(numberOfStayDays - 1), numberOfStayDays, numberOfGuests));
                MessageBox.Show("First reservation for this accommodation approved!");
                return;
            }

            if (IsDateFree(reservedDates, pickedDates))
            {
                AccommodationState.AccommodationReservations.Save(new AccommodationReservation(guestId, selectedAccommodationId, selectedStartDay, selectedStartDay.AddDays(numberOfStayDays - 1), numberOfStayDays, numberOfGuests));
                MessageBox.Show("Your reservation is approved!");
                return;
            }
            else
            {
                string message = "\n";
                List<IEnumerable<DateTime>> freeDates = GetAllFreeDates(startDays, endDays, numberOfStayDays);
                foreach (var dates in GetRecommendedDates(freeDates, numberOfStayDays))
                {
                    message += dates.First().ToShortDateString().ToString() + " - " + dates.Last().ToShortDateString().ToString() + "\n";
                }
                MessageBox.Show("Reservation already exists in this date range, pick any date range from down below! " + message);
                return;
            }
        }

        public bool IsCancellationValid(AccommodationReservation selectedReservation)
        {
            if (!(selectedReservation.StartDate > DateTime.Now))
            {
                MessageBox.Show("You can only cancel reservations that are in future!");
                return false;
            }
            if ((selectedReservation.StartDate - DateTime.Now).TotalDays < selectedReservation.Accommodation.ReservationCancellationCutoffDays || (selectedReservation.StartDate - DateTime.Now).TotalDays <= 1)
            {
                MessageBox.Show("No more time for cancellation!");
                return false;
            }
            return true;
        }

        public void CancelReservation(AccommodationReservation selectedReservation)
        {
            if (IsCancellationValid(selectedReservation))
            {
                foreach (ReservationPostponement reservationPostponement in AccommodationState.ReservationPostponements.GetAll())
                {
                    if (reservationPostponement.AccommodationReservation.Id == selectedReservation.Id)
                    {
                        AccommodationState.ReservationPostponements.Delete(reservationPostponement);
                    }
                }
                AccommodationState.AccommodationReservations.Delete(selectedReservation);
                MessageBox.Show("Reservation sucessfully canceled!");
            }
        }

        public bool GetAccommodationAvailability(ReservationPostponement reservationPostponement)
        {
            return !GetAll().Any(ReservationFilterMethod);

            bool ReservationFilterMethod(AccommodationReservation reservation)
            {
                return reservation.Id != reservationPostponement.AccommodationReservation.Id && reservation.AccommodationId == reservationPostponement.Accommodation.Id && DoDateSpansOverlap(reservation.StartDate, reservation.EndDate, reservationPostponement.StartDate, reservationPostponement.EndDate);
            }
        }


        private bool DoDateSpansOverlap(DateTime start1, DateTime end1, DateTime start2, DateTime end2)
        {
            if (start1 < end2 && start2 < end1)
            {
                return true;
            }
            return false;
        }
    }
}
