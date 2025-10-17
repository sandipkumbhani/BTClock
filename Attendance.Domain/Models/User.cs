using Attendance.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Domain.Models
{
    public class User
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Name { get; set; }
    }


    public class UserDto
    {
        public int UserId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        [ForeignKey("RoleId")]
        public int RoleId { get; set; }
        public RoleDto? Role { get; set; }
        public int? ParentId { get; set; }

        [ForeignKey("CompanyId")]
        public int? CompanyId { get; set; }
        public CompanyDto? Company { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime? CreatedAt { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
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