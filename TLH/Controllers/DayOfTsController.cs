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
    public class DayOfTsController : Controller
    {
        private readonly DataContext _context;

        public DayOfTsController(DataContext context)
        {
            _context = context;
        }

        // GET: DayOfTs
        public async Task<IActionResult> Index()
        {
            var dataContext = _context.DayOfTs.Include(d => d.Tournament);
            return View(await dataContext.ToListAsync());
        }

        // GET: DayOfTs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dayOfT = await _context.DayOfTs
                .Include(d => d.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dayOfT == null)
            {
                return NotFound();
            }

            return View(dayOfT);
        }

        [Authorize(Roles = "IbX")]
        // GET: DayOfTs/Create
        public IActionResult Create()
        {
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name");
            return View();
        }

        // POST: DayOfTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "IbX")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TournamentId,Name")] DayOfT dayOfT)
        {
            if (ModelState.IsValid)
            {
                dayOfT.Id = Guid.NewGuid();
                _context.Add(dayOfT);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", dayOfT.TournamentId);
            return View(dayOfT);
        }
        [Authorize(Roles = "IbX")]
        // GET: DayOfTs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dayOfT = await _context.DayOfTs.FindAsync(id);
            if (dayOfT == null)
            {
                return NotFound();
            }
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", dayOfT.TournamentId);
            return View(dayOfT);
        }
        [Authorize(Roles = "IbX")]
        // POST: DayOfTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TournamentId,Name")] DayOfT dayOfT)
        {
            if (id != dayOfT.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dayOfT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DayOfTExists(dayOfT.Id))
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
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", dayOfT.TournamentId);
            return View(dayOfT);
        }
        [Authorize(Roles = "IbX")]
        // GET: DayOfTs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dayOfT = await _context.DayOfTs
                .Include(d => d.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dayOfT == null)
            {
                return NotFound();
            }

            return View(dayOfT);
        }
        [Authorize(Roles = "IbX")]
        // POST: DayOfTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var dayOfT = await _context.DayOfTs.FindAsync(id);
            _context.DayOfTs.Remove(dayOfT);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "IbX")]
        private bool DayOfTExists(Guid id)
        {
            return _context.DayOfTs.Any(e => e.Id == id);
        }
    }
}
