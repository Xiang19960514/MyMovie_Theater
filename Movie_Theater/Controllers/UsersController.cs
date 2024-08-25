using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Theater.DTO;
using Movie_Theater.Models;
using Movie_Theater.ViewModels;

namespace Movie_Theater.Controllers
{
    public class UsersController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public UsersController(Movie_TheaterContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetPie()
        {
            // 性別比例
            ViewBag.boy = _context.Users.Where(u => u.Sex == "男").Count();
            ViewBag.girl = _context.Users.Where(u => u.Sex == "女").Count();

            // 年訂票
            ViewBag.Year = _context.Bookings.Select(b => b.BookingDate.Year).Distinct();

            return View();
        }

        // GET: Users/Getbooking/${id}?
        [HttpGet]
        public async Task<IEnumerable<bookingChart_DTO>> Getbooking(int id)
        { //年訂票量圖表
            var bookingchart = await (from b in _context.Bookings
                               join s in _context.Shows on b.ShowingId equals s.ShowId
                               join m in _context.Movies on s.Movie_Id equals m.MovieId
                               where b.BookingDate.Year == id
                               group b by new { m.MovieName, b.BookingDate.Year, b.BookingDate.Month } into g
                               orderby g.Key.Year, g.Key.Month
                               select new bookingChart_DTO
                               {
                                   MovieName = g.Key.MovieName,
                                   Year = g.Key.Year,
                                   Month = g.Key.Month,
                                   BookingCount = g.Count()
                               }).ToListAsync();
            return bookingchart;
        }

        // Users/GetbookingCount/${id}?movieid=${movieid}
        [HttpGet]
        public async Task<IEnumerable<bookingChart_DTO>> GetbookingCount(int id)
        {   //年訂票量
            var bookingCount = await (from b in _context.Bookings
                                      join BTD in _context.BookingTicketTypes_Detail on b.BookingId equals BTD.Booking_Id
                                      //join s in _context.Shows on b.ShowingId equals s.ShowId
                                      //join m in _context.Movies on s.Movie_Id equals m.MovieId
                                      where b.BookingDate.Year == id 
                                      group b by new { BTD.TicketTypeId } into g
                                      //orderby g.Key.Year, g.Key.Month
                                      select new bookingChart_DTO
                                      {
                                          //MovieName = g.Key.MovieName,
                                          Year = 0,
                                          //Month = g.Key.Month,
                                          BookingCount = g.Count()
                                      }).ToListAsync();
            var count = 0;
            foreach(var a in bookingCount)
            {
                count += a.BookingCount;
            }
            //TempData[$"{id}"] = count;
            foreach (var b in bookingCount)
            {
                b.Year = count;
            }

            return bookingCount;
        }
        // GET: Users/Getrevenue
        [HttpGet]
        public async Task<IEnumerable<Revenue_DTO>> Getrevenue(int id)
        {   //年營業額
            var bookingchart = await (from BTD in _context.BookingTicketTypes_Detail
                                      join TT in _context.TicketTypes on BTD.TicketTypeId equals TT.TicketTypeId
                                      join B in _context.Bookings on BTD.Booking_Id equals B.BookingId
                                      where B.BookingDate.Year == id && B.BookingStatus == "Confirmed"
                                      group TT.Price by new { B.BookingDate.Year, B.BookingDate.Month } into g
                                      orderby g.Key.Year, g.Key.Month
                                      select new Revenue_DTO
                                      {
                                          Month = g.Key.Month,
                                          Revenue = g.Sum() // 計算當月的營業額
                                      }).ToListAsync();
            return bookingchart;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        { 
            return View(_context.Users);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            ViewBag.userSex = new List<SelectListItem>
            {
                new SelectListItem {Value="男",Text="男"},
                new SelectListItem {Value="女",Text="女"},
            };
            ViewBag.MBTI = new List<SelectListItem>
            {
                new SelectListItem { Value = "INTJ", Text = "INTJ - 建築師" },
                new SelectListItem { Value = "INTP", Text = "INTP - 思考者" },
                new SelectListItem { Value = "ENTJ", Text = "ENTJ - 指揮官" },
                new SelectListItem { Value = "ENTP", Text = "ENTP - 辯論家" },
                new SelectListItem { Value = "INFJ", Text = "INFJ - 擁護者" },
                new SelectListItem { Value = "INFP", Text = "INFP - 調停者" },
                new SelectListItem { Value = "ENFJ", Text = "ENFJ - 主人公" },
                new SelectListItem { Value = "ENFP", Text = "ENFP - 活動家" },
                new SelectListItem { Value = "ISTJ", Text = "ISTJ - 後勤官" },
                new SelectListItem { Value = "ISFJ", Text = "ISFJ - 防衛者" },
                new SelectListItem { Value = "ESTJ", Text = "ESTJ - 行政官" },
                new SelectListItem { Value = "ESFJ", Text = "ESFJ - 執政官" },
                new SelectListItem { Value = "ISTP", Text = "ISTP - 巧匠" },
                new SelectListItem { Value = "ISFP", Text = "ISFP - 探險家" },
                new SelectListItem { Value = "ESTP", Text = "ESTP - 創業家" },
                new SelectListItem { Value = "ESFP", Text = "ESFP - 表演者" }
            };
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,UserName,NickName,Birthday,Phone,Sex,Email,Address,MBTI,EmailConfirm")] Users users)
        {
            if (ModelState.IsValid)
            {
                _context.Add(users);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(users);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            ViewBag.userSex = new List<SelectListItem>
            {
                new SelectListItem {Value="男",Text="男"},
                new SelectListItem {Value="女",Text="女"},
            };
            ViewBag.MBTI = new List<SelectListItem>
            {
                new SelectListItem { Value = "INTJ", Text = "INTJ - 建築師" },
                new SelectListItem { Value = "INTP", Text = "INTP - 思考者" },
                new SelectListItem { Value = "ENTJ", Text = "ENTJ - 指揮官" },
                new SelectListItem { Value = "ENTP", Text = "ENTP - 辯論家" },
                new SelectListItem { Value = "INFJ", Text = "INFJ - 擁護者" },
                new SelectListItem { Value = "INFP", Text = "INFP - 調停者" },
                new SelectListItem { Value = "ENFJ", Text = "ENFJ - 主人公" },
                new SelectListItem { Value = "ENFP", Text = "ENFP - 活動家" },
                new SelectListItem { Value = "ISTJ", Text = "ISTJ - 後勤官" },
                new SelectListItem { Value = "ISFJ", Text = "ISFJ - 防衛者" },
                new SelectListItem { Value = "ESTJ", Text = "ESTJ - 行政官" },
                new SelectListItem { Value = "ESFJ", Text = "ESFJ - 執政官" },
                new SelectListItem { Value = "ISTP", Text = "ISTP - 巧匠" },
                new SelectListItem { Value = "ISFP", Text = "ISFP - 探險家" },
                new SelectListItem { Value = "ESTP", Text = "ESTP - 創業家" },
                new SelectListItem { Value = "ESFP", Text = "ESFP - 表演者" }
            };
            ViewBag.EmailConfirm = new List<SelectListItem>
            {
                new SelectListItem{ Value = "true",  Text="已啟用"},
                new SelectListItem{ Value = "false", Text="未啟用"},
            };
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,UserName,NickName,Birthday,Phone,Sex,Email,Address,MBTI,EmailConfirm")] Users users)
        {
            if (id != users.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserId))
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
            return View(users);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }
            try
            {
                _context.Users.Remove(users);
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

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var users = await _context.Users.FindAsync(id);
            if (users != null)
            {
                _context.Users.Remove(users);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }


    }
}
