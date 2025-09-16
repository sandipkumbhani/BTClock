using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Domain.Models
{
    public class AttendanceRecord
    {
        public AttendanceRecord()
        {
            attendanceRecordDto = new List<AttendanceRecordDto>();
        }
        public List<AttendanceRecordDto> attendanceRecordDto { get; set; }


    }
    public class AttendanceRecordDto
    {
        public int Id { get; set; }
        [ForeignKey("EmployeeId")]
        public int EmployeeId { get; set; }
        public Employee? employee { get; set; }
        public DateTime Date { get; set; }
        public DateTime ClockIn { get; set; }
        public DateTime? ClockOut { get; set; }
        public TimeSpan? WorkingHour { get; set; }
        public TimeSpan? OvertimeHours { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedAt { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
