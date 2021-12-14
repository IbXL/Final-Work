using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TLH.Entity
{
    public class Tournament
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public List<TT> Teams { get; set; }

        public List<PT> Players { get; set; }

        public List<KScore> KScores { get; set; }

        public List<PScore> PScores { get; set; }

        public List<DayOfT> DayOfTs { get; set; }

        public List<Prize> Prizes { get; set; }

        public List<TeamAchivement> Achivements { get; set; }

    }

    public class DayOfT
    { 
        public Guid Id { get; set; }

        public Guid TournamentId { get; set; }

        public string Name { get; set; }

        public virtual Tournament Tournament { get; set; }

        public List<Match> Matches { get; set; }

        public List<KScore> KScores { get; set; }

        public List<PScore> PScores { get; set; }

    }

    public class Match
    { 
        public Guid Id { get; set; }

        public Guid DayOfTId { get; set; }

        public Guid TournamentId { get; set; }

        public string Name { get; set; }

        public virtual Tournament Tournament { get; set; }

        public virtual DayOfT DayOfT { get; set; }

        public List<KScore> KScores { get; set; }

        public List<PScore> PScores { get; set; }
    }

    public class Prize
    { 
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public int Placement { get; set; }

        public Guid TournamentId { get; set; }

        public virtual Tournament Tournament { get; set; }

    }
}
