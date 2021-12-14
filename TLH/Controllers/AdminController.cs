using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLH.Entity;
using TLH.Models.Admin;

namespace TLH.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly DataContext _context;

        public AdminController(DataContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Teams()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ACCreate()
        {
            ACCreateViewModel viewModel = new ACCreateViewModel();
            return View(viewModel);
        }
        [HttpPost]
        public IActionResult ACCreate(ACCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Tournament tournament = new Tournament();
                tournament.Name = viewModel.Name;
                tournament.Id = Guid.NewGuid();
                _context.Tournaments.Add(tournament);
                if (viewModel.NumberOfDays > 0)
                {
                    for (int i = viewModel.NumberOfDays; i >= 1; i--)
                    {
                        DayOfT day = new DayOfT { Id = Guid.NewGuid(), Name = "Day " + i, TournamentId = tournament.Id };
                        _context.DayOfTs.Add(day);
                        if (viewModel.NumberOfMatchesPerDay > 0)
                        {
                            for (int m = viewModel.NumberOfMatchesPerDay; m >= 1; m--)
                            {
                                _context.Matchs.Add(new Match { Id = Guid.NewGuid(), Name = "Match" + m, DayOfTId = day.Id,TournamentId = tournament.Id });
                            }
                        }
                    }
                }
                _context.SaveChanges();
                return RedirectToAction("Index","Tournaments");
            }
            return View(viewModel);
        }
    }
}
