using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Movie_Theater.Models;
using Movie_Theater.ViewModels;

namespace Movie_Theater.Controllers
{
    public class AdminsController : Controller
    {
        private readonly Movie_TheaterContext _context;

        public AdminsController(Movie_TheaterContext context)
        {
            _context = context;
        }

        // GET: Admins
        public async Task<IActionResult> Index()
        {
            //var movie_TheaterContext = _context.Admins.Include(a => a.Role);
            var admins_vm = _context.Admins.Select(a => new Admins_ViewModel
            {
                Account = a.Account,
                AdminName = a.AdminName,
                Email = a.Email,
                Phone = a.Phone,
                CreateAt = a.CreateAt
            });
            return View(admins_vm);
        }

        // GET: Admins/Details/5
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admins = await _context.Admins
                .Include(a => a.Role)
                .FirstOrDefaultAsync(m => m.Account == id);
            if (admins == null)
            {
                return NotFound();
            }

            return View(admins);
        }

        // GET: Admins/Create
        public IActionResult Create()
        {
            ViewData["Role_Id"] = new SelectList(_context.Roles, "RoleId", "RoleName");
            return View();
        }

        // POST: Admins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Account,Role_Id,AdminName,Email,Phone,CreateAt,UpdateAt,AdminPassword")] Admins_ViewModel admins_vm)
        {
            if (ModelState.IsValid)
            {
                Admins admins = new Admins
                {
                    Account = admins_vm.Account,
                    Role_Id = admins_vm.Role_Id,
                    AdminName = admins_vm.AdminName,
                    Email = admins_vm.Email,
                    Phone = admins_vm.Phone
                };
                _context.Add(admins);

                // 密碼加密
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(admins_vm.AdminPassword);

                //bool 驗證 = BCrypt.Net.BCrypt.Verify("123", hashedPassword);

                Admins_Password admins_Password = new Admins_Password
                {
                    Account = admins_vm.Account,
                    AdminPassword = hashedPassword
                };
                _context.Add(admins_Password);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch(Exception ex)
                {
                    return NotFound(ex);
                }
                TempData["ChangeResult"] = "C_1";
                return RedirectToAction(nameof(Index));
            }
            TempData["ChangeResult"] = "C_0";
            ViewData["Role_Id"] = new SelectList(_context.Roles, "RoleId", "RoleName", admins_vm.Role_Id);
            return View(admins_vm);
        }

        // GET: Admins/Edit/5
        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var admins = await _context.Admins.FindAsync(id);
            var adminsPassword = await _context.Admins_Password.FindAsync(id);

            if (admins == null || adminsPassword == null)
            {
                return NotFound();
            }

            Admins_ViewModel admins_vm = new Admins_ViewModel
            {
                Account = admins.Account,
                Role_Id = admins.Role_Id,
                AdminName = admins.AdminName,
                Email = admins.Email,
                Phone = admins.Phone,
            };
            
            ViewData["Role_Id"] = new SelectList(_context.Roles, "RoleId", "RoleDescription", admins.Role_Id);
            return View(admins_vm);
        }

        // POST: Admins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Account,Role_Id,AdminName,Email,Phone,CreateAt,UpdateAt,AdminPassword")] Admins_ViewModel admins_vm)
        {
            if (id != admins_vm.Account)
            {
                return NotFound();
            }
            var admin = await _context.Admins.FindAsync(admins_vm.Account);
            var adminPassword = await _context.Admins_Password.FindAsync(admins_vm.Account);

            if (admin == null || adminPassword == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    admin.Role_Id = admins_vm.Role_Id;
                    admin.AdminName = admins_vm.AdminName;
                    admin.Email = admins_vm.Email;
                    admin.Phone = admins_vm.Phone;
                    _context.Update(admin);
                    await _context.SaveChangesAsync();
                    if (admins_vm.AdminPassword != "******")
                    {
                        adminPassword.AdminPassword = admins_vm.AdminPassword;
                        _context.Update(adminPassword);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdminsExists(admins_vm.Account))
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
            ViewData["Role_Id"] = new SelectList(_context.Roles, "RoleId", "RoleDescription", admins_vm.Role_Id);
            return View(admins_vm);
        }

		// GET: Admins/Delete/5
		public async Task<IActionResult> Delete(string? id)
		{
			var admins = await _context.Admins.FindAsync(id);
			var adminsPassword = await _context.Admins_Password.FindAsync(id);
			if (admins != null && adminsPassword != null)
			{
                try
                {
					//_context.Admins_Password.Remove(adminsPassword);
					//await _context.SaveChangesAsync();
					//_context.Admins.Remove(admins);
					//await _context.SaveChangesAsync();
				}
                catch (DbUpdateConcurrencyException)
                {
                    TempData["ChangeResult"] = "D_0";
                }
				
			}
            TempData["ChangeResult"] = "D_1";
            return RedirectToAction(nameof(Index));
		}

		//// GET: Admins/Delete/5
		//public async Task<IActionResult> Delete(string? id)
		//{
		//    if (id == null)
		//    {
		//        return NotFound();
		//    }

		//    var admins = await _context.Admins
		//        .Include(a => a.Role)
		//        .FirstOrDefaultAsync(m => m.Account == id);
		//    if (admins == null)
		//    {
		//        return NotFound();
		//    }

		//    return View(admins);
		//}

		// POST: Admins/Delete/5
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var admins = await _context.Admins.FindAsync(id);
            if (admins != null)
            {
                _context.Admins.Remove(admins);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdminsExists(string id)
        {
            return _context.Admins.Any(e => e.Account == id);
        }
    }
}
