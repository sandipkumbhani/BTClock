using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using System;
using System.Threading.Tasks;

namespace Attendance.Application.Provider
{
	   public class UserRegisterService : IUserRegisterService
	{
		private readonly IUserRegisterRepository _userRegisterRepository;

		public UserRegisterService(IUserRegisterRepository userRegisterRepository)
		{
			_userRegisterRepository = userRegisterRepository;
		}

	   public async Task<Users> RegisterUserAsync(Users userRegister)
	   {
		   if (await IsEmailRegisteredAsync(userRegister.Email))
		   {
			   throw new InvalidOperationException("Email is already registered.");
		   }

		   userRegister.CreatedOn = DateTime.UtcNow;
		   userRegister.IsActive = true;

		   return await _userRegisterRepository.AddUserRegisterAsync(userRegister);
	   }

	   public async Task<Users?> AuthenticateUserAsync(string email, string password)
	   {
		   // Simple authentication: check if user exists and password matches
		   var users = await _userRegisterRepository.GetAllUsersAsync();
		   return users.FirstOrDefault(u => u.Email == email && u.Password == password && u.IsActive);
	   }

		public async Task<bool> IsEmailRegisteredAsync(string email)
		{
			return await _userRegisterRepository.EmailExitsAsync(email);
		}
	}
}
