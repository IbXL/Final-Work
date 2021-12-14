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
    public class PrizesController : Controller
    {
        private readonly DataContext _context;

        public PrizesController(DataContext context)
        {
            _context = context;
        }

        // GET: Prizes
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.Prizes.Include(p => p.Tournament);
            return View(await dataContext.ToListAsync());
        }

        // GET: Prizes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prize = await _context.Prizes
                .Include(p => p.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prize == null)
            {
                return NotFound();
            }

            return View(prize);
        }

        // GET: Prizes/Create
        public IActionResult Create()
        {
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name");
            return View();
        }

        // POST: Prizes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Amount,Placement,TournamentId")] Prize prize)
        {
            if (ModelState.IsValid)
            {
                _context.Add(prize);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", prize.TournamentId);
            return View(prize);
        }

        // GET: Prizes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prize = await _context.Prizes.FindAsync(id);
            if (prize == null)
            {
                return NotFound();
            }
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", prize.TournamentId);
            return View(prize);
        }

        // POST: Prizes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Amount,Placement,TournamentId")] Prize prize)
        {
            if (id != prize.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prize);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrizeExists(prize.Id))
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
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", prize.TournamentId);
            return View(prize);
        }

        // GET: Prizes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prize = await _context.Prizes
                .Include(p => p.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prize == null)
            {
                return NotFound();
            }

            return View(prize);
        }

        // POST: Prizes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prize = await _context.Prizes.FindAsync(id);
            _context.Prizes.Remove(prize);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrizeExists(int id)
        {
            return _context.Prizes.Any(e => e.Id == id);
        }
    }
}
