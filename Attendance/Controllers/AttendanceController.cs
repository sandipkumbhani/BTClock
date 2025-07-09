using Attendance.Application.Interface;
using Attendance.Domain.Utility;
using Microsoft.AspNetCore.Mvc;

namespace Attendance.Controllers
{
	[Route("[controller]/[action]")]
	public class AttendanceController : Controller
	{
		private readonly IAttendanceService _service;
		public AttendanceController(IAttendanceService service)
		{
			_service = service;
		}


		[HttpGet]
		public async Task<IActionResult> ClockIn()
		{
			try
			{
				int userId = UserUtility.GetUserId(HttpContext.User);

				if (userId == 0)
				{
					return BadRequest(new { error = "Invalid user ID in claims." });
				}
				var record = await _service.ClockInAsync(userId, DateTime.Now);
				return Ok(record);
			}
			catch (InvalidOperationException ex)
			{
				return BadRequest(new { error = ex.Message });
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"[ClockIn] Exception: {ex.Message}");
				return StatusCode(500, new { error = "Server error: " + ex.Message });
			}
		}

		[HttpGet]
		public async Task<IActionResult> ClockOut()
		{
			try
			{
				var userId = UserUtility.GetUserId(HttpContext.User);

				if (userId == 0)
				{
					return BadRequest(new { error = "Invalid user ID in claims." });
				}
				var record = await _service.ClockOutAsync(userId, DateTime.Now);
				if (record == null) return NotFound();
				return Ok(record);
			}
			catch (InvalidOperationException ex)
			{
				// Only show the message, not a server error
				return BadRequest(new { error = ex.Message });
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"[ClockOut] Exception: {ex.Message}");
				return StatusCode(500, new { error = "Server error: " + ex.Message });
			}
		}

		[HttpGet]
		public async Task<IActionResult> Report(int employeeId)
		{
			var userId = UserUtility.GetUserId(HttpContext.User);
			var records = await _service.GetAttendanceByEmployeeAsync(userId);
			return Ok(records);
		}
		
		
		[HttpGet]
		public async Task<IActionResult> IsUserClockedIn()
		{
			var userId = UserUtility.GetUserId(HttpContext.User);
			var records = await _service.IsUserClockedIn(userId);
			return Ok(records);
		}
	}
}
