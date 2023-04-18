using SIMS_Projekat.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat.Model
{
    public class TourReservation : ISerializable
    {
        public uint Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public User? User { get; set; }
        public uint UserId { get; set; }
        public Tour? Tour { get; set; }
        public uint TourId { get; set; }
        public uint GuestNumber { get; set; }

        public DateTime StartDate { get; set; }

        //public int CurrentAvailableSlots { get; set; }
        public TourReservation() { }
        public TourReservation(uint userId, uint guestNumber, uint tourId, DateTime startDate)
        {
            UserId = userId;
            GuestNumber = guestNumber;
            //CurrentAvailableSlots = currentAvailableSlots;
            TourId = tourId;
            StartDate = startDate;
            User = GlobalState.Users.GetById(UserId);
            Tour = TourState.Tours.GetById(TourId);
        }
        public string[] ToCSV()

        {
            if (UserId == 0 || TourId == 0) return null;
            string[] csvValues = { Id.ToString(), IsDeleted.ToString(), UserId.ToString(), TourId.ToString(), GuestNumber.ToString(), StartDate.ToString()/*, CurrentAvailableSlots.ToString()*/};
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToUInt32(values[0]);
            IsDeleted = Convert.ToBoolean(values[1]);
            UserId = Convert.ToUInt32(values[2]);
            TourId = Convert.ToUInt32(values[3]);
            GuestNumber = Convert.ToUInt32(values[4]);
            StartDate = DateTime.Parse(values[5]);
            //CurrentAvailableSlots = Convert.ToInt32(values[3]);
            User = GlobalState.Users.GetById(UserId);
            Tour = TourState.Tours.GetById(TourId);

        }






    }
}
