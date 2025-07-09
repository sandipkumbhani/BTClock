using Attendance.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Attendance.Domain.Interfaces
{
   public interface IUserRegisterRepository
    {
        Task<Users> AddUserRegisterAsync(Users userRegister);
        Task<bool> EmailExitsAsync(string email);
        Task<IEnumerable<Users>> GetAllUsersAsync();
    }
}
