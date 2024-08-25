using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using Newtonsoft.Json;

namespace Movie_Theater.Controllers
{
    public class AuditoriumsController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public AuditoriumsController(Movie_TheaterContext context)
        {
            _context = context;
        }

        // GET: Auditoriums
        public async Task<IActionResult> Index()
        {
            var movie_TheaterContext = _context.Auditoriums.Include(a => a.Theater);
            return View(movie_TheaterContext);
        }

        // GET: Auditoriums/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditoriums = await _context.Auditoriums
                .Include(a => a.Theater)
                .FirstOrDefaultAsync(m => m.AuditoriumId == id);
            if (auditoriums == null)
            {
                return NotFound();
            }

            return View(auditoriums);
        }

        // GET: Auditoriums/Create
        public IActionResult Create()
        {
            ViewData["Theater_Id"] = new SelectList(_context.Theaters, "TheaterId", "TheaterName");
            return View();
        }

        // POST: Auditoriums/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Theater_Id,AuditoriumName,TotalSeats,AuditoriumType,SeatsJson")] Auditoriums_ViewModel auditoriums_vm)
        {
            if (ModelState.IsValid)
            {
                // 將 JSON 字符串反序列化為 型態List<Seats> 對象
                auditoriums_vm.Seats = JsonConvert.DeserializeObject<List<Seats>>(auditoriums_vm.SeatsJson);

                // 創建新的 Auditoriums 對象
                Auditoriums auditorium = new Auditoriums
                {
                    Theater_Id = auditoriums_vm.Theater_Id,
                    AuditoriumName = auditoriums_vm.AuditoriumName,
                    TotalSeats = auditoriums_vm.TotalSeats,
                    AuditoriumType = auditoriums_vm.AuditoriumType
                };

                // 將 Auditoriums 添加到上下文
                _context.Auditoriums.Add(auditorium);
                await _context.SaveChangesAsync();
                // 將座位數據映射到 Seats 實體並添加到上下文
                foreach (var seat in auditoriums_vm.Seats)
                {
                    Seats s = new Seats
                    {
                        SeatRow = seat.SeatRow,
                        SeatNumber = seat.SeatNumber,
                        SeatType = seat.SeatType,
                        SeatStatus = seat.SeatStatus,
                        Auditorium_Id = auditorium.AuditoriumId // 將 AuditoriumId 賦值給座位
                    };

                    _context.Seats.Add(s);
                }
				TempData["ChangeResult"] = "C_1";
				await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
			TempData["ChangeResult"] = "C_0";
			ViewData["Theater_Id"] = new SelectList(_context.Theaters, "TheaterId", "TheaterName", auditoriums_vm.Theater_Id);
            return View(auditoriums_vm);
        }

        // GET: Auditoriums/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditoriums = await _context.Auditoriums.FindAsync(id);
            var seats = await _context.Seats.Where(a => a.Auditorium_Id == id).ToListAsync();

            if (auditoriums == null)
            {
                return NotFound();
            }

            Auditoriums_ViewModel auditoriums_vm = new Auditoriums_ViewModel
            {
                Theater_Id = auditoriums.Theater_Id,
                AuditoriumName = auditoriums.AuditoriumName,
                AuditoriumType = auditoriums.AuditoriumType,
                Seats = seats
            };

            ViewBag.ID = id;
            ViewBag.Row = auditoriums_vm.Seats.Select(s => s.SeatRow).Distinct().Count();
            ViewBag.Number = auditoriums_vm.Seats.Select(s => s.SeatNumber).Distinct().Count();

            ViewData["Theater_Id"] = new SelectList(_context.Theaters, "TheaterId", "TheaterName", auditoriums_vm.Theater_Id);

            return View(auditoriums_vm);
        }

        // POST: Auditoriums/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Theater_Id,AuditoriumName,TotalSeats,AuditoriumType,SeatsJson")] Auditoriums_ViewModel auditoriums_vm)
        {
            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 將 JSON 字符串反序列化為 型態List<Seats> 對象
                    auditoriums_vm.Seats = JsonConvert.DeserializeObject<List<Seats>>(auditoriums_vm.SeatsJson);

                    // 創建新的 Auditoriums 對象
                    Auditoriums auditorium = new Auditoriums
                    {
                        AuditoriumId = id,
                        Theater_Id = auditoriums_vm.Theater_Id,
                        AuditoriumName = auditoriums_vm.AuditoriumName,
                        TotalSeats = auditoriums_vm.TotalSeats,
                        AuditoriumType = auditoriums_vm.AuditoriumType
                    };

                    // 將 Auditoriums 添加到上下文
                    _context.Update(auditorium);
                    await _context.SaveChangesAsync();
                    // 將座位數據映射到 Seats 實體並添加到上下文
                    foreach (var seat in auditoriums_vm.Seats)
                    {
                        Seats s = new Seats
                        {
                            SeatRow = seat.SeatRow,
                            SeatNumber = seat.SeatNumber,
                            SeatType = seat.SeatType,
                            SeatStatus = seat.SeatStatus,
                            Auditorium_Id = auditorium.AuditoriumId // 將 AuditoriumId 賦值給座位
                        };

                        _context.Update(s);
                    }
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuditoriumsExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
				TempData["ChangeResult"] = "E_1";
				return RedirectToAction(nameof(Index));
            }
			TempData["ChangeResult"] = "E_0";
			ViewData["Theater_Id"] = new SelectList(_context.Theaters, "TheaterId", "TheaterName", auditoriums_vm.Theater_Id);
            return View(auditoriums_vm);
        }

        // GET: Auditoriums/AuditoriumsJson/5
        public async Task<JsonResult> AuditoriumsJson(int? id)
        {
            if (id == null)
            {
                return Json("查無資料");
            }

            var seats = await _context.Seats
                        .Where(a => a.Auditorium_Id == id)
                        .Select(s => new
                        {
                            s.SeatRow,
                            s.SeatNumber,
                            s.SeatType,
                            s.SeatStatus
                        })
                        .ToListAsync();

            if (seats == null)
            {
                return Json("查無資料");
            }

            return Json(seats);
        }

        // GET: Auditoriums/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auditoriums = await _context.Auditoriums.FindAsync(id);

            if (auditoriums == null)
            {
                return NotFound();
            }
            try
            {
				//_context.Auditoriums.Remove(auditoriums);
				//await _context.SaveChangesAsync();

				TempData["ChangeResult"] = "D_1";
			}
            catch (DbUpdateConcurrencyException)
            {
				TempData["ChangeResult"] = "D_0";
			}
            return RedirectToAction(nameof(Index));
        }

        // POST: Auditoriums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auditoriums = await _context.Auditoriums.FindAsync(id);
            if (auditoriums != null)
            {
                _context.Auditoriums.Remove(auditoriums);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuditoriumsExists(int id)
        {
            return _context.Auditoriums.Any(e => e.AuditoriumId == id);
        }


    }
}
