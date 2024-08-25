using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace Movie_Theater.Controllers
{
    public class ShowsController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public ShowsController(Movie_TheaterContext context)
        {
            _context = context;
        }
        // 取得所有影城API
        public IActionResult GetTheaters()
        {
            var theaters = new SelectList(_context.Theaters, "TheaterId", "TheaterName");

            return Json(theaters);
        }
        // 依影城ID取得影廳API
        public IActionResult GetAuditoriumsByTheater(int id)
        {
            //var auditoriums = _context.Auditoriums.Where(a => a.Theater_Id == id);
            //var auditorium = new SelectList(auditoriums, "AuditoriumId", "AuditoriumName");
            var auditoriums = _context.Auditoriums.Where(a => a.Theater_Id == id).Select(a => new {
                value = a.AuditoriumId,
                text = a.AuditoriumName
            }).ToList();

            return Json(auditoriums);
        }

        // GET: Shows
        public async Task<IActionResult> Index()
        {

            //var movie_TheaterContext = _context.Shows.Include(s => s.Auditorium).Include(s => s.Movie);
            //ViewBag.Movies = new SelectList(_context.Movies, "MovieId", "MovieName");
            //ViewBag.Date = _context.Shows.Select(s => s.ShowDateTime.Date).Distinct();
            ViewBag.Date = 1;
            ViewBag.Movies = _context.Movies.Where(m => m.MovieState < 2).Select(m => new
            {
                MovieId = m.MovieId,
                MovieName = $"{m.MovieName} ({m.Runtime}分鐘)",
                Runtime = m.Runtime,
            });
            //ViewBag.Theaters = new SelectList(_context.Auditoriums, "AuditoriumId", "AuditoriumName");
            //return View(_context.Theaters);
            return View();
        }

        // POST: Shows/PostShows
        [HttpPost]
        public async Task<bool> PostShows([FromBody] Shows shows)
        {
            if (shows != null)
            {
                // 取得新增場次的開始和結束時間
                var newShowStartTime = shows.ShowDateTime;
                // 找出電影片長
                var runTime = await _context.Movies.Where(m => m.MovieId == shows.Movie_Id).Select(m => m.Runtime).FirstOrDefaultAsync();
                // 算出電影結束時間
                var newShowEndTime = newShowStartTime.AddMinutes(runTime);

                // 查出所有場次
                var conflictingShows = _context.Shows
                    .Where(s => s.Auditorium_Id == shows.Auditorium_Id)
                    .ToList();

                foreach (var show in conflictingShows)
                {
                    // 取出現有場次開始時間
                    var existingShowStartTime = show.ShowDateTime;
                    // 取出電影片長
                    var existingShowRunTime = await _context.Movies.Where(m => m.MovieId == show.Movie_Id).Select(m => m.Runtime).FirstOrDefaultAsync();
                    // 設定現有場次結束時間
                    var existingShowEndTime = existingShowStartTime.AddMinutes(existingShowRunTime);

                    // 檢查是否有重疊的場次
                    if ((newShowStartTime < existingShowEndTime && newShowEndTime > existingShowStartTime) ||
                        (newShowEndTime > existingShowStartTime && newShowEndTime < existingShowEndTime))
                    {   

                        return false;
                    }
                }

                try
                {
                    _context.Add(shows);
                    await _context.SaveChangesAsync();
                    //ViewBag.Date = _context.Shows.Select(s => s.ShowDateTime.Date).Distinct();
                    ViewBag.Date = 2;
                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }

            return false;
        }

        // GET: /Shows/GetShowsDate?auditoriumId=40
        [HttpGet]
        public async Task<IEnumerable<Shows>> GetShowsDate(int auditoriumId)
        {
            var shows = await _context.Shows.Where(s => s.Auditorium_Id == auditoriumId && s.ShowDateTime > DateTime.Now).ToListAsync();

            return shows;
        }

        // GET: /Shows/ShowsAllList/5
        [HttpGet]
        public async Task<IActionResult> ShowsAllList(int id)
        {
            var shows_vm = from s in _context.Shows
                           join m in _context.Movies
                           on s.Movie_Id equals m.MovieId
                           where s.Auditorium_Id == id && s.ShowDateTime > DateTime.Now
                           orderby s.ShowDateTime
                           select new Shows_ViewModel
                           {
                               ShowId = s.ShowId,
                               MovieName = m.MovieName,
                               Runtime = m.Runtime,
                               ShowDateTime = s.ShowDateTime
                           };
            return PartialView("_ShowsPartial", shows_vm);
            //return null;
        }

        // POST: /Shows/ShowsListByDate
        [HttpPost]
        public async Task<IActionResult> ShowsListByDate([FromBody] Shows_ViewModel d)
        {
            IEnumerable<Shows_ViewModel> shows_vm;
            if (d.ShowDate == "")
            {
                shows_vm = from s in _context.Shows
                           join m in _context.Movies
                           on s.Movie_Id equals m.MovieId
                           where s.Auditorium_Id == d.Auditorium_Id
                           orderby s.ShowDateTime
                           select new Shows_ViewModel
                           {
                               ShowId = s.ShowId,
                               MovieName = m.MovieName,
                               Runtime = m.Runtime,
                               ShowDateTime = s.ShowDateTime
                           };
            }
            else
            {
                DateTime showDate = DateTime.Parse(d.ShowDate);
                shows_vm = from s in _context.Shows
                           join m in _context.Movies
                           on s.Movie_Id equals m.MovieId
                           where s.Auditorium_Id == d.Auditorium_Id && s.ShowDateTime.Date == showDate
                           orderby s.ShowDateTime
                           select new Shows_ViewModel
                           {
                               ShowId = s.ShowId,
                               MovieName = m.MovieName,
                               Runtime = m.Runtime,
                               ShowDateTime = s.ShowDateTime
                           };
            }

            return PartialView("_ShowsPartial", shows_vm);
            //return null;
        }

        // GET: Shows/Delete/5
        [HttpGet]
        public async Task<string> Delete(int id)
        {
            var shows = await _context.Shows.FindAsync(id);
            if (shows != null)
            {
                try
                {
                    _context.Shows.Remove(shows);
                    await _context.SaveChangesAsync();
                }
                catch
                {
                    return "刪除失敗";
                }
                return "刪除成功";
            }
            return "刪除失敗";

        }








        // GET: Shows/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shows = await _context.Shows
                .Include(s => s.Auditorium)
                .Include(s => s.Movie)
                .FirstOrDefaultAsync(m => m.ShowId == id);
            if (shows == null)
            {
                return NotFound();
            }

            return View(shows);
        }

        // POST: Shows/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("ShowId,Movie_Id,Auditorium_Id,ShowDateTime")] Shows shows)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(shows);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["Auditorium_Id"] = new SelectList(_context.Auditoriums, "AuditoriumId", "AuditoriumName", shows.Auditorium_Id);
        //    ViewData["Movie_Id"] = new SelectList(_context.Movies, "MovieId", "DirectorName", shows.Movie_Id);
        //    return View(shows);
        //}

        // GET: Shows/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shows = await _context.Shows.FindAsync(id);
            if (shows == null)
            {
                return NotFound();
            }
            ViewData["Auditorium_Id"] = new SelectList(_context.Auditoriums, "AuditoriumId", "AuditoriumName", shows.Auditorium_Id);
            ViewData["Movie_Id"] = new SelectList(_context.Movies, "MovieId", "DirectorName", shows.Movie_Id);
            return View(shows);
        }

        // POST: Shows/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ShowId,Movie_Id,Auditorium_Id,ShowDateTime")] Shows shows)
        {
            if (id != shows.ShowId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shows);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShowsExists(shows.ShowId))
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
            ViewData["Auditorium_Id"] = new SelectList(_context.Auditoriums, "AuditoriumId", "AuditoriumName", shows.Auditorium_Id);
            ViewData["Movie_Id"] = new SelectList(_context.Movies, "MovieId", "DirectorName", shows.Movie_Id);
            return View(shows);
        }

        // GET: Shows/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var shows = await _context.Shows
        //        .Include(s => s.Auditorium)
        //        .Include(s => s.Movie)
        //        .FirstOrDefaultAsync(m => m.ShowId == id);
        //    if (shows == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(shows);
        //}

        // POST: Shows/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shows = await _context.Shows.FindAsync(id);
            if (shows != null)
            {
                _context.Shows.Remove(shows);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShowsExists(int id)
        {
            return _context.Shows.Any(e => e.ShowId == id);
        }
    }
}
