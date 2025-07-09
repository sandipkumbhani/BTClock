using Attendance.Domain.Models;

namespace Attendance.Domain.Interfaces
{
    public interface IAttendanceRepository
    {
        Task<AttendanceRecord> ClockInAsync(int employeeId, DateTime clockIn);
        Task<AttendanceRecord> ClockOutAsync(int employeeId, DateTime clockOut);
        Task<List<AttendanceRecord>> GetAttendanceByEmployeeAsync(int employeeId);
        Task<List<AttendanceRecord>> GetLastFiveAttendanceRecordsAsync(int employeeId);
        Task<bool> IsUserClockedIn(int employeeId);
        Task<List<AttendanceRecord>> GetAllOpenAttendancesAsync();
        Task<AttendanceRecord?> GetClockInRecord(int employeeId);

	}
}
