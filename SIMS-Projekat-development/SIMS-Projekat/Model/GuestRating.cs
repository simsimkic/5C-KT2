using SIMS_Projekat.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat.Model
{
    public class GuestRating : ISerializable
    {
        public uint Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public uint OwnerId => Owner.Id;
        public uint GuestId => Guest.Id;
        public uint ReservationId { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public User? Owner => Reservation.Owner;
        public User? Guest => Reservation.Guest;
        public uint CleanlinessRating { get; set; }
        public uint ComplianceRating { get; set; }
        public uint CommunicationRating { get; set; }
        public uint RespectForPropertyRating { get; set; }
        public uint DemeanorRating { get; set; }
        public uint ComplaintsRating { get; set; }
        public string Comment { get; set; } = "";
        public bool Rated { get; set; } = false;

        public GuestRating()
        {
            Rated = false;
        }
        public GuestRating(uint reservationId)
        {
            ReservationId = reservationId;
            Reservation = AccommodationState.AccommodationReservations.GetById(ReservationId);
        }

        public string[] ToCSV()
        {
            if (ReservationId == 0)
            {
                return null;
            }
            string[] csvValues = { Id.ToString(), IsDeleted.ToString(), ReservationId.ToString(), CleanlinessRating.ToString(), ComplianceRating.ToString(), CommunicationRating.ToString(), ComplaintsRating.ToString(), DemeanorRating.ToString(), RespectForPropertyRating.ToString(), Comment, Rated.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToUInt32(values[0]);
            IsDeleted = Convert.ToBoolean(values[1]);
            ReservationId = Convert.ToUInt32(values[2]);
            CleanlinessRating = Convert.ToUInt32(values[3]);
            ComplianceRating = Convert.ToUInt32(values[4]);
            CommunicationRating = Convert.ToUInt32(values[5]);
            ComplaintsRating = Convert.ToUInt32(values[6]);
            DemeanorRating = Convert.ToUInt32(values[7]);
            RespectForPropertyRating = Convert.ToUInt32(values[8]);
            Comment = values[9];
            Rated = bool.Parse(values[10]);
            Reservation = AccommodationState.AccommodationReservations.GetById(ReservationId);

        }
    }
}
