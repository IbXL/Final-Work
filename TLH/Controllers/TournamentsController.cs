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
using TLH.Models.Tournament;

namespace TLH.Controllers
{
    [Authorize]
    public class TournamentsController : Controller
    {
        private readonly DataContext _context;

        public TournamentsController(DataContext context)
        {
            _context = context;
        }

        public TournamentIndexViewModel Map(List<Tournament> tournaments)
        {
            return new TournamentIndexViewModel
            {
                Tournaments = tournaments.Select(t => Mapper(t)).ToList()
            };
        }

        public TournamentViewModel Mapper(Tournament t)
        {
            return new TournamentViewModel
            {
                Id = t.Id,
                Name = t.Name,
                TeamCount = t.Teams.Count()
            };
        }

        // GET: Tournaments
        public async Task<IActionResult> Index()
        {
            var tournaments = await _context.Tournaments.Include(t => t.Teams).ToListAsync();
            var viewModel = Map(tournaments);
            return View(viewModel);
        }

        // GET: Tournaments/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        [Authorize(Roles = "IbX")]
        // GET: Tournaments/Create
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "IbX")]
        // POST: Tournaments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Tournament tournament)
        {
            if (ModelState.IsValid)
            {
                tournament.Id = Guid.NewGuid();
                _context.Add(tournament);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tournament);
        }

        [Authorize(Roles = "IbX")]
        // GET: Tournaments/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments.FindAsync(id);
            if (tournament == null)
            {
                return NotFound();
            }
            return View(tournament);
        }

        [Authorize(Roles = "IbX")]
        // POST: Tournaments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name")] Tournament tournament)
        {
            if (id != tournament.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tournament);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TournamentExists(tournament.Id))
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
            return View(tournament);
        }

        [Authorize(Roles = "IbX")]
        // GET: Tournaments/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tournament = await _context.Tournaments
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tournament == null)
            {
                return NotFound();
            }

            return View(tournament);
        }

        [Authorize(Roles = "IbX")]
        // POST: Tournaments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tournament = await _context.Tournaments.FindAsync(id);
            _context.Tournaments.Remove(tournament);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "IbX")]
        private bool TournamentExists(Guid id)
        {
            return _context.Tournaments.Any(e => e.Id == id);
        }
    }
}
