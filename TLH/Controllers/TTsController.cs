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
using TLH.Models.TT;

namespace TLH.Controllers
{
    [Authorize]
    public class TTsController : Controller
    {
        private readonly DataContext _context;

        public TTsController(DataContext context)
        {
            _context = context;
        }

        public TTIndexModel Map(List<TT> tTs,Guid? id)
        {
            return new TTIndexModel
            {
                TournamentId = id,
                TTs = tTs.Select(t => Mapper(t)).ToList()
            };
        }

        public TTViewModel Mapper(TT t) 
        {
            return new TTViewModel
            {
                Id = t.Id,
                TeamName = t.Team.Name,
                TournamentName = t.Tournament.Name,
                Players = t.Team.Players.Count(),
                TeamId = t.TeamId
            };
        }

        // GET: TTs
        public async Task<IActionResult> Index(Guid? id)
        {
            var dataContext = await _context.TTs.Include(t => t.Team).ThenInclude(t => t.Players).Include(t => t.Tournament).ToListAsync();
            if (id != null)
            {
                dataContext = dataContext.Where(t => t.TournamentId == id).ToList();
            }
            var viewModel = Map(dataContext, id);
            return View(viewModel);
        }

        // GET: TTs/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tT = await _context.TTs
                .Include(t => t.Team)
                .Include(t => t.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tT == null)
            {
                return NotFound();
            }

            return View(tT);
        }

        [Authorize(Roles = "IbX")]
        // GET: TTs/Create
        public IActionResult Create(Guid? id)
        {
            if (id != null)
            {
                ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name",id);
            }
            else {
                ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name");
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name");
            return View();
        }

        [Authorize(Roles = "IbX")]
        // POST: TTs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TournamentId,TeamId")] TT tT)
        {
            if (ModelState.IsValid)
            {
                tT.Id = Guid.NewGuid();
                _context.Add(tT);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { id = tT.TournamentId});
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", tT.TeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", tT.TournamentId);
            return View(tT);
        }

        [Authorize(Roles = "IbX")]
        // GET: TTs/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tT = await _context.TTs.FindAsync(id);
            if (tT == null)
            {
                return NotFound();
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", tT.TeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", tT.TournamentId);
            return View(tT);
        }

        [Authorize(Roles = "IbX")]
        // POST: TTs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TournamentId,TeamId")] TT tT)
        {
            if (id != tT.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tT);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TTExists(tT.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index),new { id = tT.TournamentId });
            }
            ViewData["TeamId"] = new SelectList(_context.Teams, "Id", "Name", tT.TeamId);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", tT.TournamentId);
            return View(tT);
        }

        [Authorize(Roles = "IbX")]
        // GET: TTs/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tT = await _context.TTs
                .Include(t => t.Team)
                .Include(t => t.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tT == null)
            {
                return NotFound();
            }

            return View(tT);
        }

        [Authorize(Roles = "IbX")]
        // POST: TTs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var tT = await _context.TTs.FindAsync(id);
            _context.TTs.Remove(tT);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "IbX")]
        private bool TTExists(Guid id)
        {
            return _context.TTs.Any(e => e.Id == id);
        }
    }
}
