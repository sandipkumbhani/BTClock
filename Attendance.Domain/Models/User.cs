using System.ComponentModel.DataAnnotations.Schema;

namespace Attendance.Domain.Models
{
    public  class User
    {
		public User()
		{
			userDto = new List<UserDto>();
		}
		public List<UserDto> userDto { get; set; }

	}
	public class UserDto
	{
		public int UserId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string Password { get; set; }
		[ForeignKey("RoleId")]
		public int RoleId { get; set; }
		public Role? Role { get; set; }
		public int PerentId { get; set; }
		public bool IsActive { get; set; } = true;
		public DateTime CreatedAt { get; set; }
		public int? CreatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public int? UpdatedBy { get; set; }
	}
}
