using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
    public interface IUserAdaptor
    {
        Task<UserDto> GetUserByIdAsync(int userId);
		Task<List<UserDto>> GetAllUserAsync();
		Task<string> AddUserAsync(UserDto userDto);
		Task<string> UpdateUserAsync(UserDto userDto, int userId);
		Task<int> DeleteUserAsync(int userId);
	}
}
