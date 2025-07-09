using Attendance.Domain.Models;

namespace Attendance.Application.Interface
{
    public interface IAttendanceService
    {
        Task<AttendanceRecord> ClockInAsync(int employeeId, DateTime clockIn);
        Task<AttendanceRecord> ClockOutAsync(int employeeId, DateTime clockOut);
        Task<List<AttendanceRecord>> GetAttendanceByEmployeeAsync(int employeeId);
        Task<List<AttendanceRecord>> GetLastFiveAttendanceRecordsAsync(int employeeId);
        //Task AutoClockOutAsync(int employeeId);
        Task<bool> IsUserClockedIn(int userId);
        Task<List<AttendanceRecord>> GetAllOpenAttendancesAsync();
        Task<AttendanceRecord> ClockOutJobAsync(int employeeId, DateTime clockOut);

	}
}
