using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLH.Entity;
using TLH.Models.BalticLeague;

namespace TLH.Controllers
{
    public class BalticLeagueController : Controller
    {
        private readonly DataContext _context;
        public BalticLeagueController(DataContext context)
        {
            _context = context;
        }
        public DayModel Map(DayOfT dayOfTs,Guid tId)
        {
            return new DayModel
            {
                Name = dayOfTs.Name,
                Id = dayOfTs.Id,
                TotalScore = dayOfTs.KScores != null && dayOfTs.PScores != null ? dayOfTs.KScores.Where(k => k.TeamId == tId).Select(k => k.Count).Sum() + dayOfTs.PScores.Where(k => k.TeamId == tId).Select(k => k.Score).Sum() : 0
            };
        }

        public TournamentModel Map(Team n,Guid Id,Tournament tour)
        {
            return new TournamentModel
            {
                TeamId = n.Id,
                TeamName = n.Name,
                TournamentId = Id,
                Days = tour.DayOfTs != null ? tour.DayOfTs.Select(d => Map(d,n.Id)).OrderBy(t => t.Name).ToList() : new List<DayModel>(),
                TotalKills = n.KScores != null ? n.KScores.Select(k => k.Count).Sum() : 0,
                TotalScore = n.KScores != null && n.PScores != null ? n.PScores.Select(k => k.Score).Sum() + n.KScores.Select(k => k.Count).Sum() : 0
            };
        }
        public TournamentModel Map(Tournament tour)
        {
            return new TournamentModel
            {
                TournamentId = tour.Id,
                TournamentName = tour.Name
            };
        }
        public PlayersViewModel Map(List<Player> players,Guid Id)
        {
            return new PlayersViewModel
            {
                TournamentId = Id,
                Players = players.Select(p => Map(p,p.PTs.First(pt => pt.TournamentId == Id),Id)).OrderByDescending(o => o.TotalKills).ToList()
            };
        }

        public PlayerModel Map(Player p,PT pT,Guid Id)
        {
            return new PlayerModel
            {
                Id = p.Id,
                Name = p.Name,
                Roles = pT.Role,
                IsSub = pT.IsSub,
                TeamName = pT.Team.Name,
                TotalKills = p.KScores.Where(k => k.TournamentId == Id).Select(k => k.Count).Sum(),
                Merit = p.Merit
            };
        }
        public PlayerModel Map(Player p)
        {
            return new PlayerModel
            {
                Id = p.Id,
                Name = p.Name,
                TotalKills = p.KScores != null ? p.KScores.Select(k => k.Count).Sum() : 0
            };
        }

        public TeamDetailsViewModel Map(Team t,Guid id)
        {
            return new TeamDetailsViewModel
            { 
                Id = t.Id,
                Name = t.Name,
                Merit = t.Merit,
                TourId = id,
                TourName = t.Tournaments.First(t => t.TournamentId == id).Tournament.Name,
                TTK = t.KScores != null ? t.KScores.Where(k => k.TournamentId == id).Select(k => k.Count).Sum() : 0,
                TTP = t.PScores != null ? t.PScores.Where(k => k.TournamentId == id).Select(p => p.Score).Sum() : 0,
                TotalKills = t.KScores != null ? t.KScores.Select(k => k.Count).Sum() : 0,
                TotalPlacement = t.PScores != null ? t.PScores.Select(p => p.Score).Sum() : 0,
                Days = t.Tournaments.First(s => s.TournamentId == id).Tournament.DayOfTs.Select(d => Map(d)).OrderBy(d => d.Name).ToList(),
                Players = t.Players.Select(p => Map(p.Player)).ToList()
            };
        }
        public PlayerDetailsViewModel Map(Player t, Guid id)
        {
            return new PlayerDetailsViewModel
            {
                Id = t.Id,
                Name = t.Name,
                Merit = t.Merit,
                TourName = t.PTs.First(t => t.TournamentId == id).Tournament.Name,
                TTK = t.KScores != null ? t.KScores.Where(k => k.TournamentId == id).Select(k => k.Count).Sum() : 0,
                TotalKills = t.KScores != null ? t.KScores.Select(k => k.Count).Sum() : 0,
                Days = t.PTs.First(s => s.TournamentId == id).Tournament.DayOfTs.Select(d => Map(d)).OrderBy(d => d.Name).ToList()
            };
        }

        public TDayViewModel Map(DayOfT d)
        {
            return new TDayViewModel
            {
                Id = d.Id,
                KScore = d.KScores != null ? d.KScores.Select(k => k.Count).Sum() : 0,
                PScore = d.PScores != null ? d.PScores.Select(k => k.Score).Sum() : 0,
                Name = d.Name,
                Matches = d.Matches != null ? d.Matches.Select(m => Map(m)).OrderBy(d => d.Name).ToList() : new List<TMatchViewModel>()
            };
        }

        public TMatchViewModel Map(Match m)
        {
            return new TMatchViewModel
            {
                Id = m.Id,
                KScore = m.KScores != null ? m.KScores.Select(k => k.Count).Sum() : 0,
                PScore = m.PScores != null ? m.PScores.Select(k => k.Score).Sum() : 0,
                Name = m.Name
            };
        }

        public IActionResult Index(Guid Id)
        {
            IndexBL viewModel = new IndexBL();
            viewModel.Id = Id;
            return View(viewModel);
        }

        public IActionResult ListOfTournaments()
        {
            var tournaments = _context.Tournaments.ToList();
            ListOfTournaments viewModel = new ListOfTournaments();
            viewModel.Tournaments = tournaments.Select(s => Map(s)).ToList();
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Scoreboard(Guid Id)
        {
            var tour = _context.Tournaments.First(t => t.Id == Id);
            var teams = _context.Teams.Include(t => t.Tournaments).ThenInclude(t => t.Tournament).ThenInclude(t => t.DayOfTs).ThenInclude(t => t.KScores).Include(t => t.PScores).Where(t => t.Tournaments.Select(d => d.TournamentId).Any(d => d == Id)).ToList();
            var prizes = _context.Prizes.Where(p => p.TournamentId == Id).ToList();
            var viewModel = new ListOfTournaments();
            viewModel.Tournaments = teams.Select(n => Map(n,Id,tour)).OrderByDescending(o => o.TotalScore).ToList();
            viewModel.Prizes = prizes;
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Teams()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Players(Guid Id)
        {
            var players = _context.Players.Include(p => p.KScores).Include(p => p.PTs).ThenInclude(p => p.Team).Where(p => p.PTs.Any(s => s.TournamentId == Id)).ToList();
            var viewModel = Map(players, Id);
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> TeamDetails(Guid? id,Guid tId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.Include(t => t.Players).ThenInclude(t =>t.Player).Include(t => t.KScores)
                .Include(t => t.PScores).Include(t => t.Tournaments).ThenInclude(t =>t.Tournament).ThenInclude(t => t.DayOfTs).ThenInclude(t => t.Matches)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (team == null)
            {
                return NotFound();
            }
            var viewModel = Map(team, tId);
            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
        [HttpGet]
        public async Task<IActionResult> PlayerDetails(Guid? id, Guid tId)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.Include(t => t.KScores)
                .Include(t => t.PTs).ThenInclude(t => t.Tournament).ThenInclude(t => t.DayOfTs).ThenInclude(t => t.Matches)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }
            var viewModel = Map(player, tId);
            if (viewModel == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }
    }
}
