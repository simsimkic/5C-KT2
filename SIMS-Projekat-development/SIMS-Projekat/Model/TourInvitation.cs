using SIMS_Projekat.Serializer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat.Model
{
    public class TourInvitation : ISerializable
    {
        public uint Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public uint TourReservationId { get; set; }
        public uint UserId { get; set; }
        public bool Accepted { get; set; }
        public uint AcceptedDate { get; set; }

        public TourInvitation(uint tourReservationId, uint userId, uint acceptedDate)
        {
            TourReservationId = tourReservationId;
            UserId = userId;
            Accepted = false;
            AcceptedDate = acceptedDate;
        }

        public TourInvitation()
        {
            Accepted = false;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToUInt32(values[0]);
            IsDeleted = Convert.ToBoolean(values[1]);
            TourReservationId = Convert.ToUInt32(values[2]);
            UserId = Convert.ToUInt32(values[3]);
            Accepted = Convert.ToBoolean(values[4]);
            AcceptedDate = Convert.ToUInt32(values[5]);

        }

        public string[] ToCSV()
        {
            if (TourReservationId == 0 || UserId == 0) return null;
            string[] csvValues = {

                Id.ToString(),
                IsDeleted.ToString(),
                TourReservationId.ToString(),
                UserId.ToString(),
                Accepted.ToString(),
                AcceptedDate.ToString()


            };
            return csvValues;
        }
    }
}
