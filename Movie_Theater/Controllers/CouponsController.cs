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
    public class CouponsController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public CouponsController(Movie_TheaterContext context)
        {
            _context = context;
        }

        // GET: Coupons
        public async Task<IActionResult> Index()
        {
            return View(await _context.Coupons.ToListAsync());
        }

        // GET: Coupons/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupons = await _context.Coupons
                .FirstOrDefaultAsync(m => m.CouponID == id);
            if (coupons == null)
            {
                return NotFound();
            }

            return View(coupons);
        }

        // GET: Coupons/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Coupons/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CouponID,CouponCode,DiscountType,DiscountValue,StartDate,ExpiryDate,MaxUsageCount,CurrentUsageCount,Description")] Coupons coupons)
        {
            if (ModelState.IsValid)
            {
                _context.Add(coupons);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(coupons);
        }

        // GET: Coupons/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupons = await _context.Coupons.FindAsync(id);
            if (coupons == null)
            {
                return NotFound();
            }
            return View(coupons);
        }

        // POST: Coupons/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CouponID,CouponCode,DiscountType,DiscountValue,StartDate,ExpiryDate,MaxUsageCount,CurrentUsageCount,Description")] Coupons coupons)
        {
            if (id != coupons.CouponID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(coupons);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CouponsExists(coupons.CouponID))
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
            return View(coupons);
        }

        // GET: Coupons/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var coupons = await _context.Coupons
                .FirstOrDefaultAsync(m => m.CouponID == id);
            if (coupons == null)
            {
                return NotFound();
            }
            try
            {
                _context.Coupons.Remove(coupons);
                await _context.SaveChangesAsync();
                TempData["Success"] = "1";
            }
            catch(DbUpdateConcurrencyException) 
            {
                TempData["Success"] = "0";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Coupons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var coupons = await _context.Coupons.FindAsync(id);
            if (coupons != null)
            {
                _context.Coupons.Remove(coupons);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CouponsExists(int id)
        {
            return _context.Coupons.Any(e => e.CouponID == id);
        }
    }
}
