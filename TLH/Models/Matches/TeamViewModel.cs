using System;
using System.Collections.Generic;

namespace TLH.Models.Matches
{
    public class TeamsViewModel
    {
        public  Guid MId { get; set; }
        public List<TeamViewModel> Teams { get; set; }

    }

    public class TeamViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public int KScores { get; set; }

        public int PScores { get; set; }

        public int Merit { get; set; }
    }

    public class AddTeamViewModel
    { 
        public Guid MatchId { get; set; }
        public int Placement { get; set; }

        public Guid TeamId { get; set; }

        public List<PlayerViewModel> Players { get; set; }
    }

    public class PlayerViewModel
    { 
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Kills { get; set; }
    }
}
