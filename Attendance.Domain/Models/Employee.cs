using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Domain.Models
{
	public class Employee
	{
		public string Email { get; set; }
		public string Password { get; set; }
		public string? Name { get; set; }

	}
	public class EmployeeDto
	{
		public int EmployeeId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string? Password { get; set; }
		public string MobileNo { get; set; }
		[ForeignKey("DesignationId")]
		public int DesignationId { get; set; }
		public DesignationDto? Designation { get; set; }
		[ForeignKey("DepartmentId")]
		public int DepartmentId { get; set; }
		public DepartmentDto? Department { get; set; }
		public int? ManagerId { get; set; }
		public DateTime DateOfJoining { get; set; }
		public DateTime? DateofLeaving { get; set; }
		public DateTime CreatedAt { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime UpdatedAt { get; set; }
		public int? UpdatedBy { get; set; }
		public bool IsActive { get; set; } = true;
	}

}
