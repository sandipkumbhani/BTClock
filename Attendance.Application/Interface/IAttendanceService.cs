using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Attendance.Domain.Models;

namespace Attendance.Application.Interface
{
    public interface IAttendanceService
    {
        Task<AttendanceRecord> ClockInAsync(int employeeId, DateTime clockIn);
        Task<AttendanceRecord> ClockOutAsync(int employeeId, DateTime clockOut);
        Task<List<AttendanceRecord>> GetAttendanceByEmployeeAsync(int employeeId);
		Task<List<AttendanceRecord>> GetLastFiveAttendanceRecordsAsync(int employeeId);

		Task<bool> IsUserClockedIn(int userId);
	}
}
