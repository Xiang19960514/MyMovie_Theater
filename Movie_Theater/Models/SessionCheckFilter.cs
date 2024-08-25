using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Movie_Theater.Models
{
    public class SessionCheckFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            // 獲取當前的控制器和動作名稱
            var controllerName = context.RouteData.Values["controller"].ToString();
            var actionName = context.RouteData.Values["action"].ToString();

            // 如果當前路由是登入頁面，則跳過檢查
            if (controllerName == "Login" && actionName == "Login")
            {
                return;
            }

            // 檢查 Session 中是否有用戶信息
            if (context.HttpContext.Session.GetString("Login_User") == null)
            {
                // 如果用戶未登入，重定向到登入頁面
                context.Result = new RedirectToActionResult("Login", "Login", null);
            }
        }
    }
}
