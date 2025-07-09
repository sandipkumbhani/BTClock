using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
	
namespace Attendance.Application.Provider
{
	public class AttendanceService : IAttendanceService
	{
		private readonly IAttendanceRepository _repo;
		private readonly IAttendanceSettingsProvider _settingsProvider;

		public AttendanceService(IAttendanceRepository repo, IAttendanceSettingsProvider settingsProvider)
		{
			_repo = repo;
			_settingsProvider = settingsProvider;
		}

		public async Task<AttendanceRecord> ClockInAsync(int employeeId, DateTime clockIn)
		{
			if (!_settingsProvider.AllowMultipleClockInOutPerDay)
			{
				var todayRecords = (await _repo.GetAttendanceByEmployeeAsync(employeeId))
					.Where(r => r.ClockIn.Date == clockIn.Date).ToList();
				if (todayRecords.Any())
				{
					// Already clocked in today

					throw new InvalidOperationException("Only one time clock-in allowed per day.");
				}
			}
			return await _repo.ClockInAsync(employeeId, clockIn);
		}

		public async Task<AttendanceRecord> ClockOutAsync(int employeeId, DateTime clockOut)
		{
			if (!_settingsProvider.AllowMultipleClockInOutPerDay)
			{
				var todayRecords = (await _repo.GetAttendanceByEmployeeAsync(employeeId))
					.Where(r => r.ClockIn.Date == clockOut.Date).ToList();
				if (!todayRecords.Any() || todayRecords.Any(r => r.ClockOut != null))
				{
					// No clock-in today or already clocked out
					throw new InvalidOperationException("Multiple clock-outs per day are not allowed.");
				}
			}
			return await _repo.ClockOutAsync(employeeId, clockOut);
		}

		public async Task<AttendanceRecord> ClockOutJobAsync(int employeeId, DateTime clockOut)
		{

			var todayRecords = await _repo.GetClockInRecord(employeeId);

			if (todayRecords != null)
			{
				var clockOutTimeDifference = (DateTime.Now - todayRecords.ClockIn).Minutes;
				if (clockOutTimeDifference >= 1)
				{
					return await _repo.ClockOutAsync(employeeId, todayRecords.ClockIn.AddMinutes(clockOutTimeDifference));
				}
			}
			return todayRecords!;

		}

		public Task<List<AttendanceRecord>> GetAttendanceByEmployeeAsync(int employeeId)
			=> _repo.GetAttendanceByEmployeeAsync(employeeId);

		public Task<bool> IsUserClockedIn(int userId)
			=> _repo.IsUserClockedIn(userId);
		public Task<List<AttendanceRecord>> GetLastFiveAttendanceRecordsAsync(int employeeId)
			=> _repo.GetLastFiveAttendanceRecordsAsync(employeeId);

		//public async Task AutoClockOutAsync(int employeeId)
		//{
		//	// This method should only process records for the given employeeId
		//	var recordsToUpdate = await _repo.GetAttendanceByEmployeeAsync(employeeId);
		//	var now = DateTime.Now;

		//	foreach (var record in recordsToUpdate.Where(r => r.ClockOut == null && r.ClockIn.Date < now.Date))
		//	{
		//		record.ClockOut = record.ClockIn.Date.AddDays(1).Date; // Set ClockOut to midnight of the next day
		//		await _repo.ClockOutAsync(record.EmployeeId, record.ClockOut.Value);
		//	}
		//}

		public async Task<List<AttendanceRecord>> GetAllOpenAttendancesAsync()
		{
			// Get all records where ClockOut is null
			return await _repo.GetAllOpenAttendancesAsync();
		}
	}
}
