using Attendance.Application.Interface;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Attendance.UI.Domain.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Attendance.Controllers 
{
    [Authorize]
    public class AttendanceController : Controller
    {
        private readonly IConfiguration _configuration;
        private ApplicationURL applicationURL;
        private readonly GlobalClass _globalClass;
        private readonly IAttendanceService _service;

        public AttendanceController(IConfiguration configuration, GlobalClass globalClass, IAttendanceService service)
        {
            _configuration = configuration;
            _globalClass = globalClass;
            _service = service;
            applicationURL = new ApplicationURL(configuration);
        }
        public IActionResult ClockIn()
        {
            if (_globalClass.Token != null)
            {
                var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_globalClass.Token);
                var claims = UserUtility.addClaimstoUser(HttpContext, jwt.Claims);
                string currentPage = "Clock-In";
                ViewBag.appUrl = applicationURL.url;
                //var canAccess = UserUtility.CanAccessMenu(HttpContext, currentPage);
                //if (canAccess == true)
                //{
                //    var employee = claims.Claims.FirstOrDefault(x => x.Type == "EmployeeId");
                //    int EmployeeId = employee != null ? (!string.IsNullOrEmpty(employee.Value) ? Convert.ToInt32(employee.Value) : 0) : 0;
                //    ViewBag.EmployeeId = EmployeeId;
                //    ViewBag.appUrl = applicationURL.url;
                //    return View();
                //}
                //else
                //{
                //    return RedirectToAction("AccessDenied", "Home");
                //}
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ClockInAttendance(int employeeId)
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
        public async Task<IActionResult> ClockOutAttendance(int employeeId)
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
        [HttpGet]
        public async Task<IActionResult> Report(int employeeId)
        {
            if (_globalClass.Token != null)
            {
                var report = await _service.GetAttendanceByEmployeeAsync(employeeId);
                var topFive = report.OrderByDescending(x => x.ClockIn).Take(6).ToList();
                ViewBag.appUrl = applicationURL.url;
                return Json(topFive);
            }
            return Json(null);
        }

    }
}
