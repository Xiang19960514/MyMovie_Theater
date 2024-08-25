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
    public class Movie_ClassController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public Movie_ClassController(Movie_TheaterContext context)
        {
            _context = context;
        }

        // GET: Movie_Class
        public async Task<IActionResult> Index(int id)
        {   //顯示電影類型
            ViewBag.MovieId = id;
            ViewData["Class_Id"] = new SelectList(_context.Classes, "ClassId", "ClassName");
            var movieclass = await (from m in _context.Movies
                                    where m.MovieId == id
                                    select new MoviesViewModel
                                    {
                                        MovieId = m.MovieId,
                                        MovieName = m.MovieName,
                                        
                                    }).FirstOrDefaultAsync();
            ViewBag.classes = id;
            return View(movieclass);

        }

        // GET: Movie_Class/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie_Class = await _context.Movie_Class
                .Include(m => m.Class)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Movie_Id == id);
            if (movie_Class == null)
            {
                return NotFound();
            }

            return View(movie_Class);
        }

        // GET Movie_Class/CreateClassType?id=1
        [HttpGet]
        public async Task<IEnumerable<MovieClassViewModel>> CreateClassType(int id) 
        { //從資料庫取最新的資料
            /*
             select ClassId,MovieName,ClassName from Movies m 
                join Movie_Class mc
                on m.MovieId = mc.Movie_Id
                join Classes c
                on mc.Class_Id = c.ClassId
                where m.MovieId = 1 
             */
            var classes = await (from m in _context.Movies
                                 join mc in _context.Movie_Class
                                 on m.MovieId equals mc.Movie_Id
                                 join c in _context.Classes
                                 on mc.Class_Id equals c.ClassId
                                 where m.MovieId == id
                                 select new MovieClassViewModel
                                 {
                                     Movie_Id = mc.Movie_Id,
                                     Class_Id = c.ClassId,
                                     ClassName = c.ClassName
                                 }).ToListAsync();
            
            return classes;
        }

        // POST: Movie_Class/Create
        [HttpPost]
        public async Task<String> Create([FromBody]Movie_Class id)
        {
            // Select * from Movie_Class Where Movie_Id = 16 and Class_Id = 1 有值=重複 沒有值=沒重複
            //var check = await (from m in _context.Movie_Class
            //            where id.Movie_Id == m.Movie_Id && id.Class_Id == m.Class_Id
            //            select m.Movie_Id).FirstOrDefaultAsync();

            var check = await _context.Movie_Class.Where(m => m.Movie_Id == id.Movie_Id && m.Class_Id == id.Class_Id).Select(m => m.Movie_Id).FirstOrDefaultAsync();
            if (check != 0) 
            {
                return "該類型已存在";
            }
            Movie_Class ID = new Movie_Class
            {
                Movie_Id = id.Movie_Id,
                Class_Id = id.Class_Id,
            };
            try 
            {
                _context.Movie_Class.Add(ID);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return "儲存失敗";
            }
            return "儲存成功";
        }

            
        // POST: Movie_Class/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Movie_Id,Class_Id,Other")] Movie_Class movie_Class)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(movie_Class);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    ViewData["Class_Id"] = new SelectList(_context.Classes, "ClassId", "ClassName", movie_Class.Class_Id);
        //    ViewData["Movie_Id"] = new SelectList(_context.Movies, "MovieId", "DirectorName", movie_Class.Movie_Id);
        //    return View(movie_Class);
        //}

        // GET: Movie_Class/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie_Class = await _context.Movie_Class.FindAsync(id);
            if (movie_Class == null)
            {
                return NotFound();
            }
            ViewData["Class_Id"] = new SelectList(_context.Classes, "ClassId", "ClassName", movie_Class.Class_Id);
            ViewData["Movie_Id"] = new SelectList(_context.Movies, "MovieId", "DirectorName", movie_Class.Movie_Id);
            return View(movie_Class);
        }

        // POST: Movie_Class/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Movie_Id,Class_Id,Other")] Movie_Class movie_Class)
        {
            if (id != movie_Class.Movie_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie_Class);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Movie_ClassExists(movie_Class.Movie_Id))
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
            ViewData["Class_Id"] = new SelectList(_context.Classes, "ClassId", "ClassName", movie_Class.Class_Id);
            ViewData["Movie_Id"] = new SelectList(_context.Movies, "MovieId", "DirectorName", movie_Class.Movie_Id);
            return View(movie_Class);
        }

        // GET: Movie_Class/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie_Class = await _context.Movie_Class
                .Include(m => m.Class)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(m => m.Movie_Id == id);
            if (movie_Class == null)
            {
                return NotFound();
            }

            return View(movie_Class);
        }

        // POST: Movie_Class/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<String> DeleteConfirmed([FromBody] Movie_Class id)
        {   //form "data" = formbody   formfrom= form/submit  
            var classid = await _context.Movie_Class.Where(m => m.Movie_Id == id.Movie_Id && m.Class_Id == id.Class_Id).FirstOrDefaultAsync();//FirstOrDefaultAsync 可能是這個方法有錯


            if (classid == null)
            {
                return "刪除失敗";
            }
            try 
            {
                _context.Movie_Class.Remove(classid);
                 await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return "刪除失敗";
            }
            return "刪除成功";

        }

        private bool Movie_ClassExists(int id)
        {
            return _context.Movie_Class.Any(e => e.Movie_Id == id);


        }
    }
}
