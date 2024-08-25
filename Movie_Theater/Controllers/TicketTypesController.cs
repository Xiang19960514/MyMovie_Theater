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
    public class TicketTypesController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public TicketTypesController(Movie_TheaterContext context)
        {
            _context = context;
        }

        // GET: TicketTypes
        public async Task<IActionResult> Index()
        {
            return View(await _context.TicketTypes.ToListAsync());
        }

        // GET: TicketTypes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketTypes = await _context.TicketTypes
                .FirstOrDefaultAsync(m => m.TicketTypeId == id);
            if (ticketTypes == null)
            {
                return NotFound();
            }

            return View(ticketTypes);
        }

        // GET: TicketTypes/Create
        public IActionResult Create()
        {
            
            ViewBag.TypeName = new List<SelectListItem>
            {
                new SelectListItem {Value="單人票",Text="單人票"},
                new SelectListItem {Value="雙人票",Text="雙人票"},
                new SelectListItem {Value="團體票",Text="團體票"},
            };
           
            return View();
        }

        // POST: TicketTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TicketTypeId,TypeName,HowManySeatForType,TicketDescription,Price")] TicketTypes ticketTypes)
        {

            if (ModelState.IsValid)
            {
                _context.Add(ticketTypes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(ticketTypes);
        }

        // GET: TicketTypes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.HowManySeatForType = new List<SelectListItem>
            {
                new SelectListItem {Value="1",Text="單人座"},
                new SelectListItem {Value="2",Text="雙人座"},
            };
            ViewBag.TypeName = new List<SelectListItem>
            {
                new SelectListItem {Value="單人票",Text="單人票"},
                new SelectListItem {Value="雙人票",Text="雙人票"},
                new SelectListItem {Value="團體票",Text="團體票"},
            };
            if (id == null)
            {
                return NotFound();
            }

            var ticketTypes = await _context.TicketTypes.FindAsync(id);
            if (ticketTypes == null)
            {
                return NotFound();
            }
            return View(ticketTypes);
        }

        // POST: TicketTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TicketTypeId,TypeName,HowManySeatForType,TicketDescription,Price")] TicketTypes ticketTypes)
        {
            if (id != ticketTypes.TicketTypeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ticketTypes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TicketTypesExists(ticketTypes.TicketTypeId))
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
            return View(ticketTypes);
        }

        // GET: TicketTypes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ticketType = await _context.TicketTypes
                .FirstOrDefaultAsync(m => m.TicketTypeId == id);
            if (ticketType == null)
            {
                return NotFound();
            }
            try
            {
                _context.TicketTypes.Remove(ticketType);
                //TicketTypes t = await _context.TicketTypes.FindAsync(ticketType.TicketTypeId);
                await _context.SaveChangesAsync();
               
                TempData["Success"] = "1";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["Success"] = "0";
            }
            return RedirectToAction(nameof(Index));
        }

        //// POST: TicketTypes/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var ticketTypes = await _context.TicketTypes.FindAsync(id);
        //    if (ticketTypes != null)
        //    {
        //        _context.TicketTypes.Remove(ticketTypes);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        private bool TicketTypesExists(int id)
        {
            return _context.TicketTypes.Any(e => e.TicketTypeId == id);
        }
    }
}
