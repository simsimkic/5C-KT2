using SIMS_Projekat.Model;
using SIMS_Projekat.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;

namespace SIMS_Projekat.Service
{
    public class GuestRatingService : Service<GuestRating>
    {
        public GuestRatingService() : base("../../../Resources/Data/guestratings.csv")
        {
        }

        public Result<GuestRating> RateGuest(GuestRating guestRating, uint cleanlinessRating, uint complianceRating, uint communicationRating, uint respectForPropertyRating, uint demeanorRating, uint complaintsRating, string comment)
        {
            Result<GuestRating> result = new Result<GuestRating>();

            if (GlobalState.CurrentUser.Role != Role.OWNER || !IsRatingValid(cleanlinessRating, complianceRating, communicationRating, respectForPropertyRating, demeanorRating, complaintsRating, comment))
            {
                result.Message = "Invalid rating.";
                return result;
            }
            if (guestRating == null)
            {
                result.Message = "Guest rating doesn't exist.";
                return result;
            }
            if (guestRating.Rated)
            {
                result.Message = "Guest already rated.";
                return result;
            }
            guestRating.CleanlinessRating = cleanlinessRating;
            guestRating.ComplianceRating = complianceRating;
            guestRating.CommunicationRating = communicationRating;
            guestRating.RespectForPropertyRating = respectForPropertyRating;
            guestRating.DemeanorRating = demeanorRating;
            guestRating.ComplaintsRating = complaintsRating;
            guestRating.Comment = comment;
            guestRating.Rated = true;
            AccommodationState.OwnerRatings.SetSuperOwnerStatus();

            result.ReturnValue = Update(guestRating);
            result.Success = true;
            result.Message = "Guest rated.";
            return result;
        }

        public bool HasPendingRatings(User owner)
        {
            foreach (var rating in GetAll())
            {
                if (rating.OwnerId == owner.Id && !rating.Rated) return true;
            }
            return false;
        }

        public List<GuestRating> GetUsersToRate()
        {
            return GetAll().Where(x => x.OwnerId == GlobalState.CurrentUser.Id && !x.Rated).ToList();
        }

        private bool IsRatingValid(uint cleanlinessRating, uint complianceRating, uint communicationRating, uint respectForPropertyRating, uint demeanorRating, uint complaintsRating, string comment)
        {
            bool isCleanlinessRatingValid = cleanlinessRating < 6 && cleanlinessRating > 0;
            bool isComplianceRatingValid = complianceRating < 6 && complianceRating > 0;
            bool isCommunicationRatingValid = communicationRating < 6 && communicationRating > 0;
            bool isRespectForPropertyRatingValid = respectForPropertyRating < 6 && respectForPropertyRating > 0;
            bool isDemeanorRatingValid = demeanorRating < 6 && demeanorRating > 0;
            bool isComplaintsRatingValid = complaintsRating < 6 && complaintsRating > 0;
            return isCleanlinessRatingValid && isComplianceRatingValid && isCommunicationRatingValid && isRespectForPropertyRatingValid && isDemeanorRatingValid && isComplaintsRatingValid && !string.IsNullOrWhiteSpace(comment);
        }
    }
}
