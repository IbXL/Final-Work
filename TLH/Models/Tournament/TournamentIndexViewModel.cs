using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TLH.Models.Tournament
{
    public class TournamentIndexViewModel
    {
        public List<TournamentViewModel> Tournaments { get; set; }
    }

    public class TournamentViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int TeamCount { get; set; }
    }
}
