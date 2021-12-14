using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TLH.Entity
{
    public class PScore
    {
        public Guid Id { get; set; }

        public int Score { get; set; }

        public int Place { get; set; }

        public Guid TournamentId { get; set; }

        public Tournament Tournament { get; set; }

        public Guid TeamId { get; set; }

        public Team Team { get; set; }

        public Guid DayOfTId { get; set; }

        public DayOfT DayOfT { get; set; }

        public Guid MatchId { get; set; }

        public Match Match { get; set; }
    }
}
