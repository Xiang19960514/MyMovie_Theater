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
    public class CommentsController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public CommentsController(Movie_TheaterContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            //            select * from Comments as c
            //join Movies as m
            //on c.Movie_Id = m.MovieId
            //join Users as u
            //on c.[User_Id] = u.UserIdselect * from Comments as c
            //join Movies as m
            //on c.Movie_Id = m.MovieId
            //join Users as u
            //on c.[User_Id] = u.UserId          
            var movie = await (from c in _context.Comments
                               join m in _context.Movies
                               on c.Movie_Id equals m.MovieId
                               join u in _context.Users
                               on c.User_Id equals u.UserId
                               select new CommentsViewModel 
                               {    
                                    CommentsId = c.CommentsId,
                                    CommentMessage = c.CommentMessage,
                                    MovieName = m.MovieName,
                                    UserName = u.UserName,
                               }).ToListAsync();

            return View(movie);
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comments = await _context.Comments
                .Include(c => c.Movie)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.CommentsId == id);
            if (comments == null)
            {
                return NotFound();
            }

            return View(comments);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["Movie_Id"] = new SelectList(_context.Movies, "MovieId", "MovieId");
            ViewData["User_Id"] = new SelectList(_context.Users, "UserId", "UserId");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Comments1,Movie_Id,User_Id,CommentMessage")] Comments comments)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comments);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Movie_Id"] = new SelectList(_context.Movies, "MovieId", "DirectorName", comments.Movie_Id);
            ViewData["User_Id"] = new SelectList(_context.Users, "UserId", "Address", comments.User_Id);
            return View(comments);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comments = await _context.Comments.FindAsync(id);
            if (comments == null)
            {
                return NotFound();
            }
            ViewData["Movie_Id"] = new SelectList(_context.Movies, "MovieId", "DirectorName", comments.Movie_Id);
            ViewData["User_Id"] = new SelectList(_context.Users, "UserId", "Address", comments.User_Id);
            return View(comments);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Comments1,Movie_Id,User_Id,CommentMessage")] Comments comments)
        {
            if (id != comments.CommentsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comments);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentsExists(comments.CommentsId))
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
            ViewData["Movie_Id"] = new SelectList(_context.Movies, "MovieId", "DirectorName", comments.Movie_Id);
            ViewData["User_Id"] = new SelectList(_context.Users, "UserId", "Address", comments.User_Id);
            return View(comments);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var comments = await _context.Comments.FindAsync(id);
            if (comments == null)
            {
                return NotFound();
            }
            try
            {
                _context.Comments.Remove(comments);
                await _context.SaveChangesAsync();
                TempData["Success"] = "1";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["Success"] = "0";
            }
            
            return RedirectToAction(nameof(Index));
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comments = await _context.Comments.FindAsync(id);
            if (comments != null)
            {
                _context.Comments.Remove(comments);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentsExists(int id)
        {
            return _context.Comments.Any(e => e.CommentsId == id);
        }
    }
}
