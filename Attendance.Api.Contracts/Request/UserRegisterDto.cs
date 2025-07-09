using System.ComponentModel.DataAnnotations;

namespace Attendance.Api.Contracts.Request
{
	   public class UserRegisterDto
	   {
			   [Required(ErrorMessage = "Email is required.")]
			   [EmailAddress(ErrorMessage = "Invalid email format.")]
			   public string? Email { get; set; }

			   [Required(ErrorMessage = "Password is required.")]
			   [MinLength(6, ErrorMessage = "Password must be at least 6 characters long.")]
			   public string? Password { get; set; }

			   [Required(ErrorMessage = "Confirm Password is required.")]
			   [Compare("Password", ErrorMessage = "Passwords do not match.")]
			   public string? ConfirmPassword { get; set; }
	   }
}
