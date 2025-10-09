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

        public async Task<List<AttendanceRecordDto>> GetAttendanceByUserAsync(int userId)
        {
            return await _adaptor.GetAttendanceByUserAsync(userId);
        }

        public Task<bool> IsUserClockedIn()
        {
            return _adaptor.IsUserClockedIn();
        }

        public async Task<List<AttendanceRecordDto>> GetLastFiveAttendanceRecordsAsync(int userId)
        {
            return await _adaptor.GetLastFiveAttendanceRecordsAsync(userId);

        }
        public async Task<List<AttendanceRecordDto>> GetAllAttendanceAsync()
		{
			return await _adaptor.GetAllAttendanceAsync();
		}
		public async Task<AttendanceRecordDto> GetAttendanceById(int id)
		{
			return await _adaptor.GetAttendanceByIdAsync(id);
		}

		public async Task<string> UpdateAttendance(AttendanceRecordDto record, int id)
		{
			return await _adaptor.UpdateAttendanceRecordAsync(record, id);
		}

	}
}
