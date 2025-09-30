using Attendance.Application.Interface;
using Attendance.Controllers;
using Attendance.Domain.Helper;
using Attendance.Domain.Models;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

public class RecordController : BaseClockInController
{
	private readonly IConfiguration _configuration;
	private ApplicationURL applicationURL;
	private readonly GlobalClass _globalClass;
	private readonly IAttendanceService _service;
	private readonly IMenuMasterService _menuService;
	private readonly IUserMenuMappingService _userMenuMappingService;
	public RecordController(IConfiguration configuration, GlobalClass globalClass, IAttendanceService service, IMenuMasterService menuService, IUserMenuMappingService userMenuMappingService, IEmployeeService employeeService) : base(menuService, userMenuMappingService)
	{
		_configuration = configuration;
		applicationURL = new ApplicationURL(configuration);
		_globalClass = globalClass;
		_service = service;
		_menuService = menuService;
		_userMenuMappingService = userMenuMappingService;
	}
	public IActionResult Record()
	{
		if (_globalClass.Token != null)
		{
			var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_globalClass.Token);
			var claims = UserUtility.addClaimstoUser(HttpContext, jwt.Claims);
			var employee = claims.Claims.FirstOrDefault(x => x.Type == "EmployeeId");
			int UserID = employee != null ? (!string.IsNullOrEmpty(employee.Value) ? Convert.ToInt32(employee.Value) : 0) : 0;
			ViewBag.UserID = UserID;
			ViewBag.appUrl = applicationURL.url;
		}
		return View();
	}
	public async Task<IActionResult> AllRecords()
	{
		if (_globalClass.Token != null)
		{
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

	[HttpGet]
	public async Task<IActionResult> GetAttendanceTableData(int UserID)
	{
		var records = await _service.GetAttendanceByEmployeeAsync(UserID);
		var filter = records.Where(r => r.ClockOut.HasValue);
		var result = filter.Select(r => new
		{
			date = r.Date.ToString("dd-MMM-yyyy"),
			clockIn = r.ClockIn.ToString(@"hh\:mm\:ss"),
			clockOut = r.ClockOut?.ToString(@"hh\:mm\:ss") ?? "-",
			totalTime = r.ClockOut.HasValue
				? (r.ClockOut.Value - r.ClockIn).ToString(@"hh\:mm\:ss")
				: "-",
			overtimeHours = r.OvertimeHours.HasValue
				? r.OvertimeHours.Value.ToString(@"hh\:mm\:ss")
				: "-"
		});

		return Json(result);
	}
	[HttpGet]
	public async Task<IActionResult> GetAttendanceByMonth(int userId, string month)
	{
		if (string.IsNullOrWhiteSpace(month))
			return BadRequest("Month is required.");
		if (!DateTime.TryParseExact(month + "-01", "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out DateTime startDate))
			return BadRequest("Invalid month format.");
		var endDate = startDate.AddMonths(1).AddDays(-1);
		var records = await _service.GetAttendanceByEmployeeAsync(userId);
		var filteredRecords = records
				.Where(r => r.Date >= startDate && r.Date <= endDate && r.ClockOut.HasValue)
				.Select(r => new
				{
					date = r.Date.ToString("dd-MMM-yyyy"),
					clockIn = r.ClockIn.ToString(@"hh\:mm\:ss"),
					clockOut = r.ClockOut?.ToString(@"hh\:mm\:ss") ?? "-",
					totalTime = (r.ClockOut.Value - r.ClockIn).ToString(@"hh\:mm\:ss"),
					overtimeHours = r.OvertimeHours.HasValue
						? r.OvertimeHours.Value.ToString(@"hh\:mm\:ss")
						: "-"
				});

		return Json(filteredRecords);
	}
	[HttpGet]
	public async Task<IActionResult> GetAvailableMonths()
	{
		var jwt = new JwtSecurityTokenHandler().ReadJwtToken(_globalClass.Token);
		var claims = UserUtility.addClaimstoUser(HttpContext, jwt.Claims);
		var user = claims.Claims.FirstOrDefault(x => x.Type == "UserId");
		int userId = user != null ? (!string.IsNullOrEmpty(user.Value) ? Convert.ToInt32(user.Value) : 0) : 0;
		var records = await _service.GetAttendanceByEmployeeAsync(userId);
		var months = records
			.Select(r => r.Date.ToString("MMMM yyyy"))
			.Distinct()
			.OrderByDescending(m => m)
			.ToList();

		return Json(months);
	}
}