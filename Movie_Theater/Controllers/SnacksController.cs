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
    public class SnacksController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public SnacksController(Movie_TheaterContext context)
        {
            _context = context;
        }

        // GET: Snacks
        public async Task<IActionResult> Index()
        {
            return View(await _context.Snacks.ToListAsync());
        }

        // GET: Snacks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var snacks = await _context.Snacks
                .FirstOrDefaultAsync(m => m.SnackId == id);
            if (snacks == null)
            {
                return NotFound();
            }

            return View(snacks);
        }

        // GET: Snacks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Snacks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("SnackId,SnackName,SnackImages,Price")] Snacks snacks)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    // 判斷是否有上傳檔案
                    if (Request.Form.Files["SnackImages"] != null)
                    {
                        // 取得照片欄位名稱
                        var pictureFile = Request.Form.Files["SnackImages"];

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
                        snacks.SnackImages = "/images/" + uniqueFileName;

                    }

                    _context.Add(snacks);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SnacksExists(snacks.SnackId))
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
            return View(snacks);
        }

        // GET: Snacks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var snacks = await _context.Snacks.FindAsync(id);
            if (snacks == null)
            {
                return NotFound();
            }
            return View(snacks);
        }

        // POST: Snacks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("SnackId,SnackName,SnackImages,Price")] Snacks snacks)
        {
            if (id != snacks.SnackId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 取出原先所有資料
                    Snacks s = await _context.Snacks.FindAsync(snacks.SnackId);
                    // 判斷是否有上傳檔案
                    if (Request.Form.Files["SnackImages"] != null)
                    {
                        // 取得照片欄位名稱
                        var pictureFile = Request.Form.Files["SnackImages"];

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
                        snacks.SnackImages = "/images/" + uniqueFileName;
                    }
                    else
                    {
                        // 放入原先資料
                        snacks.SnackImages = s.SnackImages;
                    }
                    // 解除追中
                    _context.Entry(s).State = EntityState.Detached;

                    _context.Update(snacks);
                    await _context.SaveChangesAsync();
                    // 刪除舊圖片文件（如果有）
                    // 判斷是否原有圖片
                    if (!string.IsNullOrEmpty(s.SnackImages))
                    {
                        // 取得當前目錄,圖片存放路徑, 去掉路徑開頭的 / 符號，以防止路徑不正確
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", s.SnackImages.TrimStart('/'));
                        // 檢查舊圖片文件是否存在
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            // 存在則刪除照片
                            System.IO.File.Delete(oldFilePath);
                        }
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SnacksExists(snacks.SnackId))
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
            return View(snacks);
        }

        // GET: Snacks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}

            var snacks = await _context.Snacks
               .FirstOrDefaultAsync(m => m.SnackId == id);
            if (snacks == null)
            {
                return NotFound();
            }
            try
            {
                _context.Snacks.Remove(snacks);
                Snacks s = await _context.Snacks.FindAsync(snacks.SnackId);
                await _context.SaveChangesAsync();
                if (!string.IsNullOrEmpty(s.SnackImages))
                {
                    // 取得當前目錄,圖片存放路徑, 去掉路徑開頭的 / 符號，以防止路徑不正確
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", s.SnackImages.TrimStart('/'));
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

        // POST: Snacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var snacks = await _context.Snacks.FindAsync(id);
            if (snacks != null)
            {
                _context.Snacks.Remove(snacks);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SnacksExists(int id)
        {
            return _context.Snacks.Any(e => e.SnackId == id);
        }
    }
}
