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
    public class CommanResponseDto
    {
        public int? StatusCode { get; set; }

        public object Data { get; set; }

        public string? Message { get; set; }

        public string? ErrorMessage { get; set; }
    }
    public class CommonFailureDto
    {
        public string Error { get; set; }
    }
}
