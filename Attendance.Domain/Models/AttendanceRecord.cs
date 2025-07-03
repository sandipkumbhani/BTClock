using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Domain.Models
{
	public class AttendanceRecord
	{
		public int Id { get; set; }
		public int EmployeeId { get; set; } // FK to UserRegister
		[ForeignKey("EmployeeId")]
		public Users User { get; set; }
		public DateTime Date { get; set; }
		public DateTime ClockIn { get; set; }
		public DateTime? ClockOut { get; set; }
	}
}
