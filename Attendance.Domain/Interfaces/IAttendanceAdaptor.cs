using Attendance.Domain.Models;

namespace Attendance.Domain.Interfaces
{
    public interface IAttendanceAdaptor
    {
        Task<AttendanceRecordDto> ClockInAsync();
        Task<AttendanceRecordDto> ClockOutAsync();
        Task<List<AttendanceRecordDto>> GetAttendanceByEmployeeAsync(int employeeId);
        Task<List<AttendanceRecordDto>> GetLastFiveAttendanceRecordsAsync(int employeeId);
        Task<bool> IsUserClockedIn();
        //Task<List<AttendanceRecord>> GetAllOpenAttendancesAsync();
        //Task<AttendanceRecord?> GetClockInRecord(int EmployeeId);

	}
}
