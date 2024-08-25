using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movie_Theater.Models;
using Movie_Theater.ViewModels;

namespace Movie_Theater.Controllers
{
    public class LoginController : Controller
    {
		private readonly Movie_TheaterContext _context;

		public LoginController(Movie_TheaterContext context)
		{
			_context = context;
		}
		public IActionResult Login()
        {
            ViewBag.userExist = new { errorCode = 0, errorMessage = "" };

            return View();
        }

		[HttpPost]
		public async Task<IActionResult> Login(Login_ViewModel login_vm)
		{
			// 取出資料庫內資料
			var user = await _context.Admins.FindAsync(login_vm.Account);

			if (ModelState.IsValid)
			{
				// 判斷使用者是否存在
				if (user == null)
				{
					// 丟給畫面判斷錯誤
					ViewBag.userExist = new { errorCode = 1, errorMessage = "帳號不存在"};  // 1:帳號不存在
					return View();
				}
				// 取出使用者密碼
				var userPassword = await _context.Admins_Password.FindAsync(login_vm.Account);
				// 判斷密碼是否錯誤
				if (!BCrypt.Net.BCrypt.Verify(login_vm.AdminPassword, userPassword.AdminPassword))
				{
                    // 丟給畫面判斷錯誤
                    ViewBag.userExist = new { errorCode = 2, errorMessage = "密碼錯誤" };  // 2:密碼錯誤
                    return View();
				}
				// 登入成功建立Session
				HttpContext.Session.SetString("Login_User", $"{user.AdminName}");
				HttpContext.Session.SetString("User_Account", $"{user.Account}");
				return RedirectToAction("Index", "Home");
			}

			return View();
		}
        public IActionResult Logout()
        {
            // 清除所有 Session 數據
            HttpContext.Session.Clear();

            return RedirectToAction("Login", "Login");
        }
    }
}
