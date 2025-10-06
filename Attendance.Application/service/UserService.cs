using Attendance.Application.Interface;
using Attendance.Domain.Interfaces;
using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.service
{
   public class UserService : IUserService
	{
		private readonly IUserAdaptor _userAdaptor;
		public UserService(IUserAdaptor userAdaptor)
		{
			_userAdaptor = userAdaptor;
		}
		public async Task<UserDto> GetUserById(int userId)
		{
			return await _userAdaptor.GetUserByIdAsync(userId);
		}
		public async Task<List<UserDto>> GetAllUser()
		{
			return await _userAdaptor.GetAllUserAsync();
		}
		public async Task<string> AddUser(UserDto userDto)
		{
			return await _userAdaptor.AddUserAsync(userDto);
		}
		public async Task<string> UpdateUser(UserDto userDto, int userId)
		{
			return await _userAdaptor.UpdateUserAsync(userDto, userId);
		}
		public async Task<int> DeleteUser(int userId)
		{
			return await _userAdaptor.DeleteUserAsync(userId);
		}
	}
}
