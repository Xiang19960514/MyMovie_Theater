﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Theater.Models;

namespace Movie_Theater.Controllers
{
    public class ActorsController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public ActorsController(Movie_TheaterContext context)
        {
            _context = context;
        }

        // GET: Actors
        public async Task<IActionResult> Index()
        {
            return View(await _context.Actors.ToListAsync());
        }

        // GET: Actors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actors = await _context.Actors
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actors == null)
            {
                return NotFound();
            }

            return View(actors);
        }

        // GET: Actors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ActorId,ActorName")] Actors actors)
        {
            if (ModelState.IsValid)
            {
                _context.Add(actors);
                await _context.SaveChangesAsync();
                TempData["ChangeResult"] = "C_1";
                return RedirectToAction(nameof(Index));
            }
            TempData["ChangeResult"] = "C_0";
            return View(actors);
        }

        // GET: Actors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actors = await _context.Actors.FindAsync(id);
            if (actors == null)
            {
                return NotFound();
            }
            return View(actors);
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ActorId,ActorName")] Actors actors)
        {
            if (id != actors.ActorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(actors);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ActorsExists(actors.ActorId))
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
            return View(actors);
        }

        // GET: Actors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var actors = await _context.Actors
                .FirstOrDefaultAsync(m => m.ActorId == id);
            if (actors == null)
            {
                return NotFound();
            }
            try
            {
                _context.Actors.Remove(actors);
                await _context.SaveChangesAsync();
                TempData["ChangeResult"] = "D_1";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["ChangeResult"] = "D_0";
            }
            
            return RedirectToAction(nameof(Index));
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var actors = await _context.Actors.FindAsync(id);
            if (actors != null)
            {
                _context.Actors.Remove(actors);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ActorsExists(int id)
        {
            return _context.Actors.Any(e => e.ActorId == id);
        }
    }
}
