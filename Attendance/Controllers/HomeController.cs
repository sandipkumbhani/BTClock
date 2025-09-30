using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult AccessDenied()
        {
            return View();
        }
       
    }
}
