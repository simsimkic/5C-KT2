using SIMS_Projekat.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat.Model
{
    public class OwnerRating:ISerializable
    {
        public uint Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public uint GuestId => Guest.Id;
        public uint OwnerId => Owner.Id;
        public uint ReservationId { get; set; }
        public AccommodationReservation Reservation { get; set; }
        public User Guest => Reservation.Guest;
        public User Owner => Reservation.Owner;
        public uint CleanlinessRating { get; set; }
        public uint CorrectnessRating { get; set; }
        public string Comment { get; set; }
        public List<string> ImageURLs { get; set; } = new List<string>();
        public bool Rated { get; set; }
        public double AverageRating => (double)(CleanlinessRating + CorrectnessRating) / 2;

        public OwnerRating()
        {
            Rated = false;
        }
        public OwnerRating(uint reservationId)
        {
            ReservationId = reservationId;
            Reservation = AccommodationState.AccommodationReservations.GetById(ReservationId);
        }

        public string[] ToCSV()
        {
            if (OwnerId == 0 || GuestId == 0 || ReservationId == 0)
            {
                return null;
            }
            string[] csvValues = { Id.ToString(), IsDeleted.ToString(), ReservationId.ToString(), CleanlinessRating.ToString(), CorrectnessRating.ToString(), Comment, string.Join('^', ImageURLs), Rated.ToString()};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToUInt32(values[0]);
            IsDeleted = Convert.ToBoolean(values[1]);
            ReservationId = Convert.ToUInt32(values[2]);
            CleanlinessRating = Convert.ToUInt32(values[3]);
            CorrectnessRating = Convert.ToUInt32(values[4]);
            Comment = values[5];
            ImageURLs = values[6].Split('^').ToList();
            Rated = bool.Parse(values[7]);
            Reservation = AccommodationState.AccommodationReservations.GetById(ReservationId);
        }
    }
}
