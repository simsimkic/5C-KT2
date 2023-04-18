using SIMS_Projekat.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Diagnostics.Metrics;
using System.Xml.Linq;

namespace SIMS_Projekat.Model
{
    public class AccommodationReservation : ISerializable
    {
        public uint Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public uint GuestId { get; set; }
        public uint AccommodationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public uint NumberOfStayDays { get; set; }
        public uint NumberOfGuests { get; set; }
        public bool OwnerRated { get; set; }
        public bool GuestRated { get; set; }
        public User? Guest { get; set; }
        public Accommodation? Accommodation { get; set; }
        public User? Owner => Accommodation?.Owner;
        public uint OwnerId => Owner.Id;

        public AccommodationReservation() { }

        public AccommodationReservation(uint guestId, uint accommodationId, DateTime startDate, DateTime endDate, uint numberOfStayDays, uint numberOfGuests)
        {
            GuestId = guestId;
            AccommodationId = accommodationId;
            StartDate = startDate;
            EndDate = endDate;
            NumberOfStayDays = numberOfStayDays;
            NumberOfGuests = numberOfGuests;
            OwnerRated = false;
            GuestRated = false;
            Guest = GlobalState.Users.GetById(GuestId);
            Accommodation = AccommodationState.Accommodations.GetById(AccommodationId);
        }
        public string[] ToCSV()
        {
            if (GuestId==0 || AccommodationId==0 || NumberOfGuests==0 || NumberOfStayDays==0)
                return null;
            string[] csvValues = { Id.ToString(), IsDeleted.ToString(), GuestId.ToString(), AccommodationId.ToString(), StartDate.ToString(), EndDate.ToString(), NumberOfStayDays.ToString(), NumberOfGuests.ToString(), OwnerRated.ToString(), GuestRated.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToUInt32(values[0]);
            IsDeleted = Convert.ToBoolean(values[1]);
            GuestId = Convert.ToUInt32(values[2]);
            AccommodationId = Convert.ToUInt32(values[3]);
            StartDate = Convert.ToDateTime(values[4]);
            EndDate = Convert.ToDateTime(values[5]);
            NumberOfStayDays = Convert.ToUInt32(values[6]);
            NumberOfGuests = Convert.ToUInt32(values[7]);
            OwnerRated = Convert.ToBoolean(values[8]);
            GuestRated = Convert.ToBoolean(values[9]);
            Guest = GlobalState.Users.GetById(GuestId);
            Accommodation = AccommodationState.Accommodations.GetById(AccommodationId);
        }
    }
}