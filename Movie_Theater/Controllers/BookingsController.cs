using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Movie_Theater.Models;
using Movie_Theater.ViewModels;

namespace Movie_Theater.Controllers
{
    public class BookingsController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public BookingsController(Movie_TheaterContext context)
        {
            _context = context;
        }

        // GET: Bookings
        public async Task<IActionResult> Index()
        {

            // 查詢座位資訊
            var detialsQuery = await (from bsd in _context.BookingSeats_Detail
                                join ss in _context.ShowSeats
                                on bsd.ShowSeat_Id equals ss.ShowSeatId
                                join se in _context.Seats
                                on ss.Seat_Id equals se.SeatId
                                join a in _context.Auditoriums
                                on se.Auditorium_Id equals a.AuditoriumId
                                join sh in _context.Shows
                                on ss.Show_Id equals sh.ShowId
                                join m in _context.Movies
                                on sh.Movie_Id equals m.MovieId
                                select new _Detials
                                {
                                    BookingId = bsd.Booking_Id,
                                    AuditoriumName = a.AuditoriumName,
                                    MovieName = m.MovieName,
                                    Level = m.Level,
                                    Language = m.Language,
                                    SeatRow = se.SeatRow,
                                    SeatNumber = se.SeatNumber,
                                    SeatType = se.SeatType,
                                    ShowDateTime = sh.ShowDateTime,
                                }).ToListAsync();

            // 查詢票種資訊
            var ticketTypesQuery = await (_context.BookingTicketTypes_Detail
                                    .Join(_context.TicketTypes,
                                    bttd => bttd.TicketTypeId,
                                    tt => tt.TicketTypeId,
                                    (bttd, tt) => new _TicketTypes
                                    {
                                        BookingId = bttd.Booking_Id,
                                        TypeName = tt.TypeName,
                                        HowManySeatForType = tt.HowManySeatForType,
                                        Price = tt.Price
                                    })).ToListAsync();
            // 查詢餐點
            var snacksQuery = _context.BookingSnacks
                        .Join(_context.Snacks,
                        bs => bs.SnackId,
                        s => s.SnackId,
                        (bs, s) => new _Snacks
                        {
                            BookingId = bs.BookingId,
                            SnackName = s.SnackName,
                            Price = s.Price,
                        });

            // 查詢訂單
            var bookings = await _context.Bookings.ToListAsync();

            // 查詢訂單支付
            var paymentTransactions = await _context.PaymentTransactions.ToListAsync();

            // 查出所有訂單
            var bookings_vm = (bookings.Join(paymentTransactions,
                                b => b.BookingId,
                                pt => pt.BookingId,
                                (b, pt) => new Bookings_ViewModel
                                {
                                    BookingId = b.BookingId,
                                    UserId = b.UserId,
                                    BookingDate = b.BookingDate,
                                    MerchantTradeNo = b.MerchantTradeNo,
                                    BookingStatus = b.BookingStatus,
                                    TransactionId = pt.TransactionId,
                                    PaymentMethod = pt.PaymentMethod,
                                    PaymentDate = pt.PaymentDate,
                                    detials = detialsQuery.Where(dq => dq.BookingId == b.BookingId).ToList(),
                                    ticketTypes = ticketTypesQuery.Where(ttq => ttq.BookingId == b.BookingId).ToList(),
                                    snacks = snacksQuery.Where(sq => sq.BookingId == b.BookingId).ToList()
                                })).ToList();

            return View(bookings_vm);
        }

        // GET: Bookings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookings = await _context.Bookings
                .Include(b => b.Showing)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (bookings == null)
            {
                return NotFound();
            }

            return View(bookings);
        }

        // GET: Bookings/Create
        public IActionResult Create()
        {
            ViewData["ShowingId"] = new SelectList(_context.Shows, "ShowId", "ShowId");
            return View();
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookingId,UserId,ShowingId,BookingDate,MerchantTradeNo,BookingStatus")] Bookings bookings)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ShowingId"] = new SelectList(_context.Shows, "ShowId", "ShowId", bookings.ShowingId);
            return View(bookings);
        }

        // GET: Bookings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookings = await _context.Bookings.FindAsync(id);
            if (bookings == null)
            {
                return NotFound();
            }
            ViewData["ShowingId"] = new SelectList(_context.Shows, "ShowId", "ShowId", bookings.ShowingId);
            return View(bookings);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookingId,UserId,ShowingId,BookingDate,MerchantTradeNo,BookingStatus")] Bookings bookings)
        {
            if (id != bookings.BookingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookingsExists(bookings.BookingId))
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
            ViewData["ShowingId"] = new SelectList(_context.Shows, "ShowId", "ShowId", bookings.ShowingId);
            return View(bookings);
        }

        // GET: Bookings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bookings = await _context.Bookings
                .Include(b => b.Showing)
                .FirstOrDefaultAsync(m => m.BookingId == id);
            if (bookings == null)
            {
                return NotFound();
            }

            return View(bookings);
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bookings = await _context.Bookings.FindAsync(id);
            if (bookings != null)
            {
                _context.Bookings.Remove(bookings);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BookingsExists(int id)
        {
            return _context.Bookings.Any(e => e.BookingId == id);
        }
    }
}
