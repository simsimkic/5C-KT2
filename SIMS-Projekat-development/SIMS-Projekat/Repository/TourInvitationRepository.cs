using SIMS_Projekat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SIMS_Projekat.Repository
{
    public class TourInvitationRepository : Repository<TourInvitation>
    {
        public TourInvitationRepository() : base("../../../Resources/Data/toursinvitations.csv") { }


    }
}
