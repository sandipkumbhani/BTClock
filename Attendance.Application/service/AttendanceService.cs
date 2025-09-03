using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;

namespace Attendance.Application.Provider
{
	public class AttendanceService : IAttendanceService
	{
        private readonly IAttendanceAdaptor _adaptor;

        public AttendanceService(IAttendanceAdaptor adaptor)
        {
            _adaptor = adaptor;
        }

        public async Task<AttendanceRecordDto> ClockInAsync()
        {
            return await _adaptor.ClockInAsync();
        }

        public async Task<AttendanceRecordDto> ClockOutAsync()
        {

            return await _adaptor.ClockOutAsync();
        }

        public async Task<List<AttendanceRecordDto>> GetAttendanceByEmployeeAsync(int employeeId)
        {
            return await _adaptor.GetAttendanceByEmployeeAsync(employeeId);
        }

        public Task<bool> IsUserClockedIn()
        {
            return _adaptor.IsUserClockedIn();
        }

        public async Task<List<AttendanceRecordDto>> GetLastFiveAttendanceRecordsAsync(int employeeId)
        {
            return await _adaptor.GetLastFiveAttendanceRecordsAsync(employeeId);

        }


    }
}
