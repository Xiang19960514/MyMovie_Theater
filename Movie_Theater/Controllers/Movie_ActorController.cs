using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Theater.Models;
using Movie_Theater.ViewModels;

namespace Movie_Theater.Controllers
{
    public class Movie_ActorController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public Movie_ActorController(Movie_TheaterContext context)
        {
            _context = context;
        }

        // GET: Movie_Actor
        public async Task<IActionResult> Index(int id)// id = asp-route-"id" 如果參數id改變 route-"id也要改變"
        {   //顯示演員名稱    這裡參數的id 跟Details的 asp-route-id="@Model?.MovieId" 為單向繫結 所以id丟下去比對movieid有沒有這一筆資料(資料庫)
            ViewBag.movieid = id;   
            ViewData["Actor_Id"] = new SelectList(_context.Actors, "Actor_Id", "ActorName");    

            ViewBag.name = _context.Movies.Where(m => m.MovieId == id).Select(s=> s.MovieName).FirstOrDefault();
            ViewBag.actor = _context.Actors.Select(a => new SelectListItem
            {
                Value = a.ActorId.ToString(),
                Text = a.ActorName,
            }).ToList();
            return View();
        }

        // GET: Movie_Actor/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie_Actor = await _context.Movie_Actor
                .Include(m => m.Actor)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Movie_Id == id);
            if (movie_Actor == null)
            {
                return NotFound();
            }

            return View(movie_Actor);
        }

        // GET: Movie_Actor/Create
        [HttpGet]
        public async Task<IEnumerable<MovieActorViewModel>> Create(int id)
        {
            
            var actor = await (from a in _context.Actors
                               join ma in _context.Movie_Actor
                               on a.ActorId equals ma.Actor_Id
                               join m in _context.Movies
                               on ma.Movie_Id equals m.MovieId
                               where ma.Movie_Id == id
                               select new MovieActorViewModel
                               {
                                   Movie_Id = ma.Movie_Id,
                                   Actor_Id = ma.Actor_Id,
                                   ActorName = a.ActorName,
                                   MainLevel = ma.MainLevel,
                               }).ToListAsync();
            
            return actor;
        }

        // POST: Movie_Actor/CreateActor
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<String> CreateActor([FromBody] Movie_Actor id)
        {  //取資料 有取到代表重複 沒取到代表裡面沒有 所以check !=0就會失敗  !=0 代表裡面本來就有資料 所以無法新增
            var check = await _context.Movie_Actor.Where(ma => ma.Movie_Id == id.Movie_Id && ma.Actor_Id == id.Actor_Id).Select(ma => ma.Movie_Id).FirstOrDefaultAsync();
            if(check != 0) 
            {
                return "該演員已存在";
            }
            //因為是新增資料 所以要new一個空的Model來存放資料 欄位有什麼就放什麼
            Movie_Actor actor = new Movie_Actor
            {
                Movie_Id = id.Movie_Id,
                Actor_Id = id.Actor_Id,
                MainLevel = id.MainLevel,
            };
            try 
            {
                _context.Movie_Actor.Add(actor);
                await _context.SaveChangesAsync();
            }
            catch(DbUpdateConcurrencyException)
            {
                return "新增失敗";
            }
            return "新增成功";
        }

        // GET: Movie_Actor/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie_Actor = await _context.Movie_Actor.FindAsync(id);
            if (movie_Actor == null)
            {
                return NotFound();
            }
            ViewData["Actor_Id"] = new SelectList(_context.Actors, "ActorId", "ActorName", movie_Actor.Actor_Id);
            ViewData["Movie_Id"] = new SelectList(_context.Movies, "MovieId", "DirectorName", movie_Actor.Movie_Id);
            return View(movie_Actor);
        }

        // POST: Movie_Actor/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Movie_Id,Actor_Id,MainLevel")] Movie_Actor movie_Actor)
        {
            if (id != movie_Actor.Movie_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie_Actor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Movie_ActorExists(movie_Actor.Movie_Id))
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
            ViewData["Actor_Id"] = new SelectList(_context.Actors, "ActorId", "ActorName", movie_Actor.Actor_Id);
            ViewData["Movie_Id"] = new SelectList(_context.Movies, "MovieId", "DirectorName", movie_Actor.Movie_Id);
            return View(movie_Actor);
        }

        // GET: Movie_Actor/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie_Actor = await _context.Movie_Actor
                .Include(m => m.Actor)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Movie_Id == id);
            if (movie_Actor == null)
            {
                return NotFound();
            }

            return View(movie_Actor);
        }

        // POST: Movie_Actor/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<String> Delete([FromBody] Movie_Actor id)
        {
            var actor = await _context.Movie_Actor.Where(a => a.Movie_Id == id.Movie_Id && a.Actor_Id == id.Actor_Id).FirstOrDefaultAsync();

            if (actor == null)
            {
                return "刪除失敗";
            }
            try
            {
                _context.Movie_Actor.Remove(actor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return "刪除失敗";
            }
            return "刪除成功";
        }

        private bool Movie_ActorExists(int id)
        {
            return _context.Movie_Actor.Any(e => e.Movie_Id == id);
        }
    }
}
