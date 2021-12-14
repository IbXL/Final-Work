using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TLH;
using TLH.Entity;
using TLH.Models.Matches;

namespace TLH.Controllers
{
    [Authorize]
    public class MatchesController : Controller
    {
        private readonly DataContext _context;

        public MatchesController(DataContext context)
        {
            _context = context;
        }

        public TeamsViewModel Map(List<Team> teams,Guid id,Guid Id)
        {
            return new TeamsViewModel
            {
                MId = id,
                Teams = teams.Select(t => Map(t,Id)).ToList()
            };
        }

        public TeamViewModel Map(Team team,Guid id)
        {
            return new TeamViewModel
            {
                Id = team.Id,
                Merit = team.Merit,
                Name = team.Name,
                KScores = team.KScores != null ? team.KScores.Where(k => k.MatchId == id).Select(t => t.Count).Sum() : 0,
                PScores = team.PScores != null ? team.PScores.Where(k => k.MatchId == id).Select(t => t.Score).Sum() : 0
            };
        }

        public AddTeamViewModel Map(Team team,Guid id,Guid tId,Guid tmId)
        {
            return new AddTeamViewModel
            {
                MatchId = id,
                TeamId = tmId,
                Placement = team.PScores != null && team.PScores.Where(p => p.MatchId == id).Count() > 0 ? team.PScores.First(p => p.MatchId == id).Place : 0,
                Players = team.Players != null ? team.Players.Where(p => p.TournamentId == tId).Select(p => Map(p.Player,id)).ToList() : null
            };
        }

        public PlayerViewModel Map(Player player,Guid id)
        {
            return new PlayerViewModel
            {
                Id = player.Id,
                Name = player.Name,
                Kills = player.KScores != null && player.KScores.Where(k => k.MatchId == id).Count() > 0 ? player.KScores.First(k => k.MatchId == id).Count : 0
            };
        }

        // GET: Matches
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Matchs.Include(m => m.DayOfT).Include(m => m.Tournament);
            return View(await dataContext.ToListAsync());
        }

        // GET: Matches/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matchs
                .Include(m => m.DayOfT)
                .Include(m => m.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }
        [Authorize(Roles = "IbX")]
        // GET: Matches/Create
        public IActionResult Create()
        {
            ViewData["DayOfTId"] = new SelectList(_context.DayOfTs, "Id", "Name");
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name");
            return View();
        }
        [Authorize(Roles = "IbX")]
        // POST: Matches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DayOfTId,TournamentId,Name")] Match match)
        {
            if (ModelState.IsValid)
            {
                match.Id = Guid.NewGuid();
                _context.Add(match);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DayOfTId"] = new SelectList(_context.DayOfTs, "Id", "Name", match.DayOfTId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", match.TournamentId);
            return View(match);
        }
        [Authorize(Roles = "IbX")]
        // GET: Matches/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matchs.FindAsync(id);
            if (match == null)
            {
                return NotFound();
            }
            ViewData["DayOfTId"] = new SelectList(_context.DayOfTs, "Id", "Id", match.DayOfTId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Id", match.TournamentId);
            return View(match);
        }
        [Authorize(Roles = "IbX")]
        // POST: Matches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,DayOfTId,TournamentId,Name")] Match match)
        {
            if (id != match.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(match);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MatchExists(match.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DayOfTId"] = new SelectList(_context.DayOfTs, "Id", "Id", match.DayOfTId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Id", match.TournamentId);
            return View(match);
        }
        [Authorize(Roles = "IbX")]
        // GET: Matches/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var match = await _context.Matchs
                .Include(m => m.DayOfT)
                .Include(m => m.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }
        [Authorize(Roles = "IbX")]
        // POST: Matches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var match = await _context.Matchs.FindAsync(id);
            _context.Matchs.Remove(match);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "IbX")]
        private bool MatchExists(Guid id)
        {
            return _context.Matchs.Any(e => e.Id == id);
        }

        [HttpGet]
        public IActionResult GetTeams(Guid id)
        {
            var match = _context.Matchs.First(m => m.Id == id);
            var teams = _context.Teams.Include(t => t.PScores).Include(t => t.KScores).ThenInclude(t => t.Match).Where(t => t.Tournaments.Any(ts => ts.TournamentId == match.TournamentId)).ToList();
            var viewModel = Map(teams,id,match.Id);
            return View(viewModel);
        }
        [HttpGet]
        public IActionResult EditTeam(Guid mId,Guid tmId)
        {
            var tId = _context.Matchs.First(m => m.Id == mId).TournamentId;
            var team = _context.Teams.Include(t => t.KScores).Include(t => t.PScores).Include(t => t.Players).ThenInclude(t => t.Player).First(t => t.Id == tmId);
            var viewModel = Map(team,mId,tId,tmId);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult EditTeam(AddTeamViewModel viewModel)
        {
            if (viewModel.MatchId == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var match = _context.Matchs.First(t => t.Id == viewModel.MatchId);
                    var pS = _context.PScores.Where(p => p.MatchId == viewModel.MatchId && p.TeamId == viewModel.TeamId);
                    var ss = _context.ScoreSystems.ToList();
                    if (pS.Count() == 0)
                    {
                        PScore pScore = new PScore();
                        pScore.Id = Guid.NewGuid();
                        pScore.MatchId = viewModel.MatchId;
                        pScore.DayOfTId = match.DayOfTId;
                        pScore.Place = viewModel.Placement;
                        pScore.Score = ss.Where(s => s.Place == viewModel.Placement).Count() > 0 ? ss.First(s => s.Place == viewModel.Placement).Score : 0 ;
                        pScore.TeamId = viewModel.TeamId;
                        pScore.TournamentId = match.TournamentId;
                        _context.PScores.Add(pScore);
                    }
                    else
                    {
                        PScore pSc = pS.First();
                        pSc.Place = viewModel.Placement;
                        pSc.Score = ss.Where(s => s.Place == viewModel.Placement).Count() > 0 ? ss.First(s => s.Place == viewModel.Placement).Score : 0;
                        _context.PScores.Update(pSc);
                    }
                    var kS = _context.KScores.Where(p => p.MatchId == viewModel.MatchId && p.TeamId == viewModel.TeamId);
                    foreach (var pl in viewModel.Players)
                    {
                        if (kS == null || kS.Where(k => k.PlayerId == pl.Id).Count() == 0)
                        {
                            KScore kScore = new KScore();
                            kScore.Id = Guid.NewGuid();
                            kScore.MatchId = viewModel.MatchId;
                            kScore.DayOfTId = match.DayOfTId;
                            kScore.Count = pl.Kills;
                            kScore.TeamId = viewModel.TeamId;
                            kScore.TournamentId = match.TournamentId;
                            kScore.PlayerId = pl.Id;
                            _context.KScores.Add(kScore);
                        }
                        else
                        {
                            KScore kSc = kS.First(l => l.PlayerId == pl.Id);
                            kSc.Count = pl.Kills;
                            _context.KScores.Update(kSc);
                        }
                    }
                    _context.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
                return RedirectToAction(nameof(GetTeams),new { id = viewModel.MatchId});
            }
            return View(viewModel);
        }
    }
}
