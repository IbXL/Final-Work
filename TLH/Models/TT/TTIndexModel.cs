using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TLH.Models.TT
{
    public class TTIndexModel
    {
        public Guid? TournamentId { get; set; }

        public List<TTViewModel> TTs { get; set; }
    }

    public class TTViewModel
    { 
        public string TeamName { get; set; }

        public Guid TeamId { get; set; }

        public string TournamentName { get; set; }

        public Guid Id { get; set; }

        public int Players { get; set; }
    }
}
