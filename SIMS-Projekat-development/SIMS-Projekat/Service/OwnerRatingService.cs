using SIMS_Projekat.Model;
using SIMS_Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIMS_Projekat.Service
{
    public class OwnerRatingService : Service<OwnerRating>
    {
        public OwnerRatingService() : base("../../../Resources/Data/ownerratings.csv")
        {
        }

        public Result<OwnerRating> RateOwner(OwnerRating ownerRating, uint cleanlinessRating, uint correctnessRating, string comment, string imgURLs)
        {
            Result<OwnerRating> result = new Result<OwnerRating>();

            if (GlobalState.CurrentUser.Role != Role.USER || !IsRatingValid(cleanlinessRating, correctnessRating, comment))
            {
                result.Message = "Invalid rating.";
                return result;
            }
            if (ownerRating == null)
            {
                result.Message = "Owner rating doesn't exist.";
                return result;
            }
            if (ownerRating.Rated)
            {
                result.Message = "Owner already rated.";
                return result;
            }
            ownerRating.CleanlinessRating = cleanlinessRating;
            ownerRating.CorrectnessRating = correctnessRating;
            ownerRating.Comment = comment;
            ownerRating.ImageURLs = imgURLs.Split('^').ToList();
            ownerRating.Rated = true;
            SetSuperOwnerStatus();

            result.ReturnValue = Update(ownerRating);
            result.Success = true;
            result.Message = "Owner rated.";
            return result;
        }

        public bool HasPendingRatings(User guest)
        {
            foreach (var rating in GetAll())
            {
                if (rating.GuestId == guest.Id && !rating.Rated) return true;
            }
            return false;
        }

        private bool IsRatingValid(uint cleanlinessRating, uint correctnessRating, string comment)
        {
            bool isCleanlinessRatingValid = cleanlinessRating < 6 && cleanlinessRating > 0;
            bool isCorrectnessRatingValid = correctnessRating < 6 && correctnessRating > 0;
            return isCleanlinessRatingValid && isCorrectnessRatingValid && !string.IsNullOrWhiteSpace(comment);
        }

        public List<OwnerRating> GetAllVisibleRatings() // Mozda bolje cuvati u modelu da li je vidljiva ocena?
        {
            return GetAll()
                .Where(ownerRating => ownerRating.OwnerId == GlobalState.CurrentUser.Id)
                .Where(ownerRating => AccommodationState.GuestRatings.GetAll()
                    .Where(guestRating => guestRating.ReservationId == ownerRating.ReservationId && guestRating.Rated && guestRating.OwnerId == ownerRating.OwnerId)
                    .Select(guestRating => guestRating.ReservationId)
                    .Contains(ownerRating.ReservationId)
                    ).ToList();
        }

        public double GetAverageOwnerRating()
        {
            return GetAllVisibleRatings()
                .Select(ownerRating => ownerRating.AverageRating)
                .DefaultIfEmpty(0)
                .Average();
        }

        public bool SetSuperOwnerStatus()
        {
            if (GetAverageOwnerRating() > 4.5 && GetAllVisibleRatings().Count() >= 5)
            {
                if (!GlobalState.CurrentUser.SuperUser)
                {
                    GlobalState.CurrentUser.SuperUser = true;
                    GlobalState.Users.Update(GlobalState.CurrentUser);
                }
                return true;
            }
            if (GlobalState.CurrentUser.SuperUser)
            {
                GlobalState.CurrentUser.SuperUser = false;
                GlobalState.Users.Update(GlobalState.CurrentUser);
            }
            return false;
        }
    }
}
