using System.ComponentModel.DataAnnotations;
namespace Attendance.Domain.Models
{
	public class Users
	{
		[Key]
		public int UserId { get; set; }
		public string? Email { get; set; }
		public string? Password { get; set; }
		public DateTime CreatedOn { get; set; }
		public bool IsActive { get; set; } = true;
		
	}
}
