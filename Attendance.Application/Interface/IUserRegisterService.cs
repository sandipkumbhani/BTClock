using Attendance.Domain.Models;

namespace Attendance.Application.Interface
{
	public interface IUserRegisterService
	{
		Task<Users> RegisterUserAsync(Users userRegister);
		Task<Users?> AuthenticateUserAsync(string email, string password);
		Task<bool> IsEmailRegisteredAsync(string email);
	}
}
