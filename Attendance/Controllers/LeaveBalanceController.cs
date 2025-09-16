using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
    public class LeaveBalanceController : Controller
    {
        private readonly ILogger<UserMenuMappingController> _logger;
        private readonly IConfiguration _configuration;
        private ApplicationURL applicationURL;
        private readonly GlobalClass _globalClass;
        private readonly ILeaveBalanceService _leaveBalanceService;

        public LeaveBalanceController(ILogger<UserMenuMappingController> logger, IConfiguration configuration, GlobalClass globalClass, ILeaveBalanceService leaveBalanceService)
        {
            _logger = logger;
            _configuration = configuration;
            _globalClass = new GlobalClass();
            applicationURL = new ApplicationURL(configuration);
            _leaveBalanceService = leaveBalanceService;
        }

        public async Task<IActionResult> LeaveBalance()
        {
            var leaveBalances = await _leaveBalanceService.GetAllLeaveBalances();
            ViewBag.LeaveBalances = leaveBalances;
            return View();
        }

    }
}
