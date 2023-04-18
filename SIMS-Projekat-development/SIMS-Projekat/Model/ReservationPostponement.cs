using SIMS_Projekat.Serializer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat.Model
{
    public enum PostponementApproval
    {
        APPROVED,
        DENIED,
        PENDING
    }
    public class ReservationPostponement : ISerializable
    {
        public uint Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public uint AccommodationReservationId { get; set; }
        public AccommodationReservation AccommodationReservation { get; set; }
        public Accommodation Accommodation => AccommodationReservation.Accommodation;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PostponementApproval Approval { get; set; } = PostponementApproval.PENDING;
        public bool IsDateAlreadyReserved { get; set; }
        public string Reasoning { get; set; } = "";
        public ReservationPostponement() { }

        public ReservationPostponement(uint accommodationReservationId, DateTime startDate, DateTime endDate)
        {
            AccommodationReservationId = accommodationReservationId;
            StartDate = startDate;
            EndDate = endDate;
            AccommodationReservation = AccommodationState.AccommodationReservations.GetById(AccommodationReservationId);
            IsDateAlreadyReserved = AccommodationState.AccommodationReservations.GetAccommodationAvailability(this);
        }

        public string[] ToCSV()
        {
            string[] csvValues = { Id.ToString(),IsDeleted.ToString(), AccommodationReservationId.ToString(), StartDate.ToString(), EndDate.ToString(), Approval.ToString(), Reasoning };
            return csvValues;
        }
        public void FromCSV(string[] values)
        {
            Id = Convert.ToUInt32(values[0]);
            IsDeleted = Convert.ToBoolean(values[1]);
            AccommodationReservationId = Convert.ToUInt32(values[2]);
            StartDate = Convert.ToDateTime(values[3]);
            EndDate = Convert.ToDateTime(values[4]);
            Approval = (PostponementApproval)Enum.Parse(typeof(PostponementApproval), values[5]);
            Reasoning = values[6];
            AccommodationReservation = AccommodationState.AccommodationReservations.GetById(AccommodationReservationId);
        }

        public void Approve()
        {
            Approval = PostponementApproval.APPROVED;
            AccommodationReservation.StartDate = StartDate;
            AccommodationReservation.EndDate = EndDate;
            AccommodationState.AccommodationReservations.Update(AccommodationReservation);
        }
        public void Reject(string reasoning)
        {
            Reasoning = reasoning;
            Approval = PostponementApproval.DENIED;
        }
    }
}
