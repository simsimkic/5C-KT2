using SIMS_Projekat.Serializer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat.Model
{
    public enum AccommodationType
    {
        APARTMENT,
        HOUSE,
        COTTAGE
    }
    public class Accommodation : ISerializable
    {
        public  uint Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public AccommodationType Type { get; set; }
        public uint GuestMax { get; set; }
        public uint MinReservationDays { get; set; }
        public uint ReservationCancellationCutoffDays { get; set; }    //Broj dana pred rezervaciju do kog je moguće otkazati rezervaciju
        public List<string> ImageURLs { get; set; }
        public uint OwnerId { get; set; }
        public User Owner { get; set; }

        public Accommodation() { }

        public Accommodation(string name, string city, string country, AccommodationType type, uint guestMax, uint minReservationDays, uint reservationCancellationCutoffDays, List<string> imageURLs, uint ownerId)
        {
            Name = name;
            City = city;
            Country = country;
            Type = type;
            GuestMax = guestMax;
            MinReservationDays = minReservationDays;
            ReservationCancellationCutoffDays = reservationCancellationCutoffDays;
            ImageURLs = imageURLs;
            OwnerId = ownerId;
            Owner = GlobalState.Users.GetById(ownerId);
        }

        public string[] ToCSV()
        {
            User owner = GlobalState.Users.GetById(OwnerId);
            if(owner == null) { return null; }
            if(String.IsNullOrWhiteSpace(Name) || String.IsNullOrWhiteSpace(City) || String.IsNullOrWhiteSpace(Country) || GuestMax==0 || MinReservationDays==0 || ReservationCancellationCutoffDays==0 || owner.Role!=Role.OWNER)
            {
                return null;
            }
            string[] csvValues = { Id.ToString(), IsDeleted.ToString(), Name, City, Country, Type.ToString(), GuestMax.ToString(), MinReservationDays.ToString(), ReservationCancellationCutoffDays.ToString(), string.Join('^', ImageURLs), OwnerId.ToString() };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToUInt32(values[0]);
            IsDeleted = Convert.ToBoolean(values[1]);
            Name = values[2];
            City = values[3];
            Country = values[4];
            Type = (AccommodationType)Enum.Parse(typeof(AccommodationType), values[5]);   // string to AccommodationType enum conversion
            GuestMax = Convert.ToUInt32(values[6]);
            MinReservationDays = Convert.ToUInt32(values[7]);
            ReservationCancellationCutoffDays = Convert.ToUInt32(values[8]);
            ImageURLs = values[9].Split('^').ToList();
            OwnerId = Convert.ToUInt32(values[10]);
            Owner = GlobalState.Users.GetById(OwnerId);
        }
    }
}
