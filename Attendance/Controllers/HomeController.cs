using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
