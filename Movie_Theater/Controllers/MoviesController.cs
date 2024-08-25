using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Movie_Theater.Models;
using Movie_Theater.ViewModels;
using Newtonsoft.Json;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace Movie_Theater.Controllers
{
    public class MoviesController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public MoviesController(Movie_TheaterContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            
            return View(_context.Movies);
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }                                                   
            var movies = await (from m in _context.Movies 
                                where m.MovieId == id
                                select new MoviesViewModel 
                                { //以下是把 Model的資料丟給ViewModel
                                    MovieId = m.MovieId,
                                    MovieName = m.MovieName,
                                    MovieEnglishName = m.MovieEnglishName,
                                    MovieDescription = m.MovieDescription,
                                    ReleaseDate = m.ReleaseDate,
                                    Runtime = m.Runtime,
                                    Level = m.Level,
                                    Language = m.Language,
                                    MovieImage = m.MovieImage,
                                    Movievideo = m.Movievideo,
                                    DirectorName = m.DirectorName,
                                    MovieState = m.MovieState,
                                    ClassName = (from mc in _context.Movie_Class //合併第二張 ClassName表
                                                 join c in _context.Classes on mc.Class_Id equals c.ClassId
                                                 where mc.Movie_Id == m.MovieId
                                                 select new ClassesViewModel //new class的表取類型名稱
                                                 {
                                                     ClassName = c.ClassName 
                                                 }).ToList(),

                                    ActorName = (from ma in _context.Movie_Actor //合併第三張 ActorName 演員名稱
                                                 join a in _context.Actors on ma.Actor_Id equals a.ActorId
                                                 where ma.Movie_Id == m.MovieId 
                                                 select new ActorViewModel 
                                                 {
                                                     ActorName = a.ActorName,
                                                     MainLevel = ma.MainLevel
                                                     
                                                 }).ToList() 
                                }).FirstOrDefaultAsync();

           
            if (movies == null)
            {
                return NotFound();
            }

            return View(movies);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            ViewBag.MovieState = new List<SelectListItem>
            {
                new SelectListItem {Value="0",Text="即將上映"},
                new SelectListItem {Value="1",Text="上映中"},
                new SelectListItem {Value="2",Text="已下檔"},
            };
            ViewBag.Level = new List<SelectListItem>
            {
                new SelectListItem{Value="0",Text="普遍級"},
                new SelectListItem{Value="6", Text="保護級"},
                new SelectListItem{Value="12", Text="輔導級(12)"},
                new SelectListItem{Value="15", Text="輔導級(15)"},
                new SelectListItem{Value="18", Text="限制級"},
            };
            ViewBag.Language = new List<SelectListItem>
            {
                new SelectListItem{Value="中文",Text="中文"},
                new SelectListItem{Value="英文", Text="英文"},
               
            };
            
            return View();

        }
        public string ConvertToEmbedUrl(string videoUrl)
        {
            var regex = new Regex(@"(?:https?:\/\/)?(?:www\.)?youtube\.com\/watch\?v=([^&]+)");
            var match = regex.Match(videoUrl);
            if (match.Success)
            {
                var videoId = match.Groups[1].Value;
                return $"https://www.youtube.com/embed/{videoId}";
            }
            return null;
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("MovieId,MovieName,MovieEnglishName,MovieDescription,ReleaseDate,Runtime,Level,Language,MovieImage,Movievideo,DirectorName,MovieState,ActorNameJson,GenreNameJson")] MoviesViewModel movies)
        {
            if (ModelState.IsValid)
            {
                // 將外部網址轉換為內嵌網址
                if (movies.Movievideo != null)
                {
                    movies.Movievideo = ConvertToEmbedUrl(movies.Movievideo);
                }           
                try
                {
                    // 判斷是否有上傳檔案
                    if (Request.Form.Files["MovieImage"] != null)
                    {
                        // 取得照片欄位名稱
                        var pictureFile = Request.Form.Files["MovieImage"];

                        // 新增存圖檔路徑
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        // 確保目標目錄存在
                        if (!Directory.Exists(uploadsFolder))
                        {
                            // 如果路徑不再則創建
                            Directory.CreateDirectory(uploadsFolder);
                        }

                        // 生成唯一的文件名以避免重名
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + pictureFile.FileName;

                        // 目標文件的完整路徑
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        // 將文件保存到指定路徑
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await pictureFile.CopyToAsync(fileStream);
                        }
                        // 更新圖片路徑
                        movies.MovieImage = uniqueFileName;

                    }
                    Movies moviesmodel = new Movies()
                    {
                        MovieName = movies.MovieName,
                        MovieEnglishName = movies.MovieEnglishName,
                        ReleaseDate = movies.ReleaseDate,
                        Runtime = movies.Runtime,
                        Level = movies.Level,
                        Language = movies.Language,
                        DirectorName = movies.DirectorName,
                        MovieState = movies.MovieState,
                        MovieDescription = movies.MovieDescription,
                        MovieImage = movies.MovieImage,
                        Movievideo = movies.Movievideo
                    };
                   
                    _context.Add(moviesmodel);
                    await _context.SaveChangesAsync();

                    if (movies.ActorNameJson != null)
                    {
                        var actors = JsonConvert.DeserializeObject<List<Actor>>(movies.ActorNameJson);

                        foreach (var a in actors)
                        {   //分辨主演
                            int? atr = await _context.Actors.Where(actor => actor.ActorName == a.ChineseName).Select(a => a.ActorId).FirstOrDefaultAsync();
                            int mainlevel = 0;
                            if (a.Order >= 3 && a.Order < 6)
                            {
                                mainlevel = 1;
                            }
                            if (a.Order > 5)
                            {
                                mainlevel = 2;
                            }
                            if (atr > 0)
                            {

                                Movie_Actor ma = new Movie_Actor()
                                {
                                    Movie_Id = moviesmodel.MovieId,
                                    Actor_Id = atr.Value,
                                    MainLevel = mainlevel
                                };
                                _context.Add(ma);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                Actors actors1 = new Actors()
                                {
                                    ActorName = a.ChineseName
                                };
                                _context.Add(actors1);
                                await _context.SaveChangesAsync();
                                Movie_Actor ma = new Movie_Actor()
                                {
                                    Movie_Id = moviesmodel.MovieId,
                                    Actor_Id = actors1.ActorId,
                                    MainLevel = mainlevel
                                };
                                _context.Add(ma);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }


                    if (movies.GenreNameJson != null)
                    {  //類型
                        var genres = JsonConvert.DeserializeObject<List<Genre>>(movies.GenreNameJson);

                        foreach (var g in genres)
                        {
                            int? genreId = await _context.Classes.Where(c => c.ClassName == g.GenreName).Select(c => c.ClassId).FirstOrDefaultAsync();
                            if (genreId > 0)
                            {
                                Movie_Class mc = new Movie_Class()
                                {
                                    Movie_Id = moviesmodel.MovieId,
                                    Class_Id = genreId.Value
                                };
                                _context.Add(mc);
                                await _context.SaveChangesAsync();
                            }
                            else
                            {
                                Classes newClass = new Classes()
                                {
                                    ClassName = g.GenreName
                                };
                                _context.Add(newClass);
                                await _context.SaveChangesAsync();

                                Movie_Class mc = new Movie_Class()
                                {
                                    Movie_Id = moviesmodel.MovieId,
                                    Class_Id = newClass.ClassId
                                };
                                _context.Add(mc);
                                await _context.SaveChangesAsync();
                            }
                        }

                    }


                    TempData["ChangeResult"] = "C_1";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoviesExists(movies.MovieId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                

            }
            TempData["ChangeResult"] = "C_0";
            return View(movies);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }

            var movies = await (from m in _context.Movies
                                where m.MovieId == id
                                select new MoviesViewModel 
                                { // Model的資料丟給ViewModel   
                                    MovieId = m.MovieId,
                                    MovieName = m.MovieName,
                                    MovieEnglishName = m.MovieEnglishName,
                                    MovieDescription = m.MovieDescription,
                                    ReleaseDate = m.ReleaseDate,
                                    Runtime = m.Runtime,
                                    Level = m.Level,
                                    Language = m.Language,
                                    MovieImage = m.MovieImage,
                                    Movievideo = m.Movievideo,
                                    DirectorName = m.DirectorName,
                                    MovieState = m.MovieState
                                }).FirstOrDefaultAsync();
            if (movies == null)
            {
                return NotFound();
            }
            ViewBag.MovieState = new List<SelectListItem>
            {
                new SelectListItem {Value="0",Text="即將上映"},
                new SelectListItem {Value="1",Text="上映中"},
                new SelectListItem {Value="2",Text="已下檔"},
            };
            ViewBag.Level = new List<SelectListItem>
            {
                new SelectListItem{Value="0",Text="普遍級"},
                new SelectListItem{Value="6", Text="保護級"},
                new SelectListItem{Value="12", Text="輔導級(12)"},
                new SelectListItem{Value="15", Text="輔導級(15)"},
                new SelectListItem{Value="18", Text="限制級"},
            };
            ViewBag.Language = new List<SelectListItem>
            {
                new SelectListItem{Value="中文",Text="中文"},
                new SelectListItem{Value="英文", Text="英文"},

            };

            return View(movies);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MovieId,MovieName,MovieEnglishName,MovieDescription,ReleaseDate,Runtime,Level,Language,MovieImage,Movievideo,DirectorName,MovieState")] MoviesViewModel mvm)
        {
            if (id != mvm.MovieId)
            {
                return NotFound();
            }
            // 取出原先所有資料
            Movies m = await _context.Movies.FindAsync(mvm.MovieId);
            if (ModelState.IsValid)
            {
                // 將影片網址轉換為內嵌網址
                if (mvm.Movievideo != null && mvm.Movievideo != m.Movievideo)
                {
                    mvm.Movievideo = ConvertToEmbedUrl(mvm.Movievideo);
                }
                try
                {
                    

                    // 判斷是否有上傳檔案
                    if (Request.Form.Files["MovieImage"] != null)
                    {
                        var pictureFile = Request.Form.Files["MovieImage"];
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        if (!Directory.Exists(uploadsFolder))
                        {
                            Directory.CreateDirectory(uploadsFolder);
                        }
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + pictureFile.FileName;
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await pictureFile.CopyToAsync(fileStream);
                        }
                        mvm.MovieImage = uniqueFileName;
                    }
                    else
                    {
                        mvm.MovieImage = m.MovieImage;
                    }

                    // 解除追蹤
                    _context.Entry(m).State = EntityState.Detached;

                    

                    Movies movies = new Movies
                    {
                        MovieId = mvm.MovieId,
                        MovieName = mvm.MovieName,
                        MovieEnglishName = mvm.MovieEnglishName,
                        MovieDescription = mvm.MovieDescription,
                        ReleaseDate = mvm.ReleaseDate,
                        Runtime = mvm.Runtime,
                        Level = mvm.Level,
                        Language = mvm.Language,
                        MovieImage = mvm.MovieImage,
                        Movievideo = mvm.Movievideo,
                        DirectorName = mvm.DirectorName,
                        MovieState = mvm.MovieState
                    };

                    _context.Update(movies);
                    await _context.SaveChangesAsync();

                    TempData["ChangeResult"] = "E_1";
                    if (!string.IsNullOrEmpty(m.MovieImage))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", m.MovieImage.TrimStart('/'));
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MoviesExists(mvm.MovieId))
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
            TempData["ChangeResult"] = "E_0";
            return View(mvm);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
           
            var movies = await _context.Movies
                .FirstOrDefaultAsync(m => m.MovieId == id);
            if (movies == null)
            {
                return NotFound();
            }
            try
            {
                _context.Movies.Remove(movies);
                Movies m = await _context.Movies.FindAsync(movies.MovieId);
                await _context.SaveChangesAsync();
                if (!string.IsNullOrEmpty(m.MovieImage))
                {
                    // 取得當前目錄,圖片存放路徑, 去掉路徑開頭的 / 符號，以防止路徑不正確
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", m.MovieImage.TrimStart('/'));
                    // 檢查舊圖片文件是否存在
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        // 存在則刪除照片
                        System.IO.File.Delete(oldFilePath);
                    }
                }
                TempData["Success"] = "1";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["Success"] = "0";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var movies = await _context.Movies.FindAsync(id);
            if (movies != null)
            {
                _context.Movies.Remove(movies);
            }

            await _context.SaveChangesAsync();

            Movies m = await _context.Movies.FindAsync(movies.MovieId);
            if (!string.IsNullOrEmpty(m.MovieImage))
            {
                // 取得當前目錄,圖片存放路徑, 去掉路徑開頭的 / 符號，以防止路徑不正確
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", m.MovieImage.TrimStart('/'));
                // 檢查舊圖片文件是否存在
                if (System.IO.File.Exists(oldFilePath))
                {
                    // 存在則刪除照片
                    System.IO.File.Delete(oldFilePath);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        private bool MoviesExists(int id)
        {
            return _context.Movies.Any(e => e.MovieId == id);
        }


    }
}
