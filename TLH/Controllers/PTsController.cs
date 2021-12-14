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
using TLH.Models.PT;

namespace TLH.Controllers
{
    [Authorize]
    public class PTsController : Controller
    {
        private readonly DataContext _context;

        public PTsController(DataContext context)
        {
            _context = context;
        }


        public PTIndexViewModel Map(List<PT> pTs, Guid? teamId,Guid? tournamentId)
        {
            return new PTIndexViewModel
            {
                TournamentId = tournamentId,
                TeamId = teamId,
                PTs = pTs.Select(t => Mapper(t)).ToList()
            };
        }

        public PTViewModel Mapper(PT p)
        {
            return new PTViewModel
            {
                Id = p.Id,
                TeamName = p.Team.Name,
                TournamentName = p.Tournament.Name,
                PlayerName = p.Player.Name,
                IsSub = p.IsSub,
                Role = p.Role
            };
        }

        // GET: PTs
        public async Task<IActionResult> Index(Guid? teamId,Guid? tournamentId)
        {
            var dataContext = await _context.PTs.Include(p => p.Player).Include(p => p.Team).Include(p => p.Tournament).ToListAsync();
            if (teamId != null)
            {
                dataContext = dataContext.Where(d => d.TeamId == teamId).ToList();
            }
            if (tournamentId != null)
            {
                dataContext = dataContext.Where(d => d.TournamentId == tournamentId).ToList();
            }
            var viewModel = Map(dataContext, teamId, tournamentId);
            return View(viewModel);
        }

        // GET: PTs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pT = await _context.PTs
                .Include(p => p.Player)
                .Include(p => p.Team)
                .Include(p => p.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pT == null)
            {
                return NotFound();
            }

            return View(pT);
        }

        [Authorize(Roles = "IbX")]
        // GET: PTs/Create
        public IActionResult Create(Guid? teamId,Guid? tournamentId)
        {
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Name");
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name",teamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name",tournamentId);
            return View();
        }

        [Authorize(Roles = "IbX")]
        // POST: PTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TournamentId,PlayerId,TeamId,IsSub,Role")] PT pT)
        {
            if (ModelState.IsValid)
            {
                pT.Id = Guid.NewGuid();
                _context.Add(pT);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index),new { teamId = pT.TeamId, tournamentId = pT.TournamentId});
            }
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Name", pT.PlayerId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", pT.TeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", pT.TournamentId);
            return View(pT);
        }

        [Authorize(Roles = "IbX")]
        // GET: PTs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pT = await _context.PTs.FindAsync(id);
            if (pT == null)
            {
                return NotFound();
            }
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Name", pT.PlayerId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", pT.TeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", pT.TournamentId);
            return View(pT);
        }

        [Authorize(Roles = "IbX")]
        // POST: PTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TournamentId,PlayerId,TeamId,IsSub,Role")] PT pT)
        {
            if (id != pT.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(pT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PTExists(pT.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { teamId = pT.TeamId, tournamentId = pT.TournamentId });
            }
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Name", pT.PlayerId);
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", pT.TeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", pT.TournamentId);
            return View(pT);
        }

        [Authorize(Roles = "IbX")]
        // GET: PTs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pT = await _context.PTs
                .Include(p => p.Player)
                .Include(p => p.Team)
                .Include(p => p.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pT == null)
            {
                return NotFound();
            }

            return View(pT);
        }

        [Authorize(Roles = "IbX")]
        // POST: PTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var pT = await _context.PTs.FindAsync(id);
            _context.PTs.Remove(pT);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "IbX")]
        private bool PTExists(Guid id)
        {
            return _context.PTs.Any(e => e.Id == id);
        }
    }
}
