using SIMS_Projekat.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat.Repository
{
    public class TourRepository : Repository<Tour>
    {

        public TourRepository() : base("../../../Resources/Data/tours.csv") { }

        public List<string> StartTour(uint id, uint guideId)
        {
            Tour tour = GetById(id);
            if (!tour.TourStart && CheckGuideTours(guideId)==false && CheckIfTourIsToday(id)==true)
            {
                tour.TourStart = true;
                tour.GuideId = guideId;
                tour.ActiveKeyPoint = 0;
                Update(tour);
                return tour.KeyPoints;

            }
        //1. grad i drzava
        //2. jezik
        //3. trajanje ture

            return null;

        }


        public bool CheckGuideTours (uint guideId)
        {
           return  GetAll().Where(tour=> tour.GuideId == guideId && tour.TourEnd==false && tour.TourStart).Any();
        }

        public Tour GetGuidesActiveTour(uint guideId)
        {
            return GetAll().Where(tour => tour.GuideId == guideId && tour.TourEnd == false && tour.TourStart).ToList()[0];
        }


        public List<Tour> GetTodayTours(uint guideId)
        {
            return GetAll().Where(tour => CheckIfTourIsToday(tour.Id) && tour.GuideId==guideId).ToList();

        }

        public bool CheckIfTourIsToday (uint id)
        {
            return GetById(id).StartDate.Where(tourDate => DateOnly.FromDateTime(tourDate) == DateOnly.FromDateTime(DateTime.Now)).Any();
        }
 

        public void SetActiveKeyPoint(uint tourId, uint guideId, uint keyPointIndex)

        {

            Tour tour = GetById(tourId);
            if (tour.GuideId==guideId && tour.TourEnd==false && tour.KeyPoints.Count()>keyPointIndex && tour.TourStart)
            {
                tour.ActiveKeyPoint = keyPointIndex;
                Update(tour);
                if (tour.ActiveKeyPoint== tour.KeyPoints.Count()-1 )
                {
                    EndTour(tourId, guideId);

                }
            }
            
        }

        public bool EndTour(uint tourId, uint guideId )
        {
            Tour tour = GetById(tourId);
            if (tour.GuideId == guideId && tour.TourEnd == false && tour.TourStart==true)
            {
                tour.TourEnd = true;
                tour.ActiveKeyPoint = (uint)tour.KeyPoints.Count() - 1;
                tour.CurrentFreeSlots = tour.GuestMax;
                Update(tour);
                return true;

            }
            return false;


        }

        public IEnumerable<Tour> SearchTours(string city, string country, string language, uint guestNumber, uint duration)
        {

            if (city == null || country == null | language == null)
            {
                //Trace.WriteLine("Usao u if");
                return TourState.Tours.GetAll();
            }
            else
            {
                return TourState.Tours.GetAll().Where(x => x.City.ToLower().Contains(city.ToLower()) && x.Country.ToLower().Contains(country.ToLower()) && x.Language.ToLower().Contains(language.ToLower()) && guestNumber <= x.GuestMax && guestNumber > 0 && duration <= x.Duration && duration > 0);
            }
        }



    }
}
