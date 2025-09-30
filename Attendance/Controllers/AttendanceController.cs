using Attendance.Application.Interface;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Attendance.Domain.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Attendance.Controllers
{
    [Authorize]
    public class AttendanceController : BaseClockInController
    {
        private readonly IConfiguration _configuration;
        private ApplicationURL applicationURL;
        private readonly GlobalClass _globalClass;
        private readonly IAttendanceService _service;
        private readonly IMenuMasterService _menuService;
        private readonly IUserMenuMappingService _userMenuMappingService;


        public AttendanceController(IConfiguration configuration, GlobalClass globalClass, IAttendanceService service, IMenuMasterService menuService, IUserMenuMappingService userMenuMappingService) : base(menuService, userMenuMappingService)
        {
            _configuration = configuration;
            _globalClass = globalClass;
            _service = service;
            applicationURL = new ApplicationURL(configuration);
            _menuService = menuService;
            _userMenuMappingService = userMenuMappingService;
        }
        public IActionResult ClockIn()
        {
            if (_globalClass.Token != null)
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_globalClass.Token);
                var claims = UserUtility.addClaimstoUser(HttpContext, jwt.Claims);
                string currentPage = "Clockin";
                var canAccess = UserUtility.CanAccessMenu(HttpContext, currentPage);
                if (canAccess == true)
                {
                    var employee = claims.Claims.FirstOrDefault(x => x.Type == "EmployeeId");
                    int EmployeeId = employee != null ? (!string.IsNullOrEmpty(employee.Value) ? Convert.ToInt32(employee.Value) : 0) : 0;
                    ViewBag.EmployeeId = EmployeeId;
                    ViewBag.appUrl = applicationURL.url;
                    return View();
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Home");
                }

            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ClockInAttendance()
        {
            try
            {

                if (_globalClass.Token != null)
                {
                    var clockin = await _service.ClockInAsync();
                    if (clockin != null)
                    {
                        return Json(new { clockIn = clockin.ClockIn, message = "OK" });
                    }
                }
                return Json(new { clockIn = (DateTime?)null });
            }
            catch (Exception ex)
            {
                return Json(new { clockIn = (DateTime?)null, message = ex.Message });
            }
        }
        [HttpGet]
        public async Task<IActionResult> ClockOutAttendance()
        {
            if (_globalClass.Token != null)
            {
                var clockout = await _service.ClockOutAsync();
                if (clockout != null)
                {
                    return Json(new { clockOut = clockout.ClockOut, clockIn = clockout.ClockIn });
                }
            }
            return Json(new { clockOut = (DateTime?)null, clockIn = (DateTime?)null });
        }
        //[HttpGet]
        //public async Task<IActionResult> Report(int employeeId)
        //{
        //    if (_globalClass.Token != null)
        //    {
        //        var report = await _service.GetAttendanceByEmployeeAsync(employeeId);
        //        var topFive = report.OrderByDescending(x => x.ClockIn).Take(6).ToList();
        //        ViewBag.appUrl = applicationURL.url;
        //        return Json(topFive);
        //    }
        //    return Json(null);
        //}
        [HttpGet]
        public async Task<IActionResult> Report(int employeeId)
        {
            if (_globalClass.Token != null)
            {
                var report = await _service.GetAttendanceByEmployeeAsync(employeeId);

                var topFive = report.OrderByDescending(x => x.ClockIn).Take(5).ToList();

                var enrichedRecords = topFive.Select(x =>
                {
                    TimeSpan? duration = null;
                    TimeSpan? overtime = null;

                    if (x.ClockIn != null && x.ClockOut != null)
                    {
                        duration = x.ClockOut.Value - x.ClockIn;
                        var standardHours = new TimeSpan(9, 0, 0);
                        overtime = duration > standardHours ? duration - standardHours : TimeSpan.Zero;
                    }

                    return new  
                    {
                        x.ClockIn,
                        x.ClockOut,
                        OvertimeHours = overtime?.ToString(@"hh\:mm\:ss"),
                        WorkingHour = duration?.ToString(@"hh\:mm\:ss")
                    };
                });

                return Json(enrichedRecords);
            }

            return Json(null);
        }

        
	}
}
