using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Theater.Models;

namespace Movie_Theater.Controllers
{
    public class HabbitsController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public HabbitsController(Movie_TheaterContext context)
        {
            _context = context;
        }

        // GET: Habbits
        public async Task<IActionResult> Index()
        {
            return View(await _context.Habbits.ToListAsync());
        }

        // GET: Habbits/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habbits = await _context.Habbits
                .FirstOrDefaultAsync(m => m.HabbitId == id);
            if (habbits == null)
            {
                return NotFound();
            }

            return View(habbits);
        }

        // GET: Habbits/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Habbits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HabbitId,Habbit")] Habbits habbits)
        {
            if (ModelState.IsValid)
            {
                _context.Add(habbits);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(habbits);
        }

        // GET: Habbits/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habbits = await _context.Habbits.FindAsync(id);
            if (habbits == null)
            {
                return NotFound();
            }
            return View(habbits);
        }

        // POST: Habbits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HabbitId,Habbit")] Habbits habbits)
        {
            if (id != habbits.HabbitId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(habbits);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabbitsExists(habbits.HabbitId))
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
            return View(habbits);
        }

        // GET: Habbits/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var habbits = await _context.Habbits
                .FirstOrDefaultAsync(m => m.HabbitId == id);
            if (habbits == null)
            {
                return NotFound();
            }
            try
            {
                _context.Habbits.Remove(habbits);
                await _context.SaveChangesAsync();

                TempData["success"] = "1";
            }
            catch(DbUpdateConcurrencyException) 
            {
                TempData["success"] = "0";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Habbits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var habbits = await _context.Habbits.FindAsync(id);
            if (habbits != null)
            {
                _context.Habbits.Remove(habbits);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HabbitsExists(int id)
        {
            return _context.Habbits.Any(e => e.HabbitId == id);
        }
    }
}
