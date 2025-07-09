using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            // Get EmployeeId from claims (NameIdentifier)
            var employeeIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(employeeIdStr) || !int.TryParse(employeeIdStr, out int employeeId) || employeeId <= 0)
            {
                // If claim is missing or invalid, redirect to login
                return RedirectToAction("Login", "Auth");
            }
            ViewBag.EmployeeId = UserUtility.GetUserId(HttpContext.User);
            return View();
        }
    }
}
