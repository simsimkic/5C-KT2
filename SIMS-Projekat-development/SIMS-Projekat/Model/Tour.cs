using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIMS_Projekat.Serializer;

namespace SIMS_Projekat.Model
{
    public class Tour : ISerializable
    {
        public uint Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public uint GuideId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public uint GuestMax { get; set; }
        public List<string> KeyPoints { get; set; }
        public List<DateTime> StartDate { get; set; }
        public uint Duration { get; set; }
        public List<string> ImageURLs { get; set; }
        public bool TourStart { get; set; }
        public bool TourEnd { get; set; }
        public uint ActiveKeyPoint { get; set; }
        public uint CurrentFreeSlots { get; set; }

        public Tour() { }

        public Tour(uint guideId, string name, string city, string country, string description, string language, uint guestMax, List<string> keyPoints, List<DateTime> startDate, uint duration, List<string> imageURLs)
        {

            Name = name;
            City = city;
            Country = country;
            Description = description;
            Language = language;
            GuestMax = guestMax;
            KeyPoints = keyPoints;
            StartDate = startDate;
            Duration = duration;
            ImageURLs = imageURLs;
            GuideId = guideId;
            TourStart = false;
            TourEnd = false;
            ActiveKeyPoint = 0;
            CurrentFreeSlots = guestMax;

        }

        public string[] ToCSV()
        {
            if (String.IsNullOrWhiteSpace(Name) || String.IsNullOrWhiteSpace(City) || String.IsNullOrWhiteSpace(Country) || String.IsNullOrWhiteSpace(Description) || String.IsNullOrWhiteSpace(Language) || GuestMax == 0 || KeyPoints.Count < 2 || StartDate.Count == 0 || Duration == 0)
                return null;
            string[] csvValues = {

                Id.ToString(),
                IsDeleted.ToString(),
                Name,
                City,
                Country,
                Description,
                Language,
                GuestMax.ToString(),
                string.Join(',' ,KeyPoints),
                string.Join (',' , StartDate),
                Duration.ToString(),
                string.Join('^', ImageURLs),
                GuideId.ToString(),
                TourStart.ToString(),
                TourEnd.ToString(),
                ActiveKeyPoint.ToString(),
                CurrentFreeSlots.ToString()

            };
            return csvValues;
        }

        public void FromCSV(string[] values)
        {
            Id = Convert.ToUInt32(values[0]);
            IsDeleted = Convert.ToBoolean(values[1]);
            Name = values[2];
            City = values[3];
            Country = values[4];
            Description = values[5];
            Language = values[6];
            GuestMax = Convert.ToUInt32(values[7]);
            KeyPoints = values[8].Split(',').ToList();
            StartDate = values[9].Split(',').Select(date => DateTime.Parse(date)).ToList();
            Duration = Convert.ToUInt32(values[10]);
            ImageURLs = values[11].Split('^').ToList();
            GuideId = Convert.ToUInt32(values[12]);
            TourStart = Convert.ToBoolean(values[13]);
            TourEnd = Convert.ToBoolean(values[14]);
            ActiveKeyPoint = Convert.ToUInt32(values[15]);
            CurrentFreeSlots = Convert.ToUInt32(values[16]);

        }
    }


}

