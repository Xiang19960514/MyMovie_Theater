using Microsoft.AspNetCore.Mvc;

namespace Movie_Theater.Controllers
{
    public class ChatController : Controller
    {
        public IActionResult Chat()
        {
            return View();
        }
        public IActionResult Status()
        {
            return Ok(new { status = "Server is running" });
        }
    }
}
