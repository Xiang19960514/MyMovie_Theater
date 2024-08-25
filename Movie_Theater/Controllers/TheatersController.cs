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
    public class TheatersController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public TheatersController(Movie_TheaterContext context)
        {
            _context = context;
        }

        // 更新圖片
        async Task<string> UpdateImage(string imageName)
        {
            // 取得照片欄位名稱
            var pictureFile = Request.Form.Files[imageName];

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
            return uniqueFileName;
        }

        async Task DeleteImage(string image)
        {
            // 判斷是否原有圖片
            if (!string.IsNullOrEmpty(image))
            {
                // 取得當前目錄,圖片存放路徑, 去掉路徑開頭的 / 符號，以防止路徑不正確
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", image.TrimStart('/'));
                // 檢查舊圖片文件是否存在
                if (System.IO.File.Exists(oldFilePath))
                {
                    // 存在則刪除照片
                    System.IO.File.Delete(oldFilePath);
                }
            }
        }

        // GET: Theaters
        public async Task<IActionResult> Index()
        {
            return View(_context.Theaters);
        }

        // GET: Theaters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theaters = await _context.Theaters
                .FirstOrDefaultAsync(m => m.TheaterId == id);
            if (theaters == null)
            {
                return NotFound();
            }

            return View(theaters);
        }

        // GET: Theaters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Theaters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TheaterId,TheaterName,TheaterPhone,TheaterEmail,TheaterLocation,TheaterDescription,TheaterStartTime,TheaterEndTime,TheaterImage")] Theaters theaters)
        {
            if (ModelState.IsValid)
            {
                try
                { 
                    // 判斷是否有上傳檔案
                    if (Request.Form.Files["TheaterImage"] != null)
                    {
                        theaters.TheaterImage = await UpdateImage("TheaterImage");
                    }

                    _context.Add(theaters);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheatersExists(theaters.TheaterId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
				TempData["ChangeResult"] = "C_1";
				return RedirectToAction(nameof(Index));

            }
			TempData["ChangeResult"] = "C_0";
			return View(theaters);
        }

        // GET: Theaters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theaters = await _context.Theaters.FindAsync(id);
            if (theaters == null)
            {
                return NotFound();
            }
            return View(theaters);
        }

        // POST: Theaters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TheaterId,TheaterName,TheaterPhone,TheaterEmail,TheaterLocation,TheaterDescription,TheaterStartTime,TheaterEndTime,TheaterImage")] Theaters theaters)
        {
            if (id != theaters.TheaterId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 取出原先所有資料
                    Theaters t = await _context.Theaters.FindAsync(theaters.TheaterId);
                    // 判斷是否有上傳檔案
                    if (Request.Form.Files["TheaterImage"] != null)
                    {
                        theaters.TheaterImage = await UpdateImage("TheaterImage");
                    }
                    else
                    {
                        // 放入原先資料
                        theaters.TheaterImage = t.TheaterImage;
                    }
                    // 解除追中
                    _context.Entry(t).State = EntityState.Detached;

                    _context.Update(theaters);
                    await _context.SaveChangesAsync();
                    // 判斷是否有上傳檔案
                    if (Request.Form.Files["TheaterImage"] != null)
                    {
                        // 刪除舊圖片文件（如果有）
                        await DeleteImage(t.TheaterImage);
                    }
                    
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TheatersExists(theaters.TheaterId))
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
            return View(theaters);
        }

        // GET: Theaters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var theaters = await _context.Theaters.FindAsync(id);

            if (theaters == null)
            {
                return NotFound();

            }
            try
            {
                //_context.Theaters.Remove(theaters);
                //await _context.SaveChangesAsync();
                //await DeleteImage(theaters.TheaterImage);
                TempData["ChangeResult"] = "D_1";
            }
            catch (DbUpdateConcurrencyException)
            {
                TempData["ChangeResult"] = "D_0";
            }

            return RedirectToAction(nameof(Index));

        }

        //// POST: Theaters/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var theaters = await _context.Theaters.FindAsync(id);

        //    if (theaters == null)
        //    {
        //        return NotFound();

        //    }

        //    _context.Theaters.Remove(theaters);
        //    await _context.SaveChangesAsync();
        //    await DeleteImage(theaters.TheaterImage);

        //    return RedirectToAction(nameof(Index));

        //}

        private bool TheatersExists(int id)
        {
            return _context.Theaters.Any(e => e.TheaterId == id);
        }
    }
}
