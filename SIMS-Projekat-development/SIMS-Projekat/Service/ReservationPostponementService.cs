using SIMS_Projekat.Model;
using SIMS_Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SIMS_Projekat.Service
{
    public class ReservationPostponementService : Service<ReservationPostponement>
    {
        public ReservationPostponementService() : base("../../../Resources/Data/reservationpostponements.csv") { }

        public List<ReservationPostponement> GetAllRequestsForOwner()
        {
            return GetAll().Where(reservationPostponement => reservationPostponement.Accommodation.OwnerId == GlobalState.CurrentUser.Id && reservationPostponement.Approval == PostponementApproval.PENDING).ToList();
        }

        public Result<ReservationPostponement> HandlePostponement(ReservationPostponement reservationPostponement, bool isApproved, string reasoning)
        {
            Result<ReservationPostponement> result = new Result<ReservationPostponement>();

            if (reservationPostponement == null)
            {
                result.Message = "Invalid postponement.";
                return result;
            }
            result.ReturnValue = reservationPostponement;

            if (reservationPostponement.Approval != PostponementApproval.PENDING)
            {
                result.Message = $"Postponement already {reservationPostponement.Approval.ToString().ToLower()}.";
                return result;
            }

            if (isApproved)
            {
                reservationPostponement.Approve();
            }
            else
            {
                reservationPostponement.Reject(reasoning);
            }

            Update(reservationPostponement);
            result.Message = $"Successfully {(isApproved ? "approved" : "denied")} request.";
            result.Success = true;
            return result;
        }

        public bool IsPostponementValid(AccommodationReservation selectedAccommodationReservation, DateTime newStartDay, DateTime newEndDay)
        {
            if (selectedAccommodationReservation == null)
            {
                return false;
            }
            if (AccommodationState.ReservationPostponements.GetById(selectedAccommodationReservation.Id) != null)
            {
                MessageBox.Show("Postponement already exists!");
                return false;
            }
            if (newStartDay < DateTime.Now || newEndDay < DateTime.Now)
            {
                MessageBox.Show("Start and end day cannot be in past!");
                return false;
            }
            if (newEndDay < newStartDay)
            {
                MessageBox.Show("Start day must be before end day!");
                return false;
            }
            if (selectedAccommodationReservation.NumberOfStayDays != (newEndDay - newStartDay).TotalDays + 1)
            {
                MessageBox.Show("Number of stay days must stay the same!");
                return false;
            }
            return true;
        }

        public void CreatePostponement(AccommodationReservation selectedReservation, DateTime newStartDate, DateTime newEndDate)
        {
            if (IsPostponementValid(selectedReservation, newStartDate, newEndDate))
                AccommodationState.ReservationPostponements.Save(new ReservationPostponement(selectedReservation.Id, newStartDate, newEndDate));
        }
    }
}
