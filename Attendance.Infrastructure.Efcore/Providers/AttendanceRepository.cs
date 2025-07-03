using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Attendance.Infrastructure.Efcore.Providers
{
	public class AttendanceRepository : IAttendanceRepository
	{
		private readonly AppDbContext _context;
		public AttendanceRepository(AppDbContext context)
		{
			_context = context;
		}

		public async Task<AttendanceRecord> ClockInAsync(int employeeId, DateTime clockIn)
		{
			try
			{
				// Check if employee exists
				var user = await _context.Users.FindAsync(employeeId);
				if (user == null)
				{
					throw new Exception($"EmployeeId {employeeId} does not exist in UserRegister table.");
				}
				var record = new AttendanceRecord
				{
					EmployeeId = employeeId,
					ClockIn = clockIn,
					Date = clockIn.Date
				};
				_context.AttendanceRecords.Add(record);
				await _context.SaveChangesAsync();
				return record;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"[AttendanceRepository.ClockInAsync] Exception: {ex.Message}");
				throw;
			}
		}

		public async Task<AttendanceRecord> ClockOutAsync(int employeeId, DateTime clockOut)
		{
			var record = await _context.AttendanceRecords
				.Where(r => r.EmployeeId == employeeId && r.Date == clockOut.Date && r.ClockOut == null)
				.OrderByDescending(r => r.ClockIn)
				.FirstOrDefaultAsync();
			if (record != null)
			{
				record.ClockOut = clockOut;
				_context.Update(record);
				await _context.SaveChangesAsync();
			}
			return record;
		}

		public async Task<List<AttendanceRecord>> GetAttendanceByEmployeeAsync(int employeeId)
		{
			return await _context.AttendanceRecords
				.Where(r => r.EmployeeId == employeeId)
				.OrderByDescending(r => r.Date)
				.ToListAsync();
		}

		public async Task<bool> IsUserClockedIn(int userId)
		{
			try
			{
				var result = await _context.AttendanceRecords
					.Where(r => r.EmployeeId == userId && r.Date.Date.Date == DateTime.Now.Date && r.ClockOut == null)
					.ToListAsync();
				return result == null ? false : true;

			}
			catch (Exception ex)
			{
				throw;
			}
		}
		public async Task<List<AttendanceRecord>> GetLastFiveAttendanceRecordsAsync(int employeeId)
		{
			return await _context.AttendanceRecords
				.Where(r => r.EmployeeId == employeeId)
				.OrderByDescending(r => r.ClockIn)
				.Take(5)
				.ToListAsync();
		}

	}
}
