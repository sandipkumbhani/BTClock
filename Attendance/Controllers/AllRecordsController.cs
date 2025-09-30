using Attendance.Application.Interface;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace Attendance.Controllers
{
	public class AllRecordsController : BaseClockInController
	{
		private readonly IConfiguration _configuration;
		private ApplicationURL applicationURL;
		private readonly GlobalClass _globalClass;
		private readonly IAttendanceService _service;
		private readonly IMenuMasterService _menuService;
		private readonly IUserMenuMappingService _userMenuMappingService;
		private readonly IEmployeeService _employeeService;

		public AllRecordsController(IConfiguration configuration, GlobalClass globalClass, IAttendanceService service, IMenuMasterService menuService, IUserMenuMappingService userMenuMappingService, IEmployeeService employeeService) : base(menuService, userMenuMappingService)
		{
			_configuration = configuration;
			applicationURL = new ApplicationURL(configuration);
			_globalClass = globalClass;
			_service = service;
			_menuService = menuService;
			_userMenuMappingService = userMenuMappingService;
			_employeeService = employeeService;
		}
		public async Task<IActionResult> AllRecords()
		{
			if (_globalClass.Token != null)
			{
				var employees = await _employeeService.GetAllEmployee();
				ViewBag.EmployeeList = employees;
				var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_globalClass.Token);
				var claims = UserUtility.addClaimstoUser(HttpContext, jwt.Claims);
				ViewBag.appUrl = applicationURL.url;
			}
			return View();
		}
		[HttpGet]
		public async Task<IActionResult> GetAllAttendanceTableData()
		{
			var records = await _service.GetAllAttendanceAsync();
			var result = records
				.Where(r => r.ClockOut.HasValue)
				.Select(r => new
				{
					id = r.Id,
					EmployeeId = r.EmployeeId,
					EmployeeName = r.employee.Name.ToString(), // Replace with actual employee name retrieval if needed
					date = r.Date.ToString("dd-MMM-yyyy"),
					clockIn = r.ClockIn.ToString(@"hh\:mm\:ss"),
					clockOut = r.ClockOut?.ToString(@"hh\:mm\:ss") ?? "-",
					totalTime = r.ClockOut.HasValue
						? (r.ClockOut.Value - r.ClockIn).ToString(@"hh\:mm\:ss")
						: "-",
					overtimeHours = r.OvertimeHours.HasValue
						? r.OvertimeHours.Value.ToString(@"hh\:mm\:ss")
						: "-"
				})
				.ToList();

			return Json(result);
		}
		public IActionResult AttendanceRecord()
		{
			return View();
		}
		public async Task<IActionResult> UpdateAttendanceRecord(int id)
		{
			var attendanceRecord = await _service.GetAttendanceById(id);
			if (attendanceRecord == null)
			{
				return NotFound();
			}

			if (attendanceRecord.ClockIn != default && attendanceRecord.ClockOut.HasValue)
			{
				var duration = attendanceRecord.ClockOut.Value - attendanceRecord.ClockIn;
				attendanceRecord.WorkingHour = duration;

				var standardWorkingHour = TimeSpan.FromHours(9);
				attendanceRecord.OvertimeHours = duration > standardWorkingHour
					? duration - standardWorkingHour
					: TimeSpan.Zero;
			}

			return View(attendanceRecord);
		}
		[HttpPost]
		public async Task<IActionResult> UpdateAttendanceRecord(AttendanceRecordDto model)
		{
			if (ModelState.IsValid)
			{
				var existingRecord = await _service.GetAttendanceById(model.Id);
				if (existingRecord == null)
				{
					return NotFound();
				}

				TimeSpan workingHour = TimeSpan.Zero;
				TimeSpan overtime = TimeSpan.Zero;

				if (model.ClockIn != default && model.ClockOut.HasValue)
				{
					workingHour = model.ClockOut.Value - model.ClockIn;
					var standardWorkingHour = TimeSpan.FromHours(9);
					overtime = workingHour > standardWorkingHour ? workingHour - standardWorkingHour : TimeSpan.Zero;
				}

				existingRecord.EmployeeId = model.EmployeeId;
				existingRecord.Date = model.Date;
				existingRecord.ClockIn = model.ClockIn;
				existingRecord.ClockOut = model.ClockOut;
				existingRecord.WorkingHour = workingHour;
				existingRecord.OvertimeHours = overtime;
				existingRecord.UpdatedAt = DateTime.Now;
				existingRecord.CreatedAt = existingRecord.CreatedAt;
				existingRecord.CreatedBy = existingRecord.CreatedBy;
				existingRecord.UpdatedBy = UserUtility.GetUserId(HttpContext);
				await _service.UpdateAttendance(existingRecord, existingRecord.Id);

				TempData["msg"] = "Data updated successfully !";
			}
			else
			{
				TempData["errormsg"] = "Fill The Form.";
			}

			var attendanceRecord = await _service.GetAttendanceById(model.Id);
			return RedirectToAction("AllRecords");
		}

	}
}