using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TLH.Models.PT
{
    public class PTIndexViewModel
    {
        public Guid? TeamId { get; set; }

        public Guid? TournamentId { get; set; }

        public List<PTViewModel> PTs { get; set; }
    }

    public class PTViewModel
    { 
        public string TeamName { get; set; }

        public string TournamentName { get; set; }

        public string PlayerName { get; set; }

        public bool IsSub { get; set; }

        public string Role { get; set; }

        public Guid Id { get; set; }
    }
}
