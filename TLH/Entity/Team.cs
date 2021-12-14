using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TLH.Entity
{
    public class Team
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<PT> Players { get; set; }

        public List<TT> Tournaments { get; set; }

        public List<TeamAchivement> Achivements { get; set; }

        public List<KScore> KScores { get; set; }

        public List<PScore> PScores { get; set; }

        public int Merit { get; set; }
    }

    public class TT
    {
        public Guid Id { get; set; }

        public Guid TournamentId { get; set; }

        public virtual Tournament Tournament { get; set; }

        public Guid TeamId { get; set; }

        public virtual Team Team { get; set; }
    }

    public class ScoreSystem
    { 
        public int Id { get; set; }

        public int Place { get; set; }

        public int Score { get; set; }

    }

    public class TeamAchivement
    { 
        public Guid Id { get; set; }

        public Guid TournamentId { get; set; }

        public virtual Tournament Tournament { get; set; }

        public Guid TeamId { get; set; }

        public virtual Team Team { get; set; }

        public int Placement { get; set; }
    }
}
