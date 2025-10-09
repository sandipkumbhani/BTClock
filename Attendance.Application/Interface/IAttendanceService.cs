using Attendance.Domain.Models;

namespace Attendance.Application.Interface
{
    public interface IAttendanceService
    {
        Task<AttendanceRecordDto> ClockInAsync();
        Task<AttendanceRecordDto> ClockOutAsync();
        Task<List<AttendanceRecordDto>> GetAttendanceByUserAsync(int userId);
        Task<List<AttendanceRecordDto>> GetLastFiveAttendanceRecordsAsync(int userId);
		//Task AutoClockOutAsync(int employeeId);
		Task<List<AttendanceRecordDto>> GetAllAttendanceAsync();

		Task<bool> IsUserClockedIn();
		Task<string> UpdateAttendance(AttendanceRecordDto record, int id);
		Task<AttendanceRecordDto> GetAttendanceById(int id);


		//Task<List<AttendanceRecord>> GetAllOpenAttendancesAsync(); 
		//Task<AttendanceRecord> ClockOutJobAsync(int employeeId, DateTime clockOut);

	}
}
