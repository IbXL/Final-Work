using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TLH.Entity
{
    public class Player
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Comment { get; set; }

        public int Merit { get; set; }

        public List<KScore> KScores { get; set; }

        public List<PT> PTs { get; set; }
    }

    public class PT
    {
        public Guid Id { get; set; }

        public Guid TournamentId { get; set; }

        public virtual Tournament Tournament { get; set; }

        public Guid PlayerId { get; set; }

        public virtual Player Player { get; set; }

        public Guid TeamId { get; set; }

        public virtual Team Team { get; set; }

        public bool IsSub { get; set; }

        public string Role { get; set; }
    }
}
