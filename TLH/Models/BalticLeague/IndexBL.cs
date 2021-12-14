using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLH.Entity;

namespace TLH.Models.BalticLeague
{
    public class IndexBL
    {
        public Guid Id { get; set; }

        public List<TournamentModel> Tournaments { get; set; }
    }

    public class ListOfTournaments
    { 
        public List<TournamentModel> Tournaments { get; set; }

        public List<Prize> Prizes { get; set; }
    }
}
