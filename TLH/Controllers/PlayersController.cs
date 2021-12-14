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
using TLH.Models.Players;

namespace TLH.Controllers
{
    public class PlayersController : Controller
    {
        private readonly DataContext _context;

        public PlayersController(DataContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
            return View(await _context.Players.ToListAsync());
        }

        // GET: Players/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.Include(p => p.PTs).ThenInclude(p => p.Team).ThenInclude(p => p.Achivements).ThenInclude(p => p.Tournament)
                .FirstOrDefaultAsync(m => m.Id == id);
            PLayerDetailViewModel viewModel = new PLayerDetailViewModel { Id = player.Id, Name = player.Name, Comment = player.Comment, Merit = player.Merit };
            viewModel.Achives = new List<AchivementViewModel>();
            var achivementsList = player.PTs.Select(p => p.Team.Achivements).ToList();
            if (achivementsList.Count() > 0)
            {
                var prizes = _context.Prizes.Where(pz => player.PTs.Select(p => p.TournamentId).Contains(pz.TournamentId)).ToList();
                foreach (var achivements in achivementsList)
                {
                    foreach (var achivement in achivements)
                    {
                        var prize = prizes.Where(p => p.TournamentId == achivement.TournamentId && p.Placement == achivement.Placement).ToList();
                        AchivementViewModel achivementView = new AchivementViewModel { Place = achivement.Placement, TeamName = achivement.Team.Name, TourName = achivement.Tournament.Name,Amount = prize.Count() > 0 ? prize.FirstOrDefault().Amount : 0 };
                        viewModel.Achives.Add(achivementView);
                    }
                }
            }
            if (player == null)
            {
                return NotFound();
            }

            return View(viewModel);
        }

        public async Task<IActionResult> DetailedDetails(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        [Authorize(Roles = "IbX")]
        // GET: Players/Create
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Roles = "IbX")]
        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Comment,Merit")] Player player)
        {
            if (ModelState.IsValid)
            {
                player.Id = Guid.NewGuid();
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(player);
        }

        [Authorize(Roles = "IbX")]
        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            return View(player);
        }

        [Authorize(Roles = "IbX")]
        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,Name,Comment,Merit")] Player player)
        {
            if (id != player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
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
            return View(player);
        }

        [Authorize(Roles = "IbX")]
        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        [Authorize(Roles = "IbX")]
        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var player = await _context.Players.FindAsync(id);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "IbX")]
        private bool PlayerExists(Guid id)
        {
            return _context.Players.Any(e => e.Id == id);
        }
    }
}
