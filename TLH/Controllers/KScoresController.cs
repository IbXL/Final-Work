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

namespace TLH.Controllers
{
    [Authorize]
    public class KScoresController : Controller
    {
        private readonly DataContext _context;

        public KScoresController(DataContext context)
        {
            _context = context;
        }

        // GET: KScores
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.KScores.Include(k => k.DayOfT).Include(k => k.Match).Include(k => k.Player).Include(k => k.Team).Include(k => k.Tournament);
            return View(await dataContext.ToListAsync());
        }

        // GET: KScores/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kScore = await _context.KScores
                .Include(k => k.DayOfT)
                .Include(k => k.Match)
                .Include(k => k.Player)
                .Include(k => k.Team)
                .Include(k => k.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kScore == null)
            {
                return NotFound();
            }

            return View(kScore);
        }
        [Authorize(Roles = "IbX")]
        // GET: KScores/Create
        public IActionResult Create()
        {
            ViewData["DayOfTId"] = new SelectList(_context.DayOfTs, "Id", "Name");
            ViewData["MatchId"] = new SelectList(_context.Matchs, "Id", "Name");
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Name");
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name");
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name");
            return View();
        }
        [Authorize(Roles = "IbX")]
        // POST: KScores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Count,TournamentId,TeamId,PlayerId,DayOfTId,MatchId")] KScore kScore)
        {
            if (ModelState.IsValid)
            {
                kScore.Id = Guid.NewGuid();
                _context.Add(kScore);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DayOfTId"] = new SelectList(_context.DayOfTs, "Id", "Name", kScore.DayOfTId);
            ViewData["MatchId"] = new SelectList(_context.Matchs, "Id", "Name", kScore.MatchId);
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Name", kScore.PlayerId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Id", kScore.TeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", kScore.TournamentId);
            return View(kScore);
        }
        [Authorize(Roles = "IbX")]
        // GET: KScores/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kScore = await _context.KScores.FindAsync(id);
            if (kScore == null)
            {
                return NotFound();
            }
            ViewData["DayOfTId"] = new SelectList(_context.DayOfTs, "Id", "Id", kScore.DayOfTId);
            ViewData["MatchId"] = new SelectList(_context.Matchs, "Id", "Id", kScore.MatchId);
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Id", kScore.PlayerId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Id", kScore.TeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Id", kScore.TournamentId);
            return View(kScore);
        }
        [Authorize(Roles = "IbX")]
        // POST: KScores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Count,TournamentId,TeamId,PlayerId,DayOfTId,MatchId")] KScore kScore)
        {
            if (id != kScore.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kScore);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KScoreExists(kScore.Id))
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
            ViewData["DayOfTId"] = new SelectList(_context.DayOfTs, "Id", "Id", kScore.DayOfTId);
            ViewData["MatchId"] = new SelectList(_context.Matchs, "Id", "Id", kScore.MatchId);
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Id", kScore.PlayerId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Id", kScore.TeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Id", kScore.TournamentId);
            return View(kScore);
        }
        [Authorize(Roles = "IbX")]
        // GET: KScores/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kScore = await _context.KScores
                .Include(k => k.DayOfT)
                .Include(k => k.Match)
                .Include(k => k.Player)
                .Include(k => k.Team)
                .Include(k => k.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kScore == null)
            {
                return NotFound();
            }

            return View(kScore);
        }
        [Authorize(Roles = "IbX")]
        // POST: KScores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var kScore = await _context.KScores.FindAsync(id);
            _context.KScores.Remove(kScore);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "IbX")]
        private bool KScoreExists(Guid id)
        {
            return _context.KScores.Any(e => e.Id == id);
        }
    }
}
