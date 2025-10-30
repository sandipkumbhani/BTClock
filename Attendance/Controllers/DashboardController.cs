using Attendance.Domain.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
	[Authorize]
	public class DashboardController : Controller
    {
        private ApplicationURL applicationURL;
        private readonly IConfiguration _configuration;

        public DashboardController( IConfiguration configuration)
        {
            _configuration = configuration;
            applicationURL = new ApplicationURL(configuration);
        }
        [AllowAnonymous]
        public async Task<IActionResult> Dashboard()
        {
            return View();
        }
    }
}
