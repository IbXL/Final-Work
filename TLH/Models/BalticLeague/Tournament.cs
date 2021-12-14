using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TLH.Models.BalticLeague
{
    public class TournamentModel
    {
        public Guid TournamentId { get; set; }
        public Guid TeamId { get; set; }
        public string TeamName { get; set; }

        public List<DayModel> Days { get; set; }

        public int TotalKills { get; set; }

        public int TotalScore { get; set; }

        public string TournamentName { get; set; }
    }
    public class TeamModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class DayModel
    { 
        public Guid Id { get; set; }
        public string Name { get; set; }

        public int TotalScore { get; set; }
    }


    public class PlayerModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int TotalKills { get; set; }

        public string Roles { get; set; }

        public string TeamName { get; set; }

        public bool IsSub { get; set; }

        public int Merit { get; set; }
    }

    public class PlayersViewModel
    { 
        public Guid TournamentId { get; set; }

        public List<PlayerModel> Players { get; set; }
    }

    public class TeamDetailsViewModel
    { 
        public Guid Id { get; set; }

        public string TourName { get; set; }

        public Guid TourId { get; set; }

        public string Name { get; set; }

        public int Merit { get; set; }

        public int TotalKills { get; set; }

        public List<PlayerModel> Players { get; set; }

        public int TotalPlacement { get; set; }

        public int TTK { get; set; }

        public int TTP { get; set; }

        public int AvarageKill { get; set; }

        public int AvaragePlacement { get; set; }

        public List<TDayViewModel> Days { get; set; }
    }
    public class PlayerDetailsViewModel
    {
        public Guid Id { get; set; }

        public string TourName { get; set; }

        public string Name { get; set; }

        public int Merit { get; set; }

        public int TotalKills { get; set; }

        public int TTK { get; set; }

        public int AvarageKill { get; set; }

        public List<TDayViewModel> Days { get; set; }
    }

    public class TDayViewModel
    { 
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int PScore { get; set; }

        public int KScore { get; set; }

        public List<TMatchViewModel> Matches { get; set; }
    }

    public class TMatchViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int PScore { get; set; }

        public int KScore { get; set; }
    }
}
