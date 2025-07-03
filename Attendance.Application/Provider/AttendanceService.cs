using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;

namespace Attendance.Application.Provider
{
	public class AttendanceService : IAttendanceService
	{
		private readonly IAttendanceRepository _repo;
		public AttendanceService(IAttendanceRepository repo)
		{
			_repo = repo;
		}

		public Task<AttendanceRecord> ClockInAsync(int employeeId, DateTime clockIn)
			=> _repo.ClockInAsync(employeeId, clockIn);

		public Task<AttendanceRecord> ClockOutAsync(int employeeId, DateTime clockOut)
			=> _repo.ClockOutAsync(employeeId, clockOut);

		public Task<List<AttendanceRecord>> GetAttendanceByEmployeeAsync(int employeeId)
			=> _repo.GetAttendanceByEmployeeAsync(employeeId);

		public Task<bool> IsUserClockedIn(int userId)
			=> _repo.IsUserClockedIn(userId);
		public Task<List<AttendanceRecord>> GetLastFiveAttendanceRecordsAsync(int employeeId)
			=> _repo.GetLastFiveAttendanceRecordsAsync(employeeId);


	}
}
