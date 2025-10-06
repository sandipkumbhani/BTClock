using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Application.Interface
{
    public interface IUserService
    {
        Task<UserDto> GetUserById(int userId);
		Task<List<UserDto>> GetAllUser();
		Task<string> AddUser(UserDto userDto);
		Task<string> UpdateUser(UserDto userDto, int userId);
		Task<int> DeleteUser(int userId);
	}
}
